﻿using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(BishopDirections);
        directions.AddRange(RookDirections);

		for (int i = 0; i <4; i++)
		{
			for (int j=0; j<4; j++)
			{
				locations.Add(new Vector2Int(gridPoint.x + i, gridPoint.y + j));
				locations.Add(new Vector2Int(gridPoint.x + i, gridPoint.y - j));
				locations.Add(new Vector2Int(gridPoint.x - i, gridPoint.y - j));
				locations.Add(new Vector2Int(gridPoint.x - i, gridPoint.y + j));
			}
		}

        return locations;
    }

	public override void setup_attributes()
	{
		name = "General";
		is_range = 0;
		attack = 2;
		defense = 1;
		health = 10;
		maxHealth = health;
	}
}