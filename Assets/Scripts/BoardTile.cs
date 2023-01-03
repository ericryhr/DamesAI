using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
	public int index;
	MeshRenderer tileRenderer;
	Color tileColor;
	Color highlightColor;
	//Lerp
	public float duration = .1f;
	float t = 2;
	Color startColor;
	Color endColor;

	void Start()
	{
		tileRenderer = GetComponent<MeshRenderer>();
		tileColor = tileRenderer.material.color;
		highlightColor = GraphicsBoard.instance.highlightColor;
	}

	void Update()
	{
		if(t <= 1)
		{
			tileRenderer.material.color = Color.Lerp(startColor, endColor, t);
			t += Time.deltaTime / duration;
		}
	}

	void OnMouseEnter()
	{
		startColor = tileColor;
		endColor = highlightColor;
		t = 0;
	}

	void OnMouseExit()
	{
		startColor = highlightColor;
		endColor = tileColor;
		t = 0;
	}
}
