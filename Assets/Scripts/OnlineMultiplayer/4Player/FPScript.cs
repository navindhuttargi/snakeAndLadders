using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using SimpleJSON;
using ExitGames.Client.Photon;

public class FPScript : PunBehaviour,IPunTurnManagerCallbacks 
{
	JSONNode rootNode=new JSONClass();
	int masterCounter=0,remoteCounter=0,remote2Counter=0;
	public Text TurnStatus,MasterText,RemoteText,Remote2Text;
	public List<Transform> wayPoints;
	int id1=0,id2=0,id3=0,id11,id22,id33;
	private PunTurnManager turnManager;
	public bool isMaster=false , isMyTurn ,isMyTurn2,isMyTurn3,isMasterCompleteMoved,isRemoteCompletelyMoved,isMasterCompletelyMovedForSnake,isRemoteCompletelyMovedForSnake,isRemote2CompletelyMoved,isRemote2CompletelyMovedForSnake,isRemote3CompletelyMoved,isRemote3CompletelyMovedForSnake;
	public GameObject MasterPlayer, RemotePlayer,RemotePlayer2,RemotePlayer3,Dice,RoomCreation,InGame,Maze,WayPoints1;
	public static float mover=0;
	static int counter=0,ladderCounter=0,snakeCounter=0;
	public static int  MasterI = 1,RemoteI=1,RemoteI2=1,RemoteI3=1;
	public int Mnum = 1,Rnum=1,Rnum2=1,Rnum3=1;
	public bool isMasterMove,isRemoteMove,isRemote2Move,isRemote3Move,isSecondPlayer,isThirdPlayer,isFourthPlayer,MasterWon,RemoteWon,Remote2Won,Remote3Won;
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
	bool masterCanMove,RemoteCanMove,Remote2CanMove,Remote3CanMove;
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
		PhotonNetwork.CreateRoom(this.roomName, new RoomOptions() { MaxPlayers = 4 }, null);
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
		//		ThreePlayerGameLogic.num++;
		//		print ("In OnCreatedRoom num:" + ThreePlayerGameLogic.num);
	}


	public void OnJoinedRoom()
	{


		Debug.Log("OnJoinedRoom");
		StartCoroutine (SpawnPlayers ());
		//		print ("In OnCreatedRoom num:" + ThreePlayerGameLogic.num);
	}


	IEnumerator SpawnPlayers()
	{
		yield return new WaitForSeconds (.5f);

		print("OnJoinedRoom");

		print (PhotonNetwork.room.name);

		if (PhotonNetwork.playerList.Length == 2) 
		{
			isSecondPlayer = true;
		}

		if (PhotonNetwork.playerList.Length == 3) 
		{
			isThirdPlayer = true;
		}

		if (PhotonNetwork.playerList.Length == 4) 
		{
			isFourthPlayer = true;
		}
		if (isMaster) 
		{
			this.GetComponent<FPGameLogic> ().Player1.SetActive (true);
			MasterPlayer = GameObject.Find ("Master");
			RoomCreation.SetActive (false);
			InGame.SetActive (true);
			Maze.SetActive (true);
			WayPoints1.SetActive (true);

		} 
		else
		{
			this.GetComponent<FPGameLogic> ().Player1.SetActive (true);
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

		PlayerNaming ();

		DisplayTurnStatus ();

		PlayerEnableOrDisable ();

		PlayerMovement ();

		MasterWin ();

		RemoteWin ();

		Remote2Win ();

		Remote3Win ();

		//		print ("playerCount:" + PhotonNetwork.playerList.Length);

	}
	void MasterWin ()
	{
		if (MasterWon == true && counter == 0 && isMaster == true && isMyTurn==true) 
		{
			int finalRoll = 0;

			rootNode.Add ("places",finalRoll.ToString());

			string  infoToSend = ""+rootNode.ToString() ;

			this.MakeTurn(infoToSend);

			print (finalRoll);
		}
	}
	void RemoteWin ()
	{
		if (RemoteWon == true && counter == 1 && isSecondPlayer == true && isMyTurn == true) 
		{
			int finalRoll = 0;

			rootNode.Add ("places",finalRoll.ToString());

			string  infoToSend = ""+rootNode.ToString() ;

			this.MakeTurn(infoToSend);

			print (finalRoll);
		}
	}
	void Remote2Win ()
	{
		if (Remote2Won == true && counter == 2 && isThirdPlayer == true && isMyTurn == true) 
		{
			int finalRoll = 0;

			rootNode.Add ("places",finalRoll.ToString());

			string  infoToSend = ""+rootNode.ToString() ;

			this.MakeTurn(infoToSend);

			print (finalRoll);
		}
	}
	void Remote3Win ()
	{
		if (Remote3Won == true && counter == 3 && isFourthPlayer == true && isMyTurn == true) 
		{
			int finalRoll = 0;

			rootNode.Add ("places",finalRoll.ToString());

			string  infoToSend = ""+rootNode.ToString() ;

			this.MakeTurn(infoToSend);

			print (finalRoll);
		}
	}
	void PlayerNaming()
	{
		if (PhotonNetwork.isMasterClient) 
		{

		}
	}
	void DisplayTurnStatus ()
	{
		if (isMyTurn == true && PhotonNetwork.playerList.Length == 4) 
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
		/*
		 * when counter is 0 then ismyturn must be true 
		 * when counter is 1 then ismyturn1 must be true
		 * when coubter is 2 then ismyturn2 must be true
		*/
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
		if (PhotonNetwork.playerList.Length == 4) 
		{

			this.GetComponent<FPGameLogic> ().Player2.SetActive (true);

			this.GetComponent<FPGameLogic> ().Player2.SetActive (true);

			this.GetComponent<FPGameLogic> ().Player3.SetActive (true);

			this.GetComponent<FPGameLogic> ().Player4.SetActive (true);

			RemotePlayer = this.GetComponent<FPGameLogic> ().Player2;

			RemotePlayer2 = this.GetComponent<FPGameLogic> ().Player3;

			RemotePlayer3 = this.GetComponent<FPGameLogic> ().Player4;

			Dice.SetActive (true);

			RoomCreation.SetActive (false);

			InGame.SetActive (true);

			Maze.SetActive (true);

			WayPoints1.SetActive (true);

		}
		else 
		{

			this.GetComponent<FPGameLogic> ().Player2.SetActive (false);

			TurnStatus.text = "Let Other Player to Connect";

			//			print (TurnStatus.text);

			Dice.SetActive (false);

		}
		if (PhotonNetwork.playerList.Length == 2) 
		{

			this.GetComponent<FPGameLogic> ().Player2.SetActive (true);

			RemotePlayer = this.GetComponent<FPGameLogic> ().Player2;

			RoomCreation.SetActive (false);

			InGame.SetActive (true);

			Maze.SetActive (true);

			WayPoints1.SetActive (true);
		}
		if (PhotonNetwork.playerList.Length == 3) 
		{

			this.GetComponent<FPGameLogic> ().Player2.SetActive (true);

			RemotePlayer = this.GetComponent<FPGameLogic> ().Player2;

			this.GetComponent<FPGameLogic> ().Player3.SetActive (true);

			RemotePlayer2 = this.GetComponent<FPGameLogic> ().Player3;

			RoomCreation.SetActive (false);

			InGame.SetActive (true);

			Maze.SetActive (true);

			WayPoints1.SetActive (true);
		}
	}

	void PlayerMovement ()
	{	

		MasterPlayerMovement ();

		RemotePlayerMovement ();

		Remote2PlayerMovenent ();

		Remote3playerMovenent ();

	}

	void MasterPlayerMovement()
	{
		//for Master movement

		if (MasterI == 99 && MasterPlayer.transform.position==wayPoints[99].transform.position) 
		{

			print ("Match won by MasterI");

			MasterWon = true;

			isMasterMove = false;

		}


		else if (counter==1  && isMasterMove == true )
		{

			if (((MasterI + mover) <= 99 ) && masterCanMove==false)
			{

				print ("Addition of MasterI+mover is" + (MasterI + mover) + " is Less than 99 it Can Move");

				masterCanMove = true;

				RemoteCanMove = false;

				Remote2CanMove = false;

				Remote3CanMove = false;

			}
			else if (((MasterI + mover) > 99) && masterCanMove == false) 
			{

				print ("Addition is large Master Can't Move");

				isMasterMove = false;

				masterCanMove = false;

				mover = 1;

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
	}
	void RemotePlayerMovement()
	{
		//for remote movement


		if (RemoteI == 99 && RemotePlayer.transform.position==wayPoints[99].transform.position) 
		{

			print ("Match won by Remote");

			RemoteWon = true;

			isRemoteMove = false;

//			TurnStatus.text = "Won by Remote";

//			Dice.SetActive (false);

		}



		else if (counter==2 && isRemoteMove == true) 
		{

			if (((RemoteI + mover) <= 99) && RemoteCanMove == false) 
			{

				print ("Addition of RemoteI+mover is" + (RemoteI + mover) + " is Less than 99 it Can Move");

				RemoteCanMove = true;

				masterCanMove = false;

				Remote2CanMove = false;

				Remote3CanMove = false;

			}
			else if (((RemoteI + mover) > 99) && RemoteCanMove == false)
			{

				print ("Addition is large Remote Can't Move");

				isRemoteMove = false;

				RemoteCanMove = false;

				mover = 1;

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
	void Remote2PlayerMovenent()
	{
		//for remote2 movement
		if (RemoteI2 == 99 && RemotePlayer2.transform.position==wayPoints[99].transform.position) 
		{

			print ("Match won by Remote2");

			Remote2Won = true;

			isRemote2Move = false;

//			TurnStatus.text = "Won by Remote";

//			Dice.SetActive (false);

		}



		else if (counter==3  && isRemote2Move== true) 
		{

			if (((RemoteI2 + mover) <= 99) && Remote2CanMove == false) 
			{

				print ("Addition of RemoteI+mover is" + (RemoteI + mover) + " is Less than 99 it Can Move");

				Remote2CanMove = true;

				RemoteCanMove = false;

				masterCanMove = false;

				Remote3CanMove = false;

			}
			else if((((RemoteI2 + mover) > 99) && Remote2CanMove == false))
			{

				print ("Addition is large Remote2 Can't Move");

				isRemote2Move = false;

				Remote2CanMove = false;

				mover = 1;

			}
			if (Remote2CanMove == true)
			{



				print ("Remote2Mover" + mover);

				if (Rnum2 <= mover) {

					RemotePlayer2.transform.localPosition = Vector3.MoveTowards (RemotePlayer2.transform.position, wayPoints [RemoteI2].transform.localPosition, 5 * Time.deltaTime);

					if (RemotePlayer2.transform.localPosition == wayPoints [RemoteI2].transform.localPosition) {

						RemoteI2++;

						Rnum2++;

					}
				}
				if (Rnum2 == mover + 1) {

					if (RemotePlayer2.transform.localPosition == wayPoints [RemoteI2 - 1].transform.localPosition) {

						isRemote2Move = false;

						print ("isRemoteMove Became false");

						Rnum2 = 0;

						RemoteI2--;

						print ("RemoteI:" + RemoteI2);

						print ("Rnum" + Rnum2);

						isRemote2CompletelyMoved = true;

						isRemote2CompletelyMovedForSnake = true;

					}
				}
			} 
		}
		//for checking if there any ladder in the last position
		if (isRemote2CompletelyMoved == true)
		{

			print ("At Last Counter:" + counter);

			CheckLadder (RemotePlayer2);

		}


		if (isRemote2CompletelyMovedForSnake == true) 
		{

			CheckSnakeAtThisPosition (RemotePlayer2);

		}

	}
	void Remote3playerMovenent()
	{
		//for Remote3 Movement
		if (RemoteI3 == 99 && RemotePlayer3.transform.position==wayPoints[99].transform.position) 
		{

			print ("Match won by Remote");

			Remote3Won = true;

			if (counter == 4 && isRemote3Move == true) 
			{
				counter = 0;
			}

			isRemote3Move = false;

//			TurnStatus.text = "Won by Remote";

//			Dice.SetActive (false);

		}



		else if (counter==4  && isRemote3Move== true) 
		{

			if (((RemoteI3 + mover) <= 99) && Remote3CanMove == false) {

				print ("Addition of RemoteI+mover is" + (RemoteI + mover) + " is Less than 99 it Can Move");

				Remote3CanMove = true;

				Remote2CanMove = false;

				RemoteCanMove = false;

				masterCanMove = false;



			}
			else if (((RemoteI3 + mover) > 99) && Remote3CanMove == false) 
			{

				isRemote3Move = false;

				Remote3CanMove = false;

				mover = 1;

				counter = 0;
			
			}
			if (Remote3CanMove == true)
			{



				print ("Remote2Mover" + mover);

				if (Rnum3 <= mover) {

					RemotePlayer3.transform.localPosition = Vector3.MoveTowards (RemotePlayer3.transform.position, wayPoints [RemoteI3].transform.localPosition, 5 * Time.deltaTime);

					if (RemotePlayer3.transform.localPosition == wayPoints [RemoteI3].transform.localPosition) {

						RemoteI3++;

						Rnum3++;

					}
				}
				if (Rnum3 == mover + 1) {

					if (RemotePlayer3.transform.localPosition == wayPoints [RemoteI3 - 1].transform.localPosition) {

						isRemote3Move = false;

						print ("isRemoteMove Became false");

						Rnum3 = 0;

						RemoteI3--;

						print ("RemoteI:" + RemoteI3);

						print ("Rnum" + Rnum3);

						isRemote3CompletelyMoved = true;

						isRemote3CompletelyMovedForSnake = true;

					}
				}
			} 
		}
		//for checking if there any ladder in the last position
		if (isRemote3CompletelyMoved == true)
		{

			counter = 0;

			print ("At Last Counter:" + counter);

			CheckLadder (RemotePlayer3);

		}


		if (isRemote3CompletelyMovedForSnake == true) 
		{

			CheckSnakeAtThisPosition (RemotePlayer3);

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

				print ("After climbing the RemoteI:" + RemoteI); 

			}
			else if(isRemote2CompletelyMoved == true) 
			{

				RemoteI2 = MasterIRemoteIValue [ladderCounter];

				print ("After climbing the RemoteI:" + RemoteI2); 

			}
			else if(isRemote3CompletelyMoved == true) 
			{

				RemoteI3 = MasterIRemoteIValue [ladderCounter];

				print ("After climbing the RemoteI:" + RemoteI3); 

			}

			if (Player.transform.position != LadderDestination [ladderCounter].transform.position) 
			{

				Player.transform.position = Vector3.MoveTowards (Player.transform.position, LadderDestination [ladderCounter].transform.position, 5 * Time.deltaTime);

			} 
			if (Player.transform.position == LadderDestination [ladderCounter].transform.position) 
			{

				isLadder = false;

				isMasterCompleteMoved = false;

				isRemoteCompletelyMoved = false;

				isRemote2CompletelyMoved = false;

				isRemote3CompletelyMoved = false;

				ladderCounter = 0;

				print("isLadder:"+isLadder+" ladderCounter:"+ladderCounter);

				print (" isMasterCompleteMoved:"+isMasterCompleteMoved+" isRemoteCompletelyMoved:"+isRemoteCompletelyMoved+" isRemote2CompletelyMoved "+isRemote2CompletelyMoved+" isRemote3CompletelyMoved:"+isRemote3CompletelyMoved);
			
			}
		}
		else 
		{

			isMasterCompleteMoved = false;

			isRemoteCompletelyMoved = false;

			isRemote2CompletelyMoved = false;

			isRemote3CompletelyMoved = false;

			print("isLadder:"+isLadder+" ladderCounter:"+ladderCounter);

			print (" isMasterCompleteMoved:"+isMasterCompleteMoved+" isRemoteCompletelyMoved:"+isRemoteCompletelyMoved+" isRemote2CompletelyMoved "+isRemote2CompletelyMoved+" isRemote3CompletelyMoved:"+isRemote3CompletelyMoved);		
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

			if (isMasterCompletelyMovedForSnake==true) 
			{

				print ("Player.transform.name " + Player.transform.name);

				MasterI = MasterIRemoteSnakeValue [snakeCounter];

				print ("After Beaten by snake MasterI:" + MasterI);

			} else if (isRemoteCompletelyMovedForSnake==true) {

				print ("Player.transform.name " + Player.transform.name);

				RemoteI = MasterIRemoteSnakeValue [snakeCounter];

				print ("After Beaten by snake RemoteI:" + RemoteI);

			}
			else if (isRemote2CompletelyMovedForSnake==true) {

				print ("Player.transform.name " + Player.transform.name);

				RemoteI2 = MasterIRemoteSnakeValue [snakeCounter];

				print ("After Beaten by snake RemoteI2:" + RemoteI2);

			}

			else if (isRemote3CompletelyMovedForSnake==true) {

				print ("Player.transform.name " + Player.transform.name);

				RemoteI3 = MasterIRemoteSnakeValue [snakeCounter];

				print ("After Beaten by snake RemoteI3:" + RemoteI3);

			}
			if (Player.transform.position != SnakeTalePosition [snakeCounter].transform.position) {

				print (Player.transform.name + "" + " is Moving");

				Player.transform.position = Vector3.MoveTowards (Player.transform.position, SnakeTalePosition [snakeCounter].transform.position, 5 * Time.deltaTime);

			}

			if (Player.transform.position == SnakeTalePosition [snakeCounter].transform.position) 
			{

				print (Player.transform.name + "" + " is Complately moved");

				isSnake = false;

				snakeCounter = 0;

				isMasterCompletelyMovedForSnake = false;

				isRemoteCompletelyMovedForSnake = false;

				isRemote2CompletelyMovedForSnake = false;

				isRemote3CompletelyMovedForSnake = false;

				print ("isMasterCompletelyMovedForSnake" + isMasterCompletelyMovedForSnake+"isRemoteCompletelyMovedForSnake"+isRemoteCompletelyMovedForSnake+" isRemote2CompletelyMovedForSnake"+isRemote2CompletelyMovedForSnake+" isRemote3CompletelyMovedForSnake"+isRemote3CompletelyMovedForSnake	);	

			}
		}

		else 
		{

			isMasterCompletelyMovedForSnake = false;

			isRemoteCompletelyMovedForSnake = false;

			isRemote2CompletelyMovedForSnake = false;

			isRemote3CompletelyMovedForSnake = false;

			print ("isMasterCompletelyMovedForSnake" + isMasterCompletelyMovedForSnake+" isRemoteCompletelyMovedForSnake"+isRemoteCompletelyMovedForSnake+" isRemote2CompletelyMovedForSnake"+isRemote2CompletelyMovedForSnake+" isRemote3CompletelyMovedForSnake"+isRemote3CompletelyMovedForSnake	);	

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
//		isMyTurn = false;
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

		print ("counter:" + counter);
		if (counter==1) 
		{

			JSONNode jn = SimpleJSON.JSONData.Parse (move as string);

			mover = float.Parse(jn ["places"].Value);

			isMasterMove = true;

			print ("Masters Data Parsed");
			//			StartCoroutine (MasterMove (mover));
		}
		else if(counter==2)
		{
			JSONNode jn = SimpleJSON.JSONData.Parse (move as string);

			mover = float.Parse(jn ["places"].Value);

			isRemoteMove = true;

			print ("Remote1 Data Parsed");
		}
		else if(counter==3)
		{
			JSONNode jn = SimpleJSON.JSONData.Parse (move as string);

			mover = float.Parse(jn ["places"].Value);

			//			isRemoteMove = true;

			isRemote2Move = true;

			print ("Remote2 Data Parsed");
		}
		else if(counter==4)
		{
			JSONNode jn = SimpleJSON.JSONData.Parse (move as string);

			mover = float.Parse(jn ["places"].Value);

			//			isRemoteMove = true;

			isRemote3Move = true;

			print ("Remote3 Data Parsed");
		}
		if (photonPlayer.IsLocal)
		{

			isMyTurn = false;

			print ("PlayerName"+photonPlayer.name);
			print ("Turn:" + turn);

			//			TurnStatus.enabled = false;
		}
		else
		{
			StartCoroutine (WaitS ());
		}
	}
	IEnumerator WaitS()
	{
		yield return new WaitForSeconds (5);

		//		TurnStatus.enabled = true;
	/*	if (counter == 0 && isMaster == true) 
		{
			isMyTurn = true;
			print ("This is Masters turn");
		}
		if (counter == 1 && isSecondPlayer == true) 
		{
			isMyTurn = true;
			print ("This is remotes turn");
		}
		else if (counter == 2 && isThirdPlayer == true) 
		{
			isMyTurn = true;
			print ("This is third Player turn");
		}
		else if (counter == 3 && isFourthPlayer == true) 
		{
			isMyTurn = true;
			print ("This is third Player turn");
		}
		*/


		//for master

		if(MasterWon==false && counter==0 && isMaster==true)
		{

			isMyTurn = true;

			print ("This is Masters turn");

		}
		else if(MasterWon==true && counter==0 && isMaster==true)
		{

			isMyTurn = true;

			print ("This is Masters Turn");

		}



		//for remote

		if(RemoteWon==false && counter==1 && isSecondPlayer==true)
		{

			isMyTurn = true;

			print ("This is remotes turn");

		}
		else if(RemoteWon==true && counter==1 && isSecondPlayer==true)
		{

			isMyTurn = true;

			print ("When Remote1 won Counter:" + counter);

			//send the turn

		}


		//for remote2

		if(Remote2Won==false && counter==2 && isThirdPlayer==true)
		{

			isMyTurn = true;

			print ("This is remote2 turn");

		}
		else if(Remote2Won==true && counter==2 && isThirdPlayer==true)
		{

			isMyTurn = true;

			print ("When Remote2 won Counter:" + counter);

			//send the turn


		}

		//for remote3
		if (Remote3Won == false && counter == 3 && isFourthPlayer == true) {
			
			isMyTurn = true;

			print ("This is remote3 turn");
		
		}
		else if (Remote3Won == true && counter == 3 && isFourthPlayer == true) 
		{
			isMyTurn = true;	
		}

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

		//		isMyTurn = true;

		if (PhotonNetwork.isMasterClient)
		{
			print ("StartTurn1111");

			this.turnManager.BeginTurn();
		}
	}

	public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		//		Debug.Log("Other player arrived turn = "+ this.turnManager.Turn );


		//		if (isMaster) 
		//		{
		//			this.GetComponent<Gamelogic1> ().PLayer2.SetActive (true);
		////			this.turnManager.SendMove ( (byte)1 , true );
		//		}
		if (PhotonNetwork.room.PlayerCount == 4)
		{
			if (this.turnManager.Turn == 0)
			{
				// when the room has two players, start the first turn (later on, joining players won't trigger a turn)
				//				this.StartTurn();
				if (PhotonNetwork.isMasterClient)
				{
					isMyTurn = true;
				}
			}
		}
	}
}
