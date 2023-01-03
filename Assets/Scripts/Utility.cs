using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static int UItoBoardpos(Vector3 pos)
	{
		return (int)pos.x + (int)pos.y * 8;
	}

	public static Vector2 BoardtoUIpos(int pos)
	{
		Vector2 position = new Vector2
		{
			x = pos % 8 + 0.5f,
			y = pos / 8 + 0.5f
		};
		return position;
	}
}
