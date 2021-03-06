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

	// Army Unit Model
    public GameObject redAdmiral;
    public GameObject redLieutenant;
    public GameObject redPikeman;
    public GameObject redCavalry;
    public GameObject redCannon;
    public GameObject redRifleman;

    public GameObject blueAdmiral;
    public GameObject blueLieutenant;
    public GameObject bluePikeman;
    public GameObject blueCavalry;
    public GameObject blueCannon;
    public GameObject blueRifleman;

    // Non Unit Model
    public GameObject ObstacleBlock;
    public GameObject TownBlock;
    public GameObject FortressBlock;
    public GameObject ForestBlock;
    public GameObject MoutainBlock;

    public GameObject[] BlueUnitsList;
    public GameObject[] RedUnitsList;



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
    public string start_scene_name;

	public GameObject unit_title_UI;
	public GameObject unit_info_UI;
	public GameObject unit_image_UI;

	public GameObject whoTurnIndicator;
    public GameObject EventText;

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

    string sceneName;

    public bool turn_ai;


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

        BlueUnitsList = GameObject.FindGameObjectsWithTag("Blue");
        RedUnitsList = GameObject.FindGameObjectsWithTag("Red");

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

        Scene m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;

        if (sceneName == "vsAIScene1")
        {
            turn_ai = true;
        }
        else
        {
            turn_ai = false;
        }
    }

	//创建所有棋子，绑定玩家，设置初始位置，调用模型
    private void InitialSetup()
    {
        // Team Red's inital locations
        AddPiece(redPikeman, red, 4, 7);
        AddPiece(redPikeman, red, 5, 7);
        AddPiece(redPikeman, red, 6, 7);
        AddPiece(redPikeman, red, 6, 6);
        AddPiece(redPikeman, red, 6, 5);


        AddPiece(redLieutenant, red, 8,12 );
        AddPiece(redAdmiral, red, 5, 5);

        AddPiece(redCannon, red, 6, 11);
        AddPiece(redCannon, red, 9, 11);

        for (int i = 7; i < 11; i++)
        {
            AddPiece(redRifleman, red, i, 14);
        }

        for (int i = 3; i < 6; i++)
        {
            AddPiece(redRifleman, red, 10, i);
        }
        AddPiece(redCavalry, red, 9, 3);
        AddPiece(redCavalry, red, 9, 4);
        AddPiece(redCavalry, red, 9, 5);




        // Team Blue's inital locations
        AddPiece(blueAdmiral, blue, 18, 18);
        AddPiece(bluePikeman, blue, 16, 18);
        AddPiece(bluePikeman, blue, 16, 17);

        AddPiece(blueLieutenant, blue, 17, 4);
        AddPiece(blueRifleman, blue, 15, 2);
        AddPiece(blueRifleman, blue, 15, 3);
        AddPiece(blueRifleman, blue, 15, 4);

        AddPiece(blueLieutenant, blue, 8, 18);
        AddPiece(bluePikeman, blue, 6, 17);
        AddPiece(bluePikeman, blue, 10, 17);
        AddPiece(blueRifleman, blue, 7, 16);
        AddPiece(blueRifleman, blue, 8, 16);
        AddPiece(blueRifleman, blue, 9, 16);

        AddPiece(blueCannon, blue, 7, 18);
        AddPiece(blueCannon, blue, 9, 18);

        AddPiece(blueCavalry, blue, 16, 1);
        AddPiece(blueCavalry, blue, 16, 2);
        AddPiece(blueCavalry, blue, 16, 3);

        // Place invisble object on water tiles,
        // so pieces may not move onto them.
        AddObstacle(ObstacleBlock, 15, 19);
        AddObstacle(ObstacleBlock, 14, 19);
        AddObstacle(ObstacleBlock, 15, 18);
        AddObstacle(ObstacleBlock, 14, 18);
        AddObstacle(ObstacleBlock, 15, 17);
        AddObstacle(ObstacleBlock, 14, 17);
        AddObstacle(ObstacleBlock, 15, 16);
        AddObstacle(ObstacleBlock, 15, 15);
        AddObstacle(ObstacleBlock, 16, 15);
        AddObstacle(ObstacleBlock, 16, 14);
        AddObstacle(ObstacleBlock, 17, 14);
        AddObstacle(ObstacleBlock, 16, 13);
        AddObstacle(ObstacleBlock, 17, 13);
        AddObstacle(ObstacleBlock, 16, 12);
        AddObstacle(ObstacleBlock, 17, 12);
        AddObstacle(ObstacleBlock, 16, 11);
        AddObstacle(ObstacleBlock, 17, 11);
        AddObstacle(ObstacleBlock,  16, 10);
        AddObstacle(ObstacleBlock, 16, 9);
        AddObstacle(ObstacleBlock, 17, 9);
        AddObstacle(ObstacleBlock, 15, 19);
        AddObstacle(ObstacleBlock, 17, 8);
        AddObstacle(ObstacleBlock, 15, 7);
        AddObstacle(ObstacleBlock, 16, 7);
        AddObstacle(ObstacleBlock,  14, 6);
        AddObstacle(ObstacleBlock,  15, 6);
        AddObstacle(ObstacleBlock,  16, 6);
        AddObstacle(ObstacleBlock, 13, 5);
        AddObstacle(ObstacleBlock, 14, 5);
        AddObstacle(ObstacleBlock, 12, 3);
        AddObstacle(ObstacleBlock, 12, 2);
        AddObstacle(ObstacleBlock, 11, 2);
        AddObstacle(ObstacleBlock, 11, 1);
        AddObstacle(ObstacleBlock,  10, 1);
        AddObstacle(ObstacleBlock,  9, 1);
        AddObstacle(ObstacleBlock,  4, 0);
        AddObstacle(ObstacleBlock,  5, 0);
        AddObstacle(ObstacleBlock, 6, 0);
        AddObstacle(ObstacleBlock,  7, 0);
        AddObstacle(ObstacleBlock, 8, 0);
        AddObstacle(ObstacleBlock,  9, 0);

        // Add invisible objects on Mountain tiles
        AddObstacle(ObstacleBlock, 2, 17);
        AddObstacle(ObstacleBlock, 3, 17);
        AddObstacle(ObstacleBlock, 4, 17);
        AddObstacle(ObstacleBlock, 2, 18);
        AddObstacle(ObstacleBlock, 3, 18);
        AddObstacle(ObstacleBlock, 4, 18);
        AddObstacle(ObstacleBlock, 2, 9);
        AddObstacle(ObstacleBlock, 3, 9);
        AddObstacle(ObstacleBlock, 4, 9);
        AddObstacle(ObstacleBlock, 2, 10);
        AddObstacle(ObstacleBlock, 3, 10);
        AddObstacle(ObstacleBlock, 4, 10);
        AddObstacle(ObstacleBlock, 7, 17);
        AddObstacle(ObstacleBlock, 8, 17);
        AddObstacle(ObstacleBlock, 9, 17);



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


    // 加入棋子时，会需要initial 位置、模型、和所属玩家
    // 所属玩家未来会用来判定Turn Base
    public void AddObstacle(GameObject prefab, int col, int row)
    {
        GameObject pieceObject = board.AddPiece(prefab, col, row);
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

        //Debug.Log("Going to remove water");
        // filter out locations with water
        locations.RemoveAll(gp => ObstacleCheck(gp));

        // filter out locations with friendly piece
        locations.RemoveAll(gp => FriendlyPieceAt(gp));

        return locations;
    }

	// 执行实际Move的操作，这里不会检测Valid Move，要在调用前自己检测
    public void Move(GameObject piece, Vector2Int gridPoint)
    {
        Piece pieceComponent = piece.GetComponent<Piece>();
        // Infantry只有第一次可以走两格
        if (pieceComponent.type == PieceType.Rifleman && !HasInfantryMoved(piece))
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

    public void GoToStartScene()
    {
        SceneManager.LoadScene(start_scene_name, LoadSceneMode.Single);
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

		string title = "Name: " + pieceComponent.name.ToString();
		string info = "Attack: " + pieceComponent.attack.ToString() + "\n";
		info += "Defense: " + pieceComponent.defense.ToString() + "\n";
		info += "Health: " + pieceComponent.health.ToString() + "\n";
        info += "\n";
        info += "Description: " + "\n" + pieceComponent.description.ToString() + "\n";
        
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

    // Check if the location tile is friendly unit,
    // If it is friendly, return True
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

    public void LocationCheck(GameObject unit)
    {
        Piece newPiece = unit.GetComponent<Piece>();
        Vector2Int unitGridPoint = GridForPiece(unit);
        Vector2Int[] TownList = { new Vector2Int(3, 6), new Vector2Int(4,6), 
            new Vector2Int(5, 6), new Vector2Int(3, 5), new Vector2Int(4, 5), new Vector2Int(5, 5), new Vector2Int(3, 4), new Vector2Int(4, 4), new Vector2Int(5, 4) };
        
        Vector2Int[] ForestList = { new Vector2Int(18, 10), new Vector2Int(18,11), 
            new Vector2Int(18,12), new Vector2Int(11, 10), new Vector2Int(12, 10), new Vector2Int(11, 11), new Vector2Int(12, 11), new Vector2Int(11, 12),
            new Vector2Int(12, 12),new Vector2Int(11, 13),new Vector2Int(12, 13),new Vector2Int(11, 14),new Vector2Int(12, 14),new Vector2Int(11, 15),
            new Vector2Int(12, 15),new Vector2Int(11, 16),new Vector2Int(12, 16), new Vector2Int(4, 14),new Vector2Int(4, 15),new Vector2Int(5, 14),new Vector2Int(5, 15),
            new Vector2Int(3, 11),new Vector2Int(3, 12),new Vector2Int(3, 13),new Vector2Int(4, 11),new Vector2Int(4, 12),new Vector2Int(4, 13),new Vector2Int(18, 4),new Vector2Int(18, 5), 
            new Vector2Int(9, 6),new Vector2Int(10, 6),new Vector2Int(11, 6),new Vector2Int(9, 7),new Vector2Int(10, 7),new Vector2Int(11, 7)};
        
        Vector2Int[] FortList = { new Vector2Int(8, 18), new Vector2Int(17, 4), new Vector2Int(17, 4) , new Vector2Int(7, 12), new Vector2Int(8, 12), new Vector2Int(7, 13) };



        for (int i = 0; i < TownList.Length; i++)
        {
            if (unitGridPoint == TownList[i])
            {
                if (newPiece.health != newPiece.maxHealth)
                {
                    Debug.Log("Debug 1");
                    newPiece.add_health(2);
                    TriggerUnitSpecialEvent("Unit stationed at the Town and 2 HP is recovered.");
                }
                else
                {
                    Debug.Log("Debug 2");
                    newPiece.recover_full_health();
                    TriggerUnitSpecialEvent("Unit stationed at the Town to heal and recover, HP +2.");
                    Debug.Log("The health is already full");
                }
            }
        }

        for (int i = 0; i < ForestList.Length; i++)
        {
            if (unitGridPoint == ForestList[i])
            {
                //Debug.Log("Reduce the range of unit");
                newPiece.set_range(newPiece.rangeNum-1);
                TriggerUnitSpecialEvent(newPiece.showMessage());
                break;
            }
            else
            {
                //Debug.Log("Go back to default range");
                newPiece.set_range(newPiece.defaultRange);
            }
        }

        for (int i = 0; i < FortList.Length; i++)
        {
            if (unitGridPoint == TownList[i])
            {
                newPiece.add_defense(1);
                TriggerUnitSpecialEvent("Unit stationed at the Fortress and picked up some new armor, Defense +1.");
            }
        }




    }


    // Check if the location tile is Water,
    // If it is water, return True
    public bool ObstacleCheck(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null)
        {
            return false;
        }

        if (piece.tag == "Water")
        {
            //Debug.Log("A Water Block found!");
            return true;
        }

        return false;
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
        int randomInt = r.Next(0, 20);
        turnCount += 1;

		ShowWhoTurn();


        if (turn_ai is true && currentPlayer.name == "Blue")
        {
            AiTurn();
        }
        else
        {
            if (randomInt <= 8)
            {
                TriggerRandomEvent(randomInt);
            }
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
                Debug.Log("capture 1");
                CapturePieceAt(movingPiece,gridPoint);
                NextPlayer();
            }
    }


    public void TriggerUnitSpecialEvent(string message)
    {
        EventText.GetComponent<TextMeshProUGUI>().text = message;
        if (popUpBox.activeSelf == true)
        {
            popUpBox.SetActive(false);
        }
        else
        {
            popUpBox.SetActive(true);
        }
    }

    // 随机触发一个random的event
    public void TriggerRandomEvent(int eventNum)
    {
        if (eventNum == 1)
        {
            EventText.GetComponent<TextMeshProUGUI>().text = "Union army is suffering from the cold weather. All union units -1 HP.";
            for (int i = 0; i < BlueUnitsList.Length; i++)
            {
                BlueUnitsList[i].GetComponent<Piece>().health -= 1;
                BlueUnitsList[i].GetComponent<Piece>().healthBar.SetHealth(BlueUnitsList[i].GetComponent<Piece>().health);
            }
        }

        if (eventNum == 2)
        {
            EventText.GetComponent<TextMeshProUGUI>().text = "Several Union carriages of supplies arrived! All units +1 Defense.";
            for (int i = 0; i < BlueUnitsList.Length; i++)
            {
                BlueUnitsList[i].GetComponent<Piece>().defense += 1;
            }
        }

        if (eventNum == 3)
        {
            EventText.GetComponent<TextMeshProUGUI>().text = "More weapons arrived from armory! All Unions units +1 Attack.";
            for (int i = 0; i < BlueUnitsList.Length; i++)
            {
                BlueUnitsList[i].GetComponent<Piece>().attack += 1;
            }
        }

        if (eventNum == 4)
        {
            EventText.GetComponent<TextMeshProUGUI>().text = "A small team of Union reinforcements arrived from the top right of the battle field!";
        }

        if (eventNum == 5)
        {
            EventText.GetComponent<TextMeshProUGUI>().text = "A small team of Confederate reinforcements arrived from the lower left right of the battle field!";
        }

        if (eventNum == 6)
        {
            EventText.GetComponent<TextMeshProUGUI>().text = "More weapons arrived from armory! All Confederate units +1 Attack.";
            for (int i = 0; i < RedUnitsList.Length; i++)
            {
                RedUnitsList[i].GetComponent<Piece>().attack += 1;
            }
        }

        if (eventNum == 7)
        {
            EventText.GetComponent<TextMeshProUGUI>().text = "Several Confederate carriages of supplies arrived! All units +1 Defense.";
            for (int i = 0; i < RedUnitsList.Length; i++)
            {
                RedUnitsList[i].GetComponent<Piece>().defense += 1;
            }

        }

        if (eventNum == 8)
        {
            EventText.GetComponent<TextMeshProUGUI>().text = "Confederate army is suffering from the cold weather. All Confederate units -1 HP.";
            for (int i = 0; i < RedUnitsList.Length; i++)
            {
                RedUnitsList[i].GetComponent<Piece>().health -= 1;
                RedUnitsList[i].GetComponent<Piece>().healthBar.SetHealth(RedUnitsList[i].GetComponent<Piece>().health);
            }


        }

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
