using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    public GameObject[,] pieces;
    public Player currentPlayer;
    public GameObject current_p;
    public Vector2Int current_loc;

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
/*
    public GameObject get_move_piece(GameManager instance)
    {
        bool check = true;
        //int i = 0;

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
*/

    public GameObject get_move_piece(GameManager instance)
    {
        bool check = true;
        //int i = 0;

        GameObject pie = get_first_attackable_piece(instance);
        if (pie != null)
        {
            Debug.Log("attackable 1");
            current_p = pie;
            return current_p;
        }

        while (check)
        {
            int len1 = currentPlayer.pieces.Count;
            int target1 = Random.Range(0, len1);

            List<Vector2Int> loc = instance.MovesForPiece(currentPlayer.pieces[target1]);
            if (loc.Count > 0)
            {
                check = false;
                current_p = currentPlayer.pieces[target1];
                int len2 = loc.Count;
                int target2 = Random.Range(0, len2);
                current_loc = loc[target2];
            }
        }
        Debug.Log("normal 1");
        return current_p;
    }

    public GameObject get_first_attackable_piece(GameManager instance)
    {
        int len1 = currentPlayer.pieces.Count;
        int[] values = Enumerable.Range(0, len1).ToArray();
        int l1 = values.Length;
        bool check = false;

        for (int i = 0; i < l1; i++)
        {
            List<Vector2Int> loc = instance.MovesForPiece(currentPlayer.pieces[values[i]]);
            if (loc.Count > 0)
            {
                int len2 = loc.Count;
                Debug.Log(len2);
                for (int j = 0; j < len2; j++)
                {
                    GameObject pie2 = instance.PieceAtGrid(loc[j]);
                    //if (pie2 != null && currentPlayer.pieces.Contains(pie2) is false)
                    if (pie2 != null)
                    {
                        Debug.Log("check2");
                        current_p = currentPlayer.pieces[values[i]];
                        current_loc = loc[j];
                        check = true;
                        break;
                    }
                }
                if (check)
                {
                    break;
                }
                
            }
        }

        if (check == true)
        {
            return current_p;
        }
        else
        {
            return null;
        }

    }

/*
    public Vector2Int get_move_place(GameManager instance)
    {
        List<Vector2Int> loc = instance.MovesForPiece(current_p);
        int len1 = loc.Count;
        int target1 = Random.Range(0, len1);
        return loc[target1];
    }

*/
    public Vector2Int get_move_place(GameManager instance)
    {
        return current_loc;
    }



}

