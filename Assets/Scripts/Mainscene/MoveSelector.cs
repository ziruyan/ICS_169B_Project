	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MonoBehaviour
{
    public GameObject moveLocationPrefab;
    public GameObject tileHighlightPrefab;
    public GameObject attackLocationPrefab;

    private GameObject tileHighlight;
    private GameObject movingPiece;
    private List<Vector2Int> moveLocations;
    private List<GameObject> locationHighlights;

	private Vector2Int battleGridPoint;

    void Start ()
    {
		// 开始时是关闭状态，要等Tile转入
        this.enabled = false;
        tileHighlight = Instantiate(tileHighlightPrefab, GameManager.instance.PointFromGrid(new Vector2Int(0, 0)),
            Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);
    }

    void Update ()
    {
		// 进入运行状态后，一样是用Ray去判定，逻辑大部分同理Tile
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
			Vector3 point = hit.point;
			Vector2Int gridPoint = GameManager.instance.GridFromPoint(point);

			tileHighlight.SetActive(true);
			tileHighlight.transform.position = GameManager.instance.PointFromGrid(gridPoint);

            if (Input.GetMouseButtonDown(0))
            {

				// 如果Move不合法，其实应该转Cancel
                // Reference Point 2: check for valid move location
                if (!moveLocations.Contains(gridPoint))
                {
					tileHighlight.SetActive(false);
					CancelMove();
                    return;
                }

				// 检测是否有棋子，如果没有就正常Move
                if (GameManager.instance.PieceAtGrid(gridPoint) == null)
                {
                    GameManager.instance.Move(movingPiece, gridPoint);
                    GameManager.instance.LocationCheck(movingPiece);
					ExitState();
                }
                else
                {
					battleGridPoint = gridPoint;
					GameObject battlePiece = GameManager.instance.PieceAtGrid(gridPoint);
					GameManager.instance.SaveTwoPieces(movingPiece, battlePiece);
					// 如果目标位置有棋子，就先进入GM的Capture去做Battle判定(摧毁棋子)
					// 然后再Move
					// 调出面板，然后靠两个Button调用两个不同的Function来推动Battle
					GameManager.instance.ShowCombatPanel();
/*
					int checkdestory = GameManager.instance.CapturePieceAt(movingPiece,gridPoint);
					if (checkdestory == 1)
					{
						GameManager.instance.Move(movingPiece, gridPoint);
					}
*/
                }
                // Reference Point 3: capture enemy piece here later
                // ExitState();
            }
        }
        else
        {
            tileHighlight.SetActive(false);
        }
    }

	public void BattleStart()
	{
		int checkdestory = GameManager.instance.CapturePieceAt(movingPiece,battleGridPoint);
		if (checkdestory == 1)
		{
			GameManager.instance.Move(movingPiece, battleGridPoint);
		}
		GameManager.instance.CloseCombatPanel();
		ExitState();
	}

	public void BattleCancel()
	{
		GameManager.instance.CloseCombatPanel();
		tileHighlight.SetActive(false);
		CancelMove();
	}

    private void CancelMove()
    {
		// 关闭程序
        this.enabled = false;

		// 删除高亮
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }

		// 在GM里取消选取，然后返回Tile
        GameManager.instance.DeselectPiece(movingPiece);
        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();
    }

	// 从外部Tile那边进入，包含Tile那边选中的Piece
    public void EnterState(GameObject piece)
    {
		// 启动这边的程序
        movingPiece = piece;
        this.enabled = true;

		// 进入的时候就从GM那边获取这次Move的Valid Tile
        moveLocations = GameManager.instance.MovesForPiece(movingPiece);
        locationHighlights = new List<GameObject>();

		// 如果没有合理的Move，就直接退出
        if (moveLocations.Count == 0)
        {
            CancelMove();
        }

		// 高亮所有的可到达的格子
        foreach (Vector2Int loc in moveLocations)
        {
            GameObject highlight;
			// 如果格子里有棋子，就需要红色高亮，没有就蓝色
            if (GameManager.instance.PieceAtGrid(loc))
            {
                highlight = Instantiate(attackLocationPrefab, GameManager.instance.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
            }
            else
            {
                highlight = Instantiate(moveLocationPrefab, GameManager.instance.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
            }
			// 加入到高亮列表之中
            locationHighlights.Add(highlight);
        }
    }

    private void ExitState()
    {
		// 从Update进入
        this.enabled = false;
		// 删除高亮、取消GM的选择
        TileSelector selector = GetComponent<TileSelector>();
        tileHighlight.SetActive(false);
        GameManager.instance.DeselectPiece(movingPiece);
        movingPiece = null;
		// 下一回合
        GameManager.instance.NextPlayer();
		// 跳转
        selector.EnterState();
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }
    }
}
