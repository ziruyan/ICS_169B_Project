using System.Collections.Generic;
using UnityEngine;

public class Player
{
	// Player会标记还有多少Piece,已经杀了对面多少Piece
    public List<GameObject> pieces;
    public List<GameObject> capturedPieces;

	// 名字和移动方向
    public string name;
    public int forward;

    public Player(string name, bool positiveZMovement)
    {
        this.name = name;
        pieces = new List<GameObject>();
        capturedPieces = new List<GameObject>();

        if (positiveZMovement == true)
        {
            this.forward = 1;
        }
        else
        {
            this.forward = -1;
        }
    }

	public string PieceToString()
	{
		string newString = "";
		int pawn_count = 0;
		int bishop_count = 0;
		int king_health = 0;
		foreach (GameObject pi in pieces)
		{
			if (pi.GetComponent<Piece>().type == PieceType.Rifleman)
			{
				pawn_count += 1;
			}
			if (pi.GetComponent<Piece>().type == PieceType.Pikeman)
			{
				bishop_count += 1;
			}
			if (pi.GetComponent<Piece>().type == PieceType.Admiral)
			{
				king_health = pi.GetComponent<Piece>().health;
			}
		}

		newString += pawn_count.ToString() + " Soldiers Remains\n";
		newString += bishop_count.ToString() + " Cannon Remains\n";
		newString += pawn_count.ToString() + " General Health Remains";
		return newString;
	}
	
}
