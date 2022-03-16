

using System.Collections.Generic;
using UnityEngine;

public class Cannon : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(BishopDirections);
        directions.AddRange(RookDirections);

        foreach (Vector2Int dir in directions)
        {
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
        name = "Cannon";
        attack = 4;
		defense = 0;
		health = 2;
		maxHealth = 2;
        rangeNum = 6;
        defaultRange = 6;
        specialMessage = "This canon moved into a forest, its range is reduced.";
        description = "The M1857 12-pounder cannon is a powerful weapon. While it can do great damage to enemies, but also vulnerable to attacks.";
        healthBar.SetHealth(health);
        healthBar.SetMaxHealth(maxHealth);
    }
}
