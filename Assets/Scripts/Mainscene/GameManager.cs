using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

using Random = System.Random;

public class GameManager : MonoBehaviour
{
	// 创建自身的instance，保持本instance不会重新创建引起Bug
    public static GameManager instance;

	// 保证Board的连续性
    public Board board;

	// 棋子模型
    public GameObject redAdmiral;
    public GameObject redMajor;
    public GameObject redPikeman;
    public GameObject redKnight;
    public GameObject redRook;
    public GameObject redInfantry;

    public GameObject blueAdmiral;
    public GameObject blueMajor;
    public GameObject bluePikeman;
    public GameObject blueKnight;
    public GameObject blueRook;
    public GameObject blueInfantry;
    
    public GameObject CombatCube;

	// 棋子的位置列表，标记棋盘上哪里有棋子
    private GameObject[,] pieces;

	// 移动过第一次的Infantry
    private List<GameObject> movedInfantrys;

	// 最开始分配的两个玩家
    private Player red;
    private Player blue;

	// 当前轮换的两个玩家
    public Player currentPlayer;
    public Player otherPlayer;

    public EnemyAI ai;

	// 程序的棋盘是定死在x:0-7, y:0-7范围内的，之后可能要统一更改一下

	public int board_num = 8;

	// 音乐与声效
	public AudioSource source;
	public AudioClip bgm1;
	public AudioClip move_sound_effect;
	public AudioClip fight_sound_effect;

    // Combat Panel
    public GameObject CombatPanel;

	// 跳转到结束Scene
	public string end_scene_name;

	public GameObject unit_title_UI;
	public GameObject unit_info_UI;
	public GameObject unit_image_UI;

	public GameObject whoTurnIndicator;

	public GameObject Panel_Your;
	public GameObject Panel_Enemy;
	// 当前的棋盘计算是按照Global Geometry来的，意味着移动棋盘位置就会导致系统Bug，一个是要调整成Local Geometry，一个是通过Size获取棋盘大小和Public的格子数来确定具体的棋盘位置与方格。

	private GameObject p_y;
	private GameObject p_e;

	public GameObject socreBoard;
	public GameObject socreBoard_Your;
	public GameObject socreBoard_Enemy;

	public GameObject turnNum;
	public int turnCount;

    public GameObject popUpBox;
    Scene current_scene;
    string current_scene_name;


    void Awake()
    {
        instance = this;
    }

    void Start ()
    {
		// 这个实际上是整个棋盘的位置，用来记录棋盘上哪里有棋子
        pieces = new GameObject[board_num, board_num];

        // 记录哪些Infantry已经移动过了
        movedInfantrys = new List<GameObject>();

		// 创建两个Player，之后用于轮换Turn Base
        red = new Player("Red", true);
        blue = new Player("Blue", false);

        currentPlayer = red;
        otherPlayer = blue;

        ai = new EnemyAI();

		//
		source = GetComponent<AudioSource>();
		source.clip = bgm1;
		source.loop=true;
		source.playOnAwake=true;

		//source.Play();

		// 创建所有的棋子
        InitialSetup();

		unit_title_UI.SetActive(false);
		unit_info_UI.SetActive(false);
		unit_image_UI.SetActive(false);

		socreBoard.SetActive(false);

		MultiSceneManager.End_Main_Scene(1.0f,1);

		turnCount = 1;

		ShowWhoTurn();

        // Check the scene the game is currently in
        current_scene = SceneManager.GetActiveScene();
        current_scene_name = current_scene.name;
    }

