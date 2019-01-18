using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using SimpleJSON;
using ExitGames.Client.Photon;
//using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonConnectScript :   PunBehaviour,IPunTurnManagerCallbacks
{
	public static int PlayerCounter1=0;
	public int PlayerCounter = 0;
	JSONNode rootNode=new JSONClass();
	public Text TurnStatus;
	public List<Transform> wayPoints;
	private PunTurnManager turnManager;
	public bool isMaster=false , isMyTurn ,isMasterCompleteMoved,isRemoteCompletelyMoved,isMasterCompletelyMovedForSnake,isRemoteCompletelyMovedForSnake;
	public GameObject MasterPlayer, RemotePlayer,Dice,RoomCreation,InGame,Maze,WayPoints1;
	public static float mover=0;
	static int counter=1,ladderCounter=0,snakeCounter=0;
	public static int  MasterI = 1,RemoteI=1;
	public int Mnum = 1,Rnum=1;
	public bool isMasterMove,isRemoteMove;
	bool enablingFlag;
	public Transform CurrentPosition;
	public List<GameObject> LadderList;
	public List<GameObject> LadderDestination;
	public List<GameObject> SnakeMouthPosition;
	public List<GameObject> SnakeTalePosition;
	public bool isLadder=false,isSnake=false,isClimbedMaster,isClimbedRemote;
	public int[] MasterIRemoteIValue = new int[13];
	public int[] MasterIRemoteSnakeValue = new int[15];
	public SpriteRenderer sp;
	private Sprite[] sprites;
	bool masterCanMove,RemoteCanMove;
	bool ismoving1=false,ismoving2=false,ismoving3=false,ismoving4=false,ismoving5=false,ismoving6=false,ismoving7=false,ismoving8=false,ismoving9=false,ismoving10=false,ismoving11=false,ismoving12=false,ismoving13=false;
	// Use this for initialization

	public InputField PlayerNameField;
	public InputField RoomNameField;
	public string roomName="MyRoom";

	public void Awake()
	{
		PhotonNetwork.ConnectUsingSettings("0.9");
	}

	public void Create()
	{
		PhotonNetwork.CreateRoom(this.roomName, new RoomOptions() { MaxPlayers = 2 }, null);
	}


	public void Join()
	{
		PhotonNetwork.JoinRoom(this.roomName);
	}



	public void OnCreatedRoom()
	{
		Debug.Log("OnCreatedRoom");
		isMaster = true;
		print ("IsMaster" + isMaster);
	}


	public void OnJoinedRoom()
	{
		PlayerCounter++;
		print("PlayerCounter"+PlayerCounter);
		Debug.Log("OnJoinedRoom");
		StartCoroutine (SpawnPlayers ());
	}


	IEnumerator SpawnPlayers()
	{
		yield return new WaitForSeconds (.5f);

		print("OnJoinedRoom");

		print (PhotonNetwork.room.name);

		if (isMaster) 
		{
			this.GetComponent<Gamelogic1> ().Player1.SetActive (true);
			MasterPlayer = GameObject.Find ("Master");
			RoomCreation.SetActive (false);
			InGame.SetActive (true);
			Maze.SetActive (true);
			WayPoints1.SetActive (true);

		} 
		else
		{
			this.GetComponent<Gamelogic1> ().Player1.SetActive (true);
			MasterPlayer = GameObject.Find ("Master");
			RoomCreation.SetActive (false);
			InGame.SetActive (true);
			Maze.SetActive (true);
			WayPoints1.SetActive (true);
		}
	}



	public void EnterPName(string PName)
	{
		PhotonNetwork.playerName = PName;
	}

	public void EnterRName(string RName)
	{
		this.roomName = RName;
	}



	private void Start () 
	{
		sprites = Resources.LoadAll<Sprite> ("DiceSprites/");
		Dice.SetActive (false);
		CurrentPosition=wayPoints[0];
		this.turnManager = this.gameObject.AddComponent<PunTurnManager>();
		this.turnManager.TurnManagerListener = this;
		//Connecting to the photon Server
		PhotonNetwork.ConnectUsingSettings ("NSD");  
		MasterIRemoteIValue [0] = 19;
		MasterIRemoteIValue [1] = 13;
		MasterIRemoteIValue [2] = 27;
		MasterIRemoteIValue [3] = 33;
		MasterIRemoteIValue [4] = 73;
		MasterIRemoteIValue [5] = 36;
		MasterIRemoteIValue [6] = 58;
		MasterIRemoteIValue [7] = 66;
		MasterIRemoteIValue [8] = 75;
		MasterIRemoteIValue [9] = 77;
		MasterIRemoteIValue [10] = 85;
		MasterIRemoteIValue [11] = 97;
		MasterIRemoteIValue [12] = 90;

		MasterIRemoteSnakeValue [0] = 3;
		MasterIRemoteSnakeValue [1] = 1;
		MasterIRemoteSnakeValue [2] = 9;
		MasterIRemoteSnakeValue [3] = 4;
		MasterIRemoteSnakeValue [4] = 5;
		MasterIRemoteSnakeValue [5] = 35;
		MasterIRemoteSnakeValue [6] = 1;
		MasterIRemoteSnakeValue [7] = 22;
		MasterIRemoteSnakeValue [8] = 27;
		MasterIRemoteSnakeValue [9] = 44;
		MasterIRemoteSnakeValue [10] = 58;
		MasterIRemoteSnakeValue [11] = 47;
		MasterIRemoteSnakeValue [12] = 24;
		MasterIRemoteSnakeValue [13] = 86;
		MasterIRemoteSnakeValue [14] = 62;
	}
	// Update is called once per frame
	void Update () 
	{

		DisplayTurnStatus ();

		PlayerEnableOrDisable ();

//		GetInputs ();

		PlayerMovement ();

	}
	void DisplayTurnStatus ()
	{
		if (isMyTurn == true && PhotonNetwork.playerList.Length == 2) 
		{
			TurnStatus.text = "Your Turn";
			Dice.GetComponent<Button> ().interactable = true;
		}
		else 
		{
			TurnStatus.text = "Not Your Turn";
			Dice.GetComponent<Button> ().interactable = false;
		}
	}

	void GetInputs()
	{
		if( Input.GetMouseButtonDown(0) && isMyTurn )
		{
			rootNode.Add ("places",Random.Range(1,7 ).ToString());

			string  infoToSend = ""+rootNode.ToString() ;

			this.MakeTurn(infoToSend);

		}
	}



	public void DiceToss()
	{

		StartCoroutine (SpinDice ());

		print ("Hello");
	
	}



	IEnumerator SpinDice()
	{
		
		int RandomDice=0;

		int finalRoll=0;

		for (int i = 0; i <5; i++) 
		{
		
			RandomDice = Random.Range (0, 6);

			sp.sprite = sprites [RandomDice];

			yield return new WaitForSeconds (.1f);
		}

		finalRoll = RandomDice + 1;

		rootNode.Add ("places",finalRoll.ToString());

		string  infoToSend = ""+rootNode.ToString() ;

		this.MakeTurn(infoToSend);

		print (finalRoll);
	}


	void PlayerEnableOrDisable()
	{
		if (PhotonNetwork.playerList.Length == 2) {
			this.GetComponent<Gamelogic1> ().Player2.SetActive (true);

			this.GetComponent<Gamelogic1> ().Player2.SetActive (true);

			RemotePlayer = this.GetComponent<Gamelogic1> ().Player2;

			Dice.SetActive (true);

			RoomCreation.SetActive (false);
			InGame.SetActive (true);
			Maze.SetActive (true);
			WayPoints1.SetActive (true);

		}
		else if (PhotonNetwork.playerList.Length == 1) 
		{
			
		}
		else 
		{
			
			this.GetComponent<Gamelogic1> ().Player2.SetActive (false);

			TurnStatus.text = "Let Other Player to Connect";

			Dice.SetActive (false);

		}
	}

	void PlayerMovement ()
	{	

		//for Master movement

		if (MasterI == 99 && MasterPlayer.transform.position==wayPoints[99].transform.position) 
		{

			print ("Match won by MasterI");

			TurnStatus.text = "Won by "+PhotonNetwork.masterClient.NickName;

			Dice.SetActive (false);

		}


		else if (counter % 2 == 0 && counter > 1 && isMasterMove == true)
		{
			
			if (((MasterI + mover) < 99 || MasterI + mover == 99 ) && masterCanMove==false)
			{

				print ("Addition of MasterI+mover is" + (MasterI + mover) + " is Less than 99 it Can Move");

				masterCanMove = true;
			
				RemoteCanMove = false;

			}
			if (masterCanMove == true)
			{
				


				print ("MasrterMover" + mover);

				if (Mnum <= mover) {
						
					print ("MasterMoving");
					
					MasterPlayer.transform.localPosition = Vector3.MoveTowards (MasterPlayer.transform.localPosition, wayPoints [MasterI].transform.position, 5 * Time.deltaTime);

					if (MasterPlayer.transform.localPosition == wayPoints [MasterI].transform.localPosition) {

						MasterI++;

						Mnum++;
						
					}
				}
				if (Mnum == mover + 1) {		
						
					if (MasterPlayer.transform.localPosition == wayPoints [MasterI - 1].transform.localPosition) {

						print ("isMasterMove Became false");

						isMasterMove = false;
						Mnum = 0;

						MasterI--;

						print ("MasterI:" + MasterI);

						print ("Rnum:" + Mnum);

						isMasterCompleteMoved = true;

						isMasterCompletelyMovedForSnake = true;
						
					}
				}
			}
		}
		//for checking if there any ladder in the last position
		if (isMasterCompleteMoved == true)
		{
		
			CheckLadder (MasterPlayer);
		
		}

		if (isMasterCompletelyMovedForSnake == true) 
		{

			CheckSnakeAtThisPosition (MasterPlayer);

		}





		//for remote movement


		if (RemoteI == 99 && RemotePlayer.transform.position==wayPoints[99].transform.position) 
		{

			print ("Match won by Remote");

			TurnStatus.text = "Won by "+PhotonNetwork.playerList.Length;

			Dice.SetActive (false);

		}



		else if (counter % 2 == 1 && counter > 1 && isRemoteMove == true) 
		{

			if (((RemoteI + mover) < 99 || RemoteI + mover == 99) && RemoteCanMove == false) 
			{

				print ("Addition of RemoteI+mover is" + (RemoteI + mover) + " is Less than 99 it Can Move");

				RemoteCanMove = true;

				masterCanMove = false;
		
			}
			if (RemoteCanMove == true)
			{
				


				print ("RemoteMover" + mover);

				if (Rnum <= mover) {
					
					RemotePlayer.transform.localPosition = Vector3.MoveTowards (RemotePlayer.transform.position, wayPoints [RemoteI].transform.localPosition, 5 * Time.deltaTime);

					if (RemotePlayer.transform.localPosition == wayPoints [RemoteI].transform.localPosition) {

						RemoteI++;

						Rnum++;
						
					}
				}
				if (Rnum == mover + 1) {
					
					if (RemotePlayer.transform.localPosition == wayPoints [RemoteI - 1].transform.localPosition) {
							
						isRemoteMove = false;

						print ("isRemoteMove Became false");

						Rnum = 0;

						RemoteI--;

						print ("RemoteI:" + RemoteI);

						print ("Rnum" + Rnum);

						isRemoteCompletelyMoved = true;

						isRemoteCompletelyMovedForSnake = true;
						
					}
				}
			} 
		}
		//for checking if there any ladder in the last position
		if (isRemoteCompletelyMoved == true)
		{
			
			CheckLadder (RemotePlayer);
		
		}


		if (isRemoteCompletelyMovedForSnake == true) 
		{
			
			CheckSnakeAtThisPosition (RemotePlayer);
		
		}
	}




	void CheckLadder(GameObject Player)
	{
		for (int i = 0; i < LadderList.Count; i++) 
		{
			if (Player.transform.position == LadderList [i].transform.position) 
			{
				print ("Ladder is there");

				print ("I Value:" + i);

				isLadder = true;

				ladderCounter = i;

				print ("ladderCounter" + ladderCounter); 

				break;
			}
		}

		if (isLadder == true)
		{

			if (isMasterCompleteMoved == true)
			{
				MasterI = MasterIRemoteIValue [ladderCounter];

				print ("After climbing the MasterI:" + MasterI); 
			} 

			else if (isRemoteCompletelyMoved == true) 
			{
				RemoteI = MasterIRemoteIValue [ladderCounter];
			
				print ("After climbing the RemoteI:" + MasterI); 
			}
			if (Player.transform.position != LadderDestination [ladderCounter].transform.position) 
			{
				
				Player.transform.position = Vector3.MoveTowards (Player.transform.position, LadderDestination [ladderCounter].transform.position, 5 * Time.deltaTime);

				Player.transform.rotation = wayPoints [ladderCounter].transform.rotation;
			
			} 
			if (Player.transform.position == LadderDestination [ladderCounter].transform.position) 
			{
			
				Player.transform.rotation = Quaternion.Euler (new Vector3 (0, 180, 0));

				isLadder = false;

				isMasterCompleteMoved = false;

				isRemoteCompletelyMoved = false;

				ladderCounter = 0;

				print("isLadder:"+isLadder+" isMasterCompleteMoved:"+isMasterCompleteMoved+" isRemoteCompletelyMoved:"+isRemoteCompletelyMoved+" ladderCounter:"+ladderCounter);
			}
		}
		else 
		{

			isMasterCompleteMoved = false;

			isRemoteCompletelyMoved = false;

			print ("isMasterCompleteMoved:" + isMasterCompleteMoved + " isRemoteCompletelyMoved:" + isRemoteCompletelyMoved);
		}
	}




	//Method to check wheather a snake is there at current position or not
	void CheckSnakeAtThisPosition(GameObject Player)
	{

			for (int i = 0; i < SnakeMouthPosition.Count; i++) 
			{
			
				if (Player.transform.position == SnakeMouthPosition [i].transform.position)
				{

					print ("Snake is there");
	
					isSnake = true;

					snakeCounter = i;

					print ("snakeCounter:" + snakeCounter);

					break;

				}
			}

			if (isSnake == true) 
			{

			if (isMasterCompletelyMovedForSnake==true) {
					
					print ("Player.transform.name " + Player.transform.name);

					MasterI = MasterIRemoteSnakeValue [snakeCounter];

					print ("After Beaten by snake MasterI:" + MasterI);
			
			} else if (isRemoteCompletelyMovedForSnake==true) {

					print ("Player.transform.name " + Player.transform.name);

					RemoteI = MasterIRemoteSnakeValue [snakeCounter];

					print ("After Beaten by snake RemoteI:" + RemoteI);
			
				}

			if (Player.transform.position != SnakeTalePosition [snakeCounter].transform.position) 
			{

				print (Player.transform.name + "" + " is Moving");

				Player.transform.position = Vector3.MoveTowards (Player.transform.position, SnakeTalePosition [snakeCounter].transform.position, 5 * Time.deltaTime);

				Player.transform.rotation = SnakeTalePosition [snakeCounter].transform.rotation;

			}

			if (Player.transform.position == SnakeTalePosition [snakeCounter].transform.position) 
			{

				Player.transform.rotation = Quaternion.Euler (new Vector3 (0, 180, 0));

				print (Player.transform.name + "" + " is Complately moved");
						
				isSnake = false;

				snakeCounter = 0;

				isMasterCompletelyMovedForSnake = false;

				isRemoteCompletelyMovedForSnake = false;
	
				print ("isMasterCompletelyMovedForSnake" + isMasterCompletelyMovedForSnake+"isRemoteCompletelyMovedForSnake"+isRemoteCompletelyMovedForSnake	);	

			}
			/*if (MasterI == 5)
			{

				if (MasterPlayer.transform.position != LadderDestination [1].transform.position) 
				{

					MasterPlayer.transform.position = Vector3.MoveTowards (MasterPlayer.transform.position, LadderDestination [1].transform.position, 5 * Time.deltaTime);
				}
				if (MasterPlayer.transform.position == LadderDestination [1].transform.position) 
				{
					MasterI = 13;
				}
			}
			else if (RemoteI == 5) 
			{

				if (RemotePlayer.transform.position != LadderDestination [1].transform.position) 
				{

					RemotePlayer.transform.position = Vector3.MoveTowards (RemotePlayer.transform.position, LadderDestination [1].transform.position, 5 * Time.deltaTime);
				}
				if (RemotePlayer.transform.position == LadderDestination [1].transform.position) 
				{
					RemoteI = 13;
				}

			}*/
		}

		else 
		{

			isMasterCompletelyMovedForSnake = false;
		
			isRemoteCompletelyMovedForSnake = false;

			print ("isMasterCompletelyMovedForSnake" + isMasterCompletelyMovedForSnake+"isRemoteCompletelyMovedForSnake"+isRemoteCompletelyMovedForSnake	);	

		}
	}



	public string temp;

	public void MakeTurn( string data )
	{
		temp = data;

		this.turnManager.SendMove(data as object , true);

		JSONNode jn = SimpleJSON.JSONData.Parse (temp);
	}

	#region TurnManager Callbacks

	/// <summary>Called when a turn begins (Master Client set a new Turn number).</summary>
	public void OnTurnBegins(int turn)
	{
//		Debug.Log("OnTurnBegins() turn: "+ turn);
	}
	public void OnTurnCompleted(int obj)
	{
//		Debug.Log("OnTurnCompleted: " + obj);
		isMyTurn = false;
	}
	// when a player moved (but did not finish the turn)
	public void OnPlayerMove(PhotonPlayer photonPlayer, int turn, object move)
	{
//		Debug.Log("OnPlayerMove: " + photonPlayer + " turn: " + turn + " action: " + move);
	}
	public void OnPlayerFinished(PhotonPlayer photonPlayer, int turn, object move)
	{
//		Debug.Log("OnTurnFinished: " + photonPlayer + " turn: " + turn + " action: " + move + "photonPlayer.IsLocal = "+photonPlayer.IsLocal);
		counter++;
//		print ("counter:" + counter);
		if (counter % 2 == 0) 
		{
			
			JSONNode jn = SimpleJSON.JSONData.Parse (move as string);

			mover = float.Parse(jn ["places"].Value);

			isMasterMove = true;
//			StartCoroutine (MasterMove (mover));
		}
		else if(counter%2==1 && counter>1)
		{
			JSONNode jn = SimpleJSON.JSONData.Parse (move as string);

			mover = float.Parse(jn ["places"].Value);

			isRemoteMove = true;
		}
		if (photonPlayer.IsLocal)
		{

			isMyTurn = false;

//			TurnStatus.enabled = false;
		}
		else
		{
			StartCoroutine (WaitS ());
		}
	}
	IEnumerator WaitS()
	{
		

		yield return new WaitForSeconds (mover+2);

//		TurnStatus.enabled = true;

		isMyTurn = true;

		if (PhotonNetwork.isMasterClient) 
		{
			StartTurn ();
		}
	}

	public void OnTurnTimeEnds(int obj)
	{
//		if (!IsShowingResults)
//		{
//			Debug.Log("OnTurnTimeEnds: Calling OnTurnCompleted");
			OnTurnCompleted(-1);
//		}
	}

	private void UpdateScores()
	{
//		if (this.result == ResultType.LocalWin)
//		{
			PhotonNetwork.player.AddScore(1);   // this is an extension method for PhotonPlayer. you can see it's implementation
//		}
	}


	#endregion

	public void StartTurn()
	{
		print ("StartTurn000");

		isMyTurn = true;

		if (PhotonNetwork.isMasterClient)
		{
			print ("StartTurn1111");

			this.turnManager.BeginTurn();
		}
	}

	public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		Debug.Log("Other player arrived turn = "+ this.turnManager.Turn );
		PlayerCounter++;
		print ("PlayerCounter:" + PlayerCounter);

//		if (isMaster) 
//		{
//			this.GetComponent<Gamelogic1> ().PLayer2.SetActive (true);
////			this.turnManager.SendMove ( (byte)1 , true );
//		}
		if (PhotonNetwork.room.PlayerCount == 2)
		{
			if (this.turnManager.Turn == 0)
			{
				// when the room has two players, start the first turn (later on, joining players won't trigger a turn)
//				this.StartTurn();
				isMyTurn = true;
			}
		}
	}
}

