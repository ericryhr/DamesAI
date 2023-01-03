using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class PieceUIManager : MonoBehaviour
{
    #region Singleton

    public static PieceUIManager instance;

    void Awake()
    {
        instance = this;
        pieces = new List<GameObject>();
    }

    #endregion

    public Sprite[] pieceSprites;
    public GameObject piecePrefab;
    List<GameObject> pieces;

    public void RegenerateUIBoard()
	{
        DestroyPreviousBoard();

		for (int i = 0; i < Board.Square.Length; i++)
		{
            int piece = Board.Square[i];
            if (piece == 0) continue;

            GameObject pieceToSpawn = Instantiate(piecePrefab);
            pieceToSpawn.transform.parent = transform;
            SpriteRenderer pieceRenderer = pieceToSpawn.GetComponent<SpriteRenderer>();

            switch (piece)
            {
                case 2:
                    pieceRenderer.sprite = pieceSprites[0];
                    pieceToSpawn.name = "BlancPeo";
                    break;
                case 3:
                    pieceRenderer.sprite = pieceSprites[1];
                    pieceToSpawn.name = "BlancDama";
                    break;

                case 4:
                    pieceRenderer.sprite = pieceSprites[2];
                    pieceToSpawn.name = "NegrePeo";
                    break;
                case 5:
                    pieceRenderer.sprite = pieceSprites[3];
                    pieceToSpawn.name = "NegreDama";
                    break;
            }

            pieceToSpawn.GetComponent<DragDrop>().piece = piece;
            pieceToSpawn.transform.position = BoardtoUIpos(i);
            pieces.Add(pieceToSpawn);
        }
	}

    void DestroyPreviousBoard()
	{
		foreach (GameObject obj in pieces)
		{
            Destroy(obj);
		}
        pieces = new List<GameObject>();
	}
}
