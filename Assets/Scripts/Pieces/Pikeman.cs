

using System.Collections.Generic;
using UnityEngine;

public class Pikeman : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();

        foreach (Vector2Int dir in RookDirections)
        {
            Debug.Log("It's range is" + rangeNum);
            for (int i = 1; i < rangeNum; i++)
            {
                Vector2Int nextGridPoint = new Vector2Int(gridPoint.x + i * dir.x, gridPoint.y + i * dir.y);
                locations.Add(nextGridPoint);
                if (GameManager.instance.PieceAtGrid(nextGridPoint))
                {
                    break;
                }
            }
        }

        return locations;
    }

	public override void setup_attributes()
	{
		name = "Pikeman";
		is_range = 0;
		attack = 1;
		defense = 0;
		health = 3;
		maxHealth = 3;
        defaultRange = 3;
        rangeNum = 3;
        specialMessage = "This infantry moved into a forest, range is reduced.";
        description = "They are the great man who volunteered to fight for their home and bleed for their belief.";
        healthBar.SetHealth(health);
        healthBar.SetMaxHealth(maxHealth);
    }
}
