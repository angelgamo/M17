using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(prova))]
public class Edit : Editor
{

	public override void OnInspectorGUI()
	{
		prova mapGen = (prova)target;

		if (DrawDefaultInspector())
		{
			//mapGen.GeneracioDeMapa();
		}

		if (GUILayout.Button("Generate"))
		{
			mapGen.GeneracioDeMapa();
		}
		if (GUILayout.Button("Clear"))
		{
			mapGen.Clear();
		}
	}
}
	