	//创建所有棋子，绑定玩家，设置初始位置，调用模型
    private void InitialSetup()
    {

        AddPiece(redPikeman, red, 4, 7);
        AddPiece(redPikeman, red, 5, 7);
        AddPiece(redPikeman, red, 6, 7);
        AddPiece(redPikeman, red, 6, 6);
        AddPiece(redPikeman, red, 6, 5);


        AddPiece(redMajor, red, 8,12 );
        AddPiece(redAdmiral, red, 5, 5);

        for (int i = 7; i < 11; i++)
        {
            AddPiece(redInfantry, red, i, 14);
        }

        for (int i = 3; i < 6; i++)
        {
            AddPiece(redInfantry, red, 10, i);
        }


        AddPiece(blueAdmiral, blue, 18, 18);
        AddPiece(bluePikeman, blue, 16, 18);
        AddPiece(bluePikeman, blue, 16, 17);

        AddPiece(blueMajor, blue, 17, 4);
        AddPiece(blueInfantry, blue, 15, 2);
        AddPiece(blueInfantry, blue, 15, 3);
        AddPiece(blueInfantry, blue, 15, 4);

        AddPiece(blueMajor, blue, 8, 18);
        AddPiece(bluePikeman, blue, 7, 17);
        AddPiece(bluePikeman, blue, 9, 17);
        AddPiece(blueInfantry, blue, 7, 16);
        AddPiece(blueInfantry, blue, 8, 16);
        AddPiece(blueInfantry, blue, 9, 16);





    }

	// 加入棋子时，会需要initial 位置、模型、和所属玩家
	// 所属玩家未来会用来判定Turn Base
    public void AddPiece(GameObject prefab, Player player, int col, int row)
    {
        GameObject pieceObject = board.AddPiece(prefab, col, row);
		Piece newPiece = pieceObject.GetComponent<Piece>();
		//newPiece.setup_attributes();
        player.pieces.Add(pieceObject);
        pieces[col, row] = pieceObject;
    }

    public void SelectPieceAtGrid(Vector2Int gridPoint)
    {
        GameObject selectedPiece = pieces[gridPoint.x, gridPoint.y];
        if (selectedPiece)
        {
            board.SelectPiece(selectedPiece);
        }
    }

	// 获取所有的Valid Move
    public List<Vector2Int> MovesForPiece(GameObject pieceObject)
    {
		// 每个Piece的类型里有它的可移动范围
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
		// PieceLocation返回后，再删去不合理的
        List<Vector2Int> locations = piece.MoveLocations(gridPoint);

        // filter out offboard locations
		int tmp = board_num - 1;
        locations.RemoveAll(gp => gp.x < 0 || gp.x > tmp || gp.y < 0 || gp.y > tmp);

        // filter out locations with friendly piece
        locations.RemoveAll(gp => FriendlyPieceAt(gp));

        return locations;
    }

	// 执行实际Move的操作，这里不会检测Valid Move，要在调用前自己检测
    public void Move(GameObject piece, Vector2Int gridPoint)
    {
        Piece pieceComponent = piece.GetComponent<Piece>();
        // Infantry只有第一次可以走两格
        if (pieceComponent.type == PieceType.Infantry && !HasInfantryMoved(piece))
        {
            movedInfantrys.Add(piece);
        }

		// 把原位置的Piece移除，然后在新的位置标记一个Piece
        Vector2Int startGridPoint = GridForPiece(piece);
        pieces[startGridPoint.x, startGridPoint.y] = null;
        pieces[gridPoint.x, gridPoint.y] = piece;

		// 把移动显示在棋盘上
        board.MovePiece(piece, gridPoint);

		source.PlayOneShot(move_sound_effect,0.2F);
    }

    // 更新Infantry的第一次Move
    public void InfantryMoved(GameObject Infantry)
    {
        movedInfantrys.Add(Infantry);
    }

    // 返回Infantry的第一次Move与否
    public bool HasInfantryMoved(GameObject Infantry)
    {
        return movedInfantrys.Contains(Infantry);
    }


