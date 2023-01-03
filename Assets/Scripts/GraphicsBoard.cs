using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsBoard : MonoBehaviour
{
	#region Singleton

	public static GraphicsBoard instance;

	void Awake()
	{
		instance = this;
	}

	#endregion

	public Color lightCol;
	public Color darkCol;
	public Color highlightColor;
	[Range(-10f, 10f)]
	public float offset;
	Shader squareShader;
	public Transform[] tiles;

	void Start()
	{
		CreateGraphicalBoard();
	}

	public void CreateGraphicalBoard()
	{
		ResetBoard();
		tiles = new Transform[64];

		squareShader = Shader.Find("Unlit/Color");

		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				bool isLightSquare = (i + j) % 2 != 0;

				Color squareColor = (isLightSquare) ? lightCol : darkCol;
				Vector2 pos = new Vector2(offset + i, offset + j);

				DrawSquare(squareColor, pos, i + j*8);
			}
		}
	}

	void DrawSquare(Color squareColor, Vector2 pos, int index)
	{
		tiles[index] = GameObject.CreatePrimitive(PrimitiveType.Quad).transform;
		tiles[index].parent = transform;
		tiles[index].name = index.ToString();
		tiles[index].position = pos;
		Material squareMat = new Material(squareShader);
		squareMat.color = squareColor;
		tiles[index].GetComponent<MeshRenderer>().material = squareMat;
		tiles[index].gameObject.AddComponent<BoardTile>().index = index;
	}

	void ResetBoard()
	{
		int n = transform.childCount;
		Transform[] child = new Transform[n];
		for (int i = 0; i < n; i++)
		{
			child[i] = transform.GetChild(i);
		}

		for (int i = 0; i < n; i++)
		{
			DestroyImmediate(child[i].gameObject);
		}
	}

	public Vector3 TilePosition(int tile)
	{
		return tiles[tile].position;
	}
}
