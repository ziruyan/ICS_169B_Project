using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public GameObject tileHighlightPrefab;

    private GameObject tileHighlight;

    void Start ()
    {
		// 创建记录鼠标交互地点
        Vector2Int gridPoint = GameManager.instance.GridPoint(0, 0);
        Vector3 point = GameManager.instance.PointFromGrid(gridPoint);
		// 光标模型
        tileHighlight = Instantiate(tileHighlightPrefab, point, Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);
    }

    void Update ()
    {
		// 选取方式，创建一个摄像头->鼠标位置的射线，来检测这个射线第一个经过的Object
		// 这个object必须是Physics Colliders
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
		// Raycast会做这个检测，返回交汇点
        if (Physics.Raycast(ray, out hit))
        {
			// 获取交汇点，存入Grid Point
            Vector3 point = hit.point;
            Vector2Int gridPoint = GameManager.instance.GridFromPoint(point);

			// 提前创建好HighLight的模型，然后在鼠标移到位置后激活
            tileHighlight.SetActive(true);
			// 并把HighLight的模型移动到目标位置(具体的方格方法在Geometry里面写)
            tileHighlight.transform.position = GameManager.instance.PointFromGrid(gridPoint);
			// 点击检测
            if (Input.GetMouseButtonDown(0))
            {
				// 检测点击位置，如果有棋子，就进入移动模式。
                GameObject selectedPiece = GameManager.instance.PieceAtGrid(gridPoint);
				// 确认移动合理（伪TurnBase）
                if (GameManager.instance.DoesPieceBelongToCurrentPlayer(selectedPiece))
                {
                    GameManager.instance.SelectPiece(selectedPiece);
                    // Reference Point 1: add ExitState call here later
					// 转到Move Selector那边去处理移动
                    ExitState(selectedPiece);
                }
            }
        }
        else
        {
			// 没有交汇点就取消
            tileHighlight.SetActive(false);
        }
    }

	// 外部调用的Function，把程序回到Tile的Process
    public void EnterState()
    {
        enabled = true;
    }

    private void ExitState(GameObject movingPiece)
    {
		// 关掉Tile这边的系统，进入Move Piece的系统
        this.enabled = false;
        tileHighlight.SetActive(false);
        MoveSelector move = GetComponent<MoveSelector>();
		// 这里要把选中的棋子传递到MP去
        move.EnterState(movingPiece);
    }
}