	// 已经在Move检测过是否有棋子了
	// 会在这里做失败判定
	// 去捕捉目标棋子，在这里改Battle
    public int CapturePieceAt(GameObject movingPiece, Vector2Int gridPoint)
    {
        GameObject pieceToCapture = PieceAtGrid(gridPoint);
		Piece pieceToCaptureComponent = pieceToCapture.GetComponent<Piece>();

		Piece movingPieceComponent = movingPiece.GetComponent<Piece>();


        //战斗，进行掉血
		movingPieceComponent.attack_piece(pieceToCaptureComponent);
		//.attack_piece(pieceToCapture);
		// 进行血量检测
		if (pieceToCaptureComponent.health == 0)
		{
            // 失败判定,如果Admiral被吃就结束了
            if (pieceToCapture.GetComponent<Piece>().type == PieceType.Admiral)
			{
				GoToEndScene();
			}
            // 如果不是Admiral，就摧毁对方的棋子，并更新在Board上
            currentPlayer.capturedPieces.Add(pieceToCapture);
			pieces[gridPoint.x, gridPoint.y] = null;
			Destroy(pieceToCapture);
			return 1;
		}

		return 0;

    }

	public void GoToEndScene()
	{
		Debug.Log(currentPlayer.name + " wins!");
		Destroy(board.GetComponent<TileSelector>());
		Destroy(board.GetComponent<MoveSelector>());

		SceneManager.LoadScene(end_scene_name, LoadSceneMode.Single);
	}

	public void SaveTwoPieces(GameObject py, GameObject pe)
	{
		p_y = py;
		p_e = pe;
	}

	public int ShowCombatPanel()
	{
		CombatPanel.SetActive(true);
        unit_title_UI.SetActive(false);
        unit_info_UI.SetActive(false);
        unit_image_UI.SetActive(false);
        whoTurnIndicator.SetActive(false);


        Piece pyc = p_y.GetComponent<Piece>();
		Piece pec = p_e.GetComponent<Piece>();

		string yourString = "Your unit is: " + pyc.type + "\n" + "Attack: " + pyc.attack + "\n" + "Armor: " + pyc.defense + "\n" + "HP: " + pyc.health;
		string enemyString = "Enemy unit is " + pec.type + "\n" + "Attack: " + pec.attack + "\n" + "Armor: " + pec.defense + "\n" + "HP: " + pec.health;

		Panel_Your.GetComponent<Text>().text = yourString;
		Panel_Enemy.GetComponent<Text>().text = enemyString;

		return 1;
	}

	public void CloseCombatPanel()
	{
		CombatPanel.SetActive(false);
	}

	public void ShowScoreBoard()
	{
		if (socreBoard.activeSelf == true)
		{
			socreBoard.SetActive(false);
		}
		else
		{
			socreBoard.SetActive(true);

			string your_p = "For Red:\n" + red.PieceToString();
			string enemy_p = "For Blue:\n" + blue.PieceToString();

			socreBoard_Your.GetComponent<Text>().text = your_p;
			socreBoard_Enemy.GetComponent<Text>().text = enemy_p;

			string turn_s = "Turn Number: " + turnCount.ToString();

			turnNum.GetComponent<Text>().text = turn_s;
		}
	}

	public void CloseScoreBoard()
	{
		socreBoard.SetActive(false);
	}

    public void SelectPiece(GameObject piece)
    {
        board.SelectPiece(piece);

		Piece pieceComponent = piece.GetComponent<Piece>();

		unit_title_UI.SetActive(true);
		unit_info_UI.SetActive(true);
		unit_image_UI.SetActive(true);

		string title = "Name:" + pieceComponent.name.ToString();
		string info = "Attack:" + pieceComponent.attack.ToString() + "\n";
		info += "Defense:" + pieceComponent.defense.ToString() + "\n";
		info += "Health:" + pieceComponent.health.ToString() + "\n";
		if (pieceComponent.is_range == 0)
		{
			info += "Melee Unit";
		}
		else
		{
			info += "Range Unit";
		}

		unit_title_UI.GetComponent<Text>().text = title;
		unit_info_UI.GetComponent<Text>().text = info;



    }

