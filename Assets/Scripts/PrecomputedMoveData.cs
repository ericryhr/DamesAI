using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrecomputedMoveData
{
    // N, S, E, W, NW, NE, SW, SE
    public static readonly int[] DirectionOffsets = { 8, -8, 1, -1, 7, 9, -7, -9 };
    public static readonly int[][] NumSquaresToEdge;

    static PrecomputedMoveData()
	{
		NumSquaresToEdge = new int[64][];
		for (int columna = 0; columna < 8; columna++)
		{
			for (int fila = 0; fila < 8; fila++)
			{
				int n = 7 - fila;
				int s = fila;
				int e = 7 - columna;
				int w = columna;

				int index = fila * 8 + columna;

				NumSquaresToEdge[index] = new int[8]{
					n,
					s,
					e,
					w,
					System.Math.Min(n, w),
					System.Math.Min(n, e),
					System.Math.Min(s, e),
					System.Math.Min(s, w)
				};
			}
		}
	}
}
