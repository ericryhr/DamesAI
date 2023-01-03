using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GraphicsBoard))]
public class GraphicBoardEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GraphicsBoard graphics = (GraphicsBoard)target;

		GUILayout.BeginHorizontal();

		if(GUILayout.Button("Generate Board"))
		{
			graphics.CreateGraphicalBoard();
		}

		GUILayout.EndHorizontal();
	}
}
