using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Board
{
    public static int[] Square;
	//Per desfer moviments
	public static Stack<int> killedPieces;

	static Board()
	{
		Square = new int[64];
		killedPieces = new Stack<int>();
	}

	public static void PlaceBoardPiece(int piece, int pos)
	{
		Square[pos] = piece;
	}

	//Retorna si algu ha matat alguna peça la seva posició
	public static void MakeBoardMove(Move move)
	{
		Square[move.newPos] = Square[move.originalPos];
		Square[move.originalPos] = 0;

		//Comprovem si han matat una peça
		if (move.haCapturat)
		{
			int killedPos = (move.newPos + move.originalPos) / 2;
			killedPieces.Push(Square[killedPos]);
			Square[killedPos] = 0;
		}

		//Comprovem si s'ha de transformar un peo en dama
		int piece = Square[move.newPos];
		if (move.haEvolucionat) Square[move.newPos] = Piece.Color(piece) | Piece.Dama;
	}

	//Nomes es pot desfer l'ultim moviment
	public static void UnmakeBoardMove(Move move)
	{
		Square[move.originalPos] = Square[move.newPos];
		Square[move.newPos] = 0;

		//Comprovem si havia matat una peça
		if (move.haCapturat)
		{
			int killedPos = (move.newPos + move.originalPos) / 2;
			Square[killedPos] = killedPieces.Pop();
		}

		//Comprovem si s'havia transformat en dama
		int piece = Square[move.originalPos];
		if (move.haEvolucionat) Square[move.originalPos] = Piece.Color(piece) | Piece.Peo;
	}

	public static void PrintBoard()
	{
		for (int i = 56; i >= 0; i-=8)
		{
			Debug.Log(Square[i] + " " + Square[i + 1] + " " + Square[i + 2] + " " + Square[i + 3] + " " + Square[i + 4] + " " + Square[i + 5] + " " + Square[i + 6] + " " + Square[i + 7]);
		}
	}
}
