using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject[,] pieces;
    public Player currentPlayer;
    public GameObject current_p;

    public static System.Random rd = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void input_board(GameObject[,] p)
    {
        pieces = p;
    }

    public void input_enemy(Player p)
    {
        currentPlayer = p;
    }

    public GameObject get_move_piece(GameManager instance)
    {
        bool check = true;
        int i = 0;

        while (check)
        {
            int len1 = currentPlayer.pieces.Count;
            int target1 = Random.Range(0, len1);

            List<Vector2Int> loc = instance.MovesForPiece(currentPlayer.pieces[target1]);
            if (loc.Count > 0)
            {
                check = false;
                current_p = currentPlayer.pieces[target1];
            }
        }

        return current_p;
    }

    public Vector2Int get_move_place(GameManager instance)
    {
        List<Vector2Int> loc = instance.MovesForPiece(current_p);
        int len1 = loc.Count;
        int target1 = Random.Range(0, len1);
        return loc[target1];
    }
}
