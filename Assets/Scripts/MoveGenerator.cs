using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PrecomputedMoveData;

public static class MoveGenerator
{
	static bool foundCaptureMove;

	//Donats un color que ha de moure i si en el torn anterior s'ha matat alguna peça, genera una llista de moviments
    public static List<Move> GenerateMoves(int colorToMove, int captura)
	{
		List<Move> moves = new List<Move>();
		foundCaptureMove = false;

		for (int originalPos = 0; originalPos < 64; originalPos++)
		{
			int piece = Board.Square[originalPos];
			if(Piece.EsColor(piece, colorToMove))
			{
				if(captura == -1 || captura == originalPos) GenerateMoves(ref moves, originalPos, piece, colorToMove, captura);
			}
		}

		return moves;
	}

	//Killer controla si una peça ja ha matat a algu en el torn anterior per nomes generar moviments per aquella peça
	static void GenerateMoves(ref List<Move> moves, int originalPos, int piece, int colorToMove, int captura)
	{
		int startingIndex = 4;
		int endingIndex = 7;
		//Si es un peo negre nomes comprovarem les direccions sud
		if (Piece.EsColor(piece, Piece.Negre) && Piece.EsTipus(piece, Piece.Peo)) startingIndex += 2;
		//Si es un peo blanc nomes comprovarem les direccions nord
		if (Piece.EsColor(piece, Piece.Blanc) && Piece.EsTipus(piece, Piece.Peo)) endingIndex -= 2;

		//Moviments laterals
		for (int i = startingIndex; i <= endingIndex; i++)
		{
			if (NumSquaresToEdge[originalPos][i] > 0)
			{
				int endPos = originalPos + DirectionOffsets[i];
				int pieceOnEndPos = Board.Square[endPos];
				//Si la casella es buida i no vinc de matar ningu
				if (pieceOnEndPos == 0 && captura == -1 && !foundCaptureMove)
				{
					if (PosicioPerEvolucionar(piece, endPos)) moves.Add(new Move(originalPos, endPos, false, true, colorToMove));
					else moves.Add(new Move(originalPos, endPos, colorToMove));
				}
				else if (pieceOnEndPos != 0 && !Piece.EsColor(pieceOnEndPos, colorToMove) && NumSquaresToEdge[originalPos][i] > 1)
				{
					int jumpPos = endPos + DirectionOffsets[i];
					int pieceOnJumpPos = Board.Square[jumpPos];
					if (pieceOnJumpPos == 0)
					{
						//Trobem un moviment que mata i, per tant, hem de borrar tots els que no maten
						if (!foundCaptureMove)
						{
							moves = new List<Move>();
							foundCaptureMove = true;
						}
						if (PosicioPerEvolucionar(piece, jumpPos)) moves.Add(new Move(originalPos, jumpPos, true, true, colorToMove));
						else moves.Add(new Move(originalPos, jumpPos, true, false, colorToMove));
					}
				}
			}
		}
	}

	static bool PosicioPerEvolucionar(int piece, int pos)
	{
		if (Piece.EsColor(piece, Piece.Blanc) && pos >= 56) return true;
		else if (Piece.EsColor(piece, Piece.Negre) && pos <= 7) return true;
		return false;
	}
}

public struct Move
{
    public readonly int originalPos;
    public readonly int newPos;
	public readonly int color;
	public readonly bool haCapturat;
	public readonly bool haEvolucionat;

	public Move(int originalPos, int newPos, int color)
	{
		this.originalPos = originalPos;
		this.newPos = newPos;
		haCapturat = false;
		haEvolucionat = false;
		this.color = color;
	}
	
	public Move(int originalPos, int newPos, bool haCapturat, bool haEvolucionat, int color)
	{
		this.originalPos = originalPos;
		this.newPos = newPos;
		this.color = color;
		this.haCapturat = haCapturat;
		this.haEvolucionat = haEvolucionat;
	}

	public override bool Equals(object obj) => obj is Move other && this.Equals(other);

	public bool Equals(Move a)
	{
		return originalPos == a.originalPos && newPos == a.newPos;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static bool operator ==(Move a, Move b)
	{
		return a.originalPos == b.originalPos && a.newPos == b.newPos;
	}
	
	public static bool operator !=(Move a, Move b)
	{
		return a.originalPos != b.originalPos || a.newPos != b.newPos;
	}

	public void printMove()
	{
		Debug.Log("OriginalPos: " + originalPos + " NewPos: " + newPos + " Mata: " + haCapturat + " Evoluciona: " + haEvolucionat);
	}
}
