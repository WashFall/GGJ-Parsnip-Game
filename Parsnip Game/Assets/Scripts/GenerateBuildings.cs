using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBuildings : MonoBehaviour
{
	[SerializeField] private GameObject m_topPrefab;
	[SerializeField] private GameObject[] m_middlePrefab;
	[SerializeField] private GameObject m_bottomPrefab;
	[SerializeField] private Vector2Int m_amountOfBuildingBlocks;
	[SerializeField] private int m_buildingDensity;
	[SerializeField] private BoxCollider m_buildingBounds;

	private List<PlacedBuildingSpots> m_placedBuildings = new();
	private List<GameObject> m_buildings = new();
	private int m_amountOfPlacementTries = 10;
	private float m_distanceBetweenBlocks;

	private struct PlacedBuildingSpots
	{
		public Vector3 m_placement;
		private const float k_closeRadious = 2f;

		public PlacedBuildingSpots(Vector3 placement)
		{
			m_placement = placement;
		}

		public bool CanPlaceHere(Vector3 newPlacment)
		{
			Vector3 offset = newPlacment - m_placement;
			if (offset.sqrMagnitude < k_closeRadious * k_closeRadious)
			{
				Debug.Log("To Close");
				return false;
			}

			return true;
		}
	}

	public void Generate()
	{
		for (int i = 0; i < m_buildingDensity; i++)
		{
			Vector3 newBuildingPlacement = Vector3.zero;
			GetNewPlacement(ref newBuildingPlacement);

			if (newBuildingPlacement == Vector3.zero)
			{
				continue;
			}

			m_placedBuildings.Add(new PlacedBuildingSpots(newBuildingPlacement));
			int deltaAmountOfBlocks = Random.Range(m_amountOfBuildingBlocks.x, m_amountOfBuildingBlocks.y) - 2;

			GameObject parentObject = new GameObject($"Building {i}")
			{
				transform =
				{
					position = newBuildingPlacement
				}
			};

			m_buildings.Add(parentObject);

			GameObject bottom = Instantiate(m_bottomPrefab, parentObject.transform);
			m_distanceBetweenBlocks = bottom.transform.localScale.y;
			float m_deltaDistanceBetweenBlocks = m_distanceBetweenBlocks;
			bottom.transform.localPosition = Vector3.zero;

			for (int j = 0; j < deltaAmountOfBlocks; j++)
			{
				GameObject middle = Instantiate(m_middlePrefab[Random.Range(0, m_middlePrefab.Length)],
					parentObject.transform);
				middle.transform.localPosition = new Vector3(0, m_deltaDistanceBetweenBlocks, 0);
				m_deltaDistanceBetweenBlocks = m_distanceBetweenBlocks * (j + 1);
			}

			GameObject top = Instantiate(m_topPrefab, parentObject.transform);
			top.transform.localPosition = new Vector3(0, m_deltaDistanceBetweenBlocks, 0);
		}
	}

	private void GetNewPlacement(ref Vector3 newBuildingPlacement)
	{
		for (int i = 0; i < m_amountOfPlacementTries; i++)
		{
			bool canPlace = true;
			float x = Random.Range(m_buildingBounds.bounds.min.x, m_buildingBounds.bounds.max.x);
			float z = Random.Range(m_buildingBounds.bounds.min.z, m_buildingBounds.bounds.max.z);

			newBuildingPlacement = new Vector3(x, 0, z);
			
			for (int j = 0; j < m_placedBuildings.Count; j++)
			{
				if (!m_placedBuildings[j].CanPlaceHere(newBuildingPlacement)) ;
				{
					canPlace = false;
					break;
				}
			}

			if (canPlace)
			{
				return;
			}
		}

		// for (int j = 0; j < m_placedBuildings.Count; j++)
		// {
		// 	if (!m_placedBuildings[j].CanPlaceHere(newBuildingPlacement)) ;
		// 	{
		// 		GetNewPlacement(ref newBuildingPlacement);
		// 	}
		// }
	}

	public void CleanUp()
	{
		foreach (GameObject building in m_buildings)
		{
			DestroyImmediate(building);
		}

		m_placedBuildings.Clear();
	}
}