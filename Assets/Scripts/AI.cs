using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class AI
{
	static readonly int negativeInfinity = -100000;
	//static readonly int positiveInfinity = 100000;
	static readonly int ValorPeo = 100;
	static readonly int ValorDama = 300;
	readonly int maxDepth;
	readonly int color;

	public AI(int color, int depth)
	{
		this.color = color;
		maxDepth = depth;
	}

	public Move ComputeRandomAIMove(int captura)
	{
		List<Move> moves = MoveGenerator.GenerateMoves(color, captura);
		int index = Random.Range(0, moves.Count);
		return moves[index];
	}

	//El tema de controlar si s'ha matat una peça en un cert torn es un cacau i algun dia ho arreglare
	public Move ComputeAIMove(int captura)
	{
		List<Move> moves = MoveGenerator.GenerateMoves(color, captura);
		Move bestMove = moves[0];
		int bestScore = negativeInfinity;
		int i = 0;
		foreach (Move move in moves)
		{
			Board.MakeBoardMove(move);

			int score;
			if(move.haCapturat)
			{
				score = -Search(maxDepth, color, move.newPos);
			}
			else
			{
				score = Search(maxDepth, Piece.AltreColor(color), -1);
			}

			Debug.Log("IT " + i + " " + score);

			if(score > bestScore)
			{
				bestScore = score;
				bestMove = move;
			}

			Board.UnmakeBoardMove(move);
			i++;
		}

		return bestMove;
	}

	//Retorna la puntuacio d'un moviment
	int Search(int depth, int color, int captura)
	{
		if (depth == 0) return Evaluate();

		List<Move> moves = MoveGenerator.GenerateMoves(color, captura);

		if (moves.Count == 0) return negativeInfinity;

		int bestEval = negativeInfinity;

		foreach (Move move in moves)
		{
			Board.MakeBoardMove(move);

			int evaluation;
			if (move.haCapturat) evaluation = Search(depth - 1, color, move.newPos);
			else evaluation = -Search(depth - 1, Piece.AltreColor(color), -1);

			bestEval = Max(evaluation, bestEval);
			Board.UnmakeBoardMove(move);
		}

		return bestEval;
	}

	//Evalua una posicio del taulell comptant les peces de cada costat
	int Evaluate()
	{
		int whiteEval = CountMaterial(Piece.Blanc);
		int blackEval = CountMaterial(Piece.Negre);
		int evaluation = whiteEval - blackEval;
		int perspective = (color == Piece.Blanc) ? 1 : -1;
		return evaluation * perspective;
	}

	//Valor de totes les peces del jugador
	int CountMaterial(int color)
	{
		int material = 0;
		for (int i = 0; i < 64; i++)
		{
			int piece = Board.Square[i];
			if (Piece.EsColor(piece, color))
			{
				if (Piece.EsTipus(piece, Piece.Peo)) material += ValorPeo;
				else material += ValorDama;
			}
		}
		return material;
	}
}
