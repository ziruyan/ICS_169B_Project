﻿using System.Collections.Generic;
using UnityEngine;

public class Admiral : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(BishopDirections);
        directions.AddRange(RookDirections);

		for (int i = 0; i < rangeNum; i++)
		{
			for (int j=0; j< rangeNum; j++)
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
		name = "Admiral";
		is_range = 0;
		attack = 3;
		defense = 2;
		health = 8;
		maxHealth = 8;
		rangeNum = 4;
		defaultRange = 4;
		specialMessage = "Your admiral moved into a forest, its range is reduced.";
		description = "The general is the commanding officer in a battle. They are the 'heart' of an army. Most battles end when the commander are gone.";
		healthBar.SetHealth(health);
		healthBar.SetMaxHealth(maxHealth);
	}
}