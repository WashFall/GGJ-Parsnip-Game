using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(GenerateBuildings))]
public class GenerateBuildingsEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		GenerateBuildings generatorScript = (GenerateBuildings)target;
		
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Generate Buildings"))
		{
			generatorScript.Generate();
		}
		if (GUILayout.Button("Clean Up Buildings"))
		{
			generatorScript.CleanUp();
		}
		EditorGUILayout.EndHorizontal();
	}
}
