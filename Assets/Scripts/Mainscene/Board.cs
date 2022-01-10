using UnityEngine;

public class Board : MonoBehaviour
{
	// Board不储存信息，只负责Display
    public Material defaultMaterial_red;
	public Material defaultMaterial_blue;
    public Material selectedMaterial;

	public CameraMovement CameraMovement;

	public Vector3 lastPosition;

	public GameObject help_image;
	public GameObject help_title;
	public GameObject help_content;

	void Start()
	{
		help_image.SetActive(false);
	}


    public GameObject AddPiece(GameObject piece, int col, int row)
    {
		// 这里会显示Piece，并把显示后的Object返回
        Vector2Int gridPoint = GameManager.instance.GridPoint(col, row);
        GameObject newPiece = Instantiate(piece, GameManager.instance.PointFromGrid(gridPoint), Quaternion.identity, gameObject.transform);

        return newPiece;
    }

	// 摧毁Piece
    public void RemovePiece(GameObject piece)
    {
        Destroy(piece);
    }

	// 直接位移，而非删除再产生
    public void MovePiece(GameObject piece, Vector2Int gridPoint)
    {
		// Move的话就只是Transform，可以在这里加入移动效果
        piece.transform.position = GameManager.instance.PointFromGrid(gridPoint);
    }

	// 对于被Select Piece，会换上高亮材质
    public void SelectPiece(GameObject piece)
    {
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
        renderers.material = selectedMaterial;

		lastPosition = CameraMovement.transform.position;
		
		CameraMovement.MoveTo(piece);
		
    }

	// 取消Select Piece的高亮材质
    public void DeselectPiece(GameObject piece, int player_side)
    {
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
		if (player_side == 1)
		{
			renderers.material = defaultMaterial_red;
		}
		else
		{
			renderers.material = defaultMaterial_blue;
		}

		CameraMovement.transform.position = lastPosition;
    }

	public void ShowHelp()
	{
		if (help_image.activeSelf == false)
		{
			help_image.SetActive(true);
		}
		else
		{
			help_image.SetActive(false);
		}
	}
}
