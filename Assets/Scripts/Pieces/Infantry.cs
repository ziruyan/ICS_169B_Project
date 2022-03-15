
using System.Collections.Generic;
using UnityEngine;

public class Infantry : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
		List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(BishopDirections);
        directions.AddRange(RookDirections);

        foreach (Vector2Int dir in directions)
        {
            Vector2Int nextGridPoint = new Vector2Int(gridPoint.x + dir.x, gridPoint.y + dir.y);

			Vector2Int nextGridPoint1 = new Vector2Int(gridPoint.x + dir.x+1, gridPoint.y + dir.y+1);

			Vector2Int nextGridPoint2 = new Vector2Int(gridPoint.x + dir.x+1, gridPoint.y + dir.y-1);

			Vector2Int nextGridPoint3 = new Vector2Int(gridPoint.x + dir.x-1, gridPoint.y + dir.y-1);

			Vector2Int nextGridPoint4 = new Vector2Int(gridPoint.x + dir.x-1, gridPoint.y + dir.y+1);
            locations.Add(nextGridPoint);
			locations.Add(nextGridPoint1);
			locations.Add(nextGridPoint2);
			locations.Add(nextGridPoint3);
			locations.Add(nextGridPoint4);
        }

        return locations;
    }

	public override void setup_attributes()
	{
		name = "Infantry";
		is_range = 0;
		attack = 1;
		defense = 0;
		health = 2;
		maxHealth = health;
	}
}
