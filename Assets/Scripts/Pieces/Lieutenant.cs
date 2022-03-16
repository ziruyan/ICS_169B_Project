

using System.Collections.Generic;
using UnityEngine;

public class Lieutenant : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();

        foreach (Vector2Int dir in RookDirections)
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
        name = "Lieutenant";
        is_range = 1;
        attack = 3;
        defense = 2;
        health = 4;
        maxHealth = 4;
        defaultRange = 4;
        rangeNum = 4;
        specialMessage = "This Lieutenant moved into a forest, its range is reduced.";
        description = "Lieutenant is a very experienced veteran. He is well-rounded in most combat situation.";
        healthBar.SetHealth(health);
        healthBar.SetMaxHealth(maxHealth);
    }
}
