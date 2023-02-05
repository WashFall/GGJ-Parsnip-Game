using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMaterial : MonoBehaviour
{
    public Material material;

    public GameObject buildingParent;
    public GameObject[] buildingBlocks;
    // Start is called before the first frame update
    void Start()
    {
        buildingBlocks = new GameObject[buildingParent.transform.childCount];

        for (int i = 0; i < buildingParent.transform.childCount; i++)
        {
            buildingBlocks[i] = buildingParent.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < buildingBlocks.Length; i++)
        {
            float childCount = buildingBlocks[i].transform.childCount;
            for(int j = 0; j < childCount; j++)
            {
            buildingBlocks[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = material;

            }
        }
    }

}