    public void DeselectPiece(GameObject piece)
    {
		// 10.27新增修改，为了让取消选择后的红蓝色依然正常
		int f = currentPlayer.forward;
        board.DeselectPiece(piece,f);

		unit_title_UI.SetActive(false);
		unit_info_UI.SetActive(false);
		unit_image_UI.SetActive(false);
    }

    public bool DoesPieceBelongToCurrentPlayer(GameObject piece)
    {
        return currentPlayer.pieces.Contains(piece);
    }

    public GameObject PieceAtGrid(Vector2Int gridPoint)
    {
		int tmp = board_num-1;
        if (gridPoint.x > tmp || gridPoint.y > tmp || gridPoint.x < 0 || gridPoint.y < 0)
        {
            return null;
        }
        return pieces[gridPoint.x, gridPoint.y];
    }

	// 返回
    public Vector2Int GridForPiece(GameObject piece)
    {
        for (int i = 0; i < board_num; i++) 
        {
            for (int j = 0; j < board_num; j++)
            {
                if (pieces[i, j] == piece)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

	// 检测目标位置的Piece，如果有且是友军Piece，返回True
    public bool FriendlyPieceAt(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null) {
            return false;
        }

        if (otherPlayer.pieces.Contains(piece))
        {
            return false;
        }

        return true;
    }

	// 切换Turn调换这个Player标识即可
	// AI可以在Tile里面Update或者Enter的时候做检测回合，然后再导向AI Script
    public void NextPlayer()
    {
        Player tempPlayer = currentPlayer;
        currentPlayer = otherPlayer;
        otherPlayer = tempPlayer;

        // generate a random number/event
        Random r = new Random();
        int randomInt = r.Next(0, 5);

        turnCount += 1;

		ShowWhoTurn();

        if (randomInt == 3)
        {
            TriggerEvent();
        }


        if (current_scene_name == "vsAIScene" && currentPlayer.name == "Blue")
        {
            AiTurn();
        }
    }

    public void AiTurn()
    {
        // 获取所有可以走的格子
        ai.input_board(pieces);
        ai.input_enemy(currentPlayer);
        // 获取整个盘面的情况
        GameObject move_piece = ai.get_move_piece(instance);
        Vector2Int move_place = ai.get_move_place(instance);
        // 按照某则规则行动（最开始随机移动）
        AiMove(move_piece, move_place);
        // 结束回合

    }

    public void AiMove(GameObject movingPiece, Vector2Int gridPoint)
    {
        if (PieceAtGrid(gridPoint) == null)
            {
                Move(movingPiece, gridPoint);
                NextPlayer();
            }
            else
            {
                CapturePieceAt(movingPiece,gridPoint);
                NextPlayer();
            }
    }


    // 随机触发一个random的event
    public void TriggerEvent()
    {
        if (popUpBox.activeSelf == true)
        {
            popUpBox.SetActive(false);
        }
        else
        {
            popUpBox.SetActive(true);
        }
    }

    public void EventContinue()
    {
        popUpBox.SetActive(false);
    }

    public void ShowWhoTurn()
	{
		string current_turn = currentPlayer.name;
		string newString = current_turn + " Turn";

		whoTurnIndicator.GetComponent<TextMeshProUGUI>().text = newString;
	}

	//原Geonmetry Functions

	// 编号转坐标
    public Vector3 PointFromGrid(Vector2Int gridPoint)
    {
        float x = 1.0f * gridPoint.x;
        float z = 1.0f * gridPoint.y;
        return new Vector3(x, 0, z);
    }

	// 返回标准编号
    public Vector2Int GridPoint(int col, int row)
    {
        return new Vector2Int(col, row);
    }

	// 坐标转编号，做求整
    public Vector2Int GridFromPoint(Vector3 point)
    {
		float tmp = board_num/2.0f;
        int col = Mathf.FloorToInt(0.5f + point.x);
        int row = Mathf.FloorToInt(0.5f + point.z);
        return new Vector2Int(col, row);
    }

    private IEnumerator<int> Wait()
    {
        yield return 0;
    }
}
