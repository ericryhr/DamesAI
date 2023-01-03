using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Piece
{
    public const int Peo = 0;
    public const int Dama = 1;
    public const int Blanc = 2;
    public const int Negre = 4;

    const int typeMask = 0b001;
    const int blancMask = 0b10;
    const int negreMask = 0b100;
    const int colorMask = blancMask | negreMask;

    public static bool EsColor(int piece, int color)
	{
        return (piece & colorMask) == color;
	}

    public static int Color(int piece)
	{
        return piece & colorMask;
	}

    public static int AltreColor(int color)
	{
        if ((color & colorMask) == Blanc) return Negre;
        else return Blanc;
	}

    public static bool EsTipus(int piece, int tipus)
	{
        return (piece & typeMask) == tipus;
	}
}
