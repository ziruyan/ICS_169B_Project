
using System.Collections.Generic;
using UnityEngine;

public class Rifleman : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
		List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(BishopDirections);
        directions.AddRange(RookDirections);

        foreach (Vector2Int dir in directions)
        {
			Vector2Int nextGridPoint = new Vector2Int(gridPoint.x + dir.x, gridPoint.y + dir.y);

			//Vector2Int nextGridPoint1 = new Vector2Int(gridPoint.x + dir.x+1, gridPoint.y + dir.y+1);

			//Vector2Int nextGridPoint2 = new Vector2Int(gridPoint.x + dir.x+1, gridPoint.y + dir.y-1);

			//Vector2Int nextGridPoint3 = new Vector2Int(gridPoint.x + dir.x-1, gridPoint.y + dir.y-1);

			//Vector2Int nextGridPoint4 = new Vector2Int(gridPoint.x + dir.x-1, gridPoint.y + dir.y+1);

			locations.Add(nextGridPoint);
			//locations.Add(nextGridPoint1);
			//locations.Add(nextGridPoint2);
			//locations.Add(nextGridPoint3);
			//locations.Add(nextGridPoint4);
        }

        return locations;
    }

	public override void setup_attributes()
	{
		name = "Rifleman";
		is_range = 0;
		attack = 2;
		defense = 0;
		health = 3;
		maxHealth = 3;
		rangeNum = 3;
		defaultRange = 3;
		specialMessage = "This pikeman moved into a forest, its range is reduced.";
		description = "These infantries are equipped with the Springfield Rifles and may deal a decent damage in battle.";
		healthBar.SetHealth(health);
		healthBar.SetMaxHealth(maxHealth);
	}
}
