

using System.Collections.Generic;
using UnityEngine;

public class Cavalry : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();

        locations.Add(new Vector2Int(gridPoint.x - 1, gridPoint.y + 2));
        locations.Add(new Vector2Int(gridPoint.x + 1, gridPoint.y + 2));

        locations.Add(new Vector2Int(gridPoint.x + 2, gridPoint.y + 1));
        locations.Add(new Vector2Int(gridPoint.x - 2, gridPoint.y + 1));

        locations.Add(new Vector2Int(gridPoint.x + 2, gridPoint.y - 1));
        locations.Add(new Vector2Int(gridPoint.x - 2, gridPoint.y - 1));

        locations.Add(new Vector2Int(gridPoint.x + 1, gridPoint.y - 2));
        locations.Add(new Vector2Int(gridPoint.x - 1, gridPoint.y - 2));

        return locations;
    }

	public override void setup_attributes()
	{
        name = "Cavalry";
        attack = 3;
		defense = 2;
		health = 4;
		maxHealth = 4;
        rangeNum = 4;
        defaultRange = 4;
        specialMessage = "This cavalry moved into a forest, but it is immune to terrain restrictions.";
        description = "Horses are considered precious cattles during war. Therefore, commanders need to carefully decide how to use them.";
        healthBar.SetHealth(health);
        healthBar.SetMaxHealth(maxHealth);
    }
}
