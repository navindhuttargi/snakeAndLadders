using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoPlayersScript : MonoBehaviour 
{
	public GameObject Player1, Player2,Player1Button,Player2Button,Maze,PlayerNameMenu,GameMenu,FakePlayer1,Waypoint;// Player3, Player4;
	public int[] MasterIRemoteIValue = new int[13];
	public int[] MasterIRemoteSnakeValue = new int[15];
	public bool Player1Turn, Player2Turn;
	public bool Player1CanMove, Player2CanMove;
	public bool Player1CompletelyMoved, Player2CompletelyMoved;
	public bool Player1CompletelyMovedForSnake, Player2CompletelyMovedForSnake;
	public bool Player1CanProceed,Player2CanProceed;
	public bool isLadder=false,isSnake=false;
	public bool Player1Won,Player2Won;
	public int counter=0,ladderCounter=0,snakeCounter=0,snc=0;
	public SpriteRenderer sp,sp2;
	private Sprite[] sprites;
	public int P1num = 1,P2num=1;
	public List<Transform> wayPoints;
	public List<GameObject> LadderList;
	public List<GameObject> LadderDestination;
	public List<GameObject> SnakeMouthPosition;
	public List<GameObject> SnakeTalePosition;
	public static int  Player1CurrentPos = 1,Player2CurrentPos=1,DiceNumber=0;
	public Text Player1Status, Player2Status,GameOver;
	public string NamePlayer1="";
	public string NamePlayer2="";


	// Use this for initialization
	void Start () 
	{
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

		Player1Turn = true;

		sprites = Resources.LoadAll<Sprite> ("DiceSprites/");

	}
	
	// Update is called once per frame
	void Update () 
	{

		print ("Counter:" + counter);

		ButtonEnableDisable ();

		PlayerMovement ();
	
	}
	void ButtonEnableDisable()
	{
		

		if (Player1Won == true || Player2Won == true) 
		{

			Player1Button.SetActive (false);
			Player2Button.SetActive (false);
			Player1Status.enabled = false;
			Player2Status.enabled = false;
			if (Player1Won == true) 
			{
				
//				GameOver.text = "PLAYER1 WINS".ToString();

				GameOver.text =NamePlayer1+" WINS".ToString();
			}
			if (Player2Won == true) 
			{
				
				GameOver.text =NamePlayer2+" WINS".ToString();
			}
		} 
		else 
		{
			if (NamePlayer1.Length==0) 
			{
				NamePlayer1 = "Player1";
			}
			if (NamePlayer2.Length == 0) 
			{
				NamePlayer2 = "Computer";
			}
			if (Player1Turn == true && counter == 0) 
			{
				Player1Button.GetComponent<Button> ().interactable = true;
				Player1Status.text = NamePlayer1+"'s Turn";

			} 
			else 
			{	
				Player1Button.GetComponent<Button> ().interactable = false;
				Player1Status.text = "Wait";
			}
			if (Player2Turn == true && counter == 1)
			{
				Player2Button.GetComponent<Button> ().interactable = true;
				Player2Status.text = NamePlayer2+"'s Turn";
				StartCoroutine (AutoPlay ());
				Player2Dice ();
			}
			else
			{
				Player2Button.GetComponent<Button> ().interactable = false;
				Player2Status.text = "Wait";
			}
		}
	}
	void PlayerMovement()
	{

		MovePlayer1 ();

		MovePlayer2 ();



	}
	void MovePlayer1()
	{
		print ("Executing Player1");
		if (Player1.transform.localPosition == wayPoints [99].transform.position && Player1CurrentPos == 99) 
		{
			Player1Won = true;
		}
		if (Player1CanMove==true && ((Player1CurrentPos + DiceNumber) <= 99) && Player1CanProceed == false) 
		{
			
			Player1CanProceed = true;

			Player2CanProceed = false;
		
		}
		else if (Player1CanMove==true && ((Player1CurrentPos + DiceNumber) > 99) && Player1CanProceed == false) 
		{

			Player1CanMove = false;

			Player2CanProceed = false;

			Player1Turn = false;

			Player2Turn = true;

			DiceNumber = 0;

			snc = 1;

		}
		if (Player1CanProceed == true) 
		{
			if (Player1CanMove == true && counter == 1) 
			{

				print ("PlayerDiceNumber" + DiceNumber);

				if (P1num <= DiceNumber && DiceNumber>0) 
				{
					
					print("DiceNumber:"+DiceNumber+" P1num:"+P1num);

					print ("Player1Moving");

					if (Player1.transform.localPosition != wayPoints [99].transform.position) 
					{

						Player1.transform.localPosition = Vector3.MoveTowards (Player1.transform.localPosition, wayPoints [Player1CurrentPos].transform.position, 5 * Time.deltaTime);
					
					}
					if (Player1.transform.localPosition == wayPoints [Player1CurrentPos].transform.position) 
					{

						print ("Increamenting");

						Player1CurrentPos++;

						P1num++;

					}
				}
				if (P1num == DiceNumber + 1) 
				{		

					if (Player1.transform.localPosition == wayPoints [Player1CurrentPos - 1].transform.position) {

						print ("Player1CanMove Became false");

						Player1CanMove = false;

						P1num = 0;

						Player1CurrentPos--;

						print ("Player1CurrentPos:" + Player1CurrentPos);

						print ("P1num:" + P1num);

						Player1CompletelyMoved = true;

						Player1CompletelyMovedForSnake = true;

					}
				}
			}
			if (Player1CompletelyMoved == true) 
			{

				CheckLadder (Player1);

			}

			if (Player1CompletelyMovedForSnake == true) 
			{

				CheckSnakeAtThisPosition (Player1);

			}

			if (Player1CompletelyMoved==false && Player1CompletelyMovedForSnake==false && snc==1) 
			{
				Player2Turn = true;

				Player1Turn = false;

				print ("ladderflag and snakeflag became false");
			}
		}
	}
	void MovePlayer2()
	{

		if (Player2.transform.localPosition == wayPoints [99].transform.position && Player2CurrentPos == 99) 
		{
			Player2Won = true;
		}

		if (Player2CanMove==true && ((Player2CurrentPos + DiceNumber) <= 99) && Player2CanProceed == false) 
		{

			Player2CanProceed = true;

			Player1CanProceed = false;

		}
		else if (Player2CanMove==true && ((Player2CurrentPos + DiceNumber) > 99) && Player2CanProceed == false) 
		{

			print ("Player2 Cant Move");

			Player1CanProceed = false;

			DiceNumber = 0;

			counter = 0;

			snc = 0;

			Player1Turn = true;

			Player2Turn = false;

			Player2CanMove = false;

		}
		if (Player2CanProceed == true) {
			if (Player2CanMove == true && counter == 2) {

				print ("PlayerDiceNumber" + DiceNumber);

				if (P2num <= DiceNumber && DiceNumber>0) {

					print ("Player1Moving");

					if (Player2.transform.localPosition != wayPoints [99].transform.position) 
					{

						Player2.transform.localPosition = Vector3.MoveTowards (Player2.transform.localPosition, wayPoints [Player2CurrentPos].transform.position, 5 * Time.deltaTime);
					}

					if (Player2.transform.localPosition == wayPoints [Player2CurrentPos].transform.position) 
					{

						Player2CurrentPos++;

						P2num++;

					}
				}
				if (P2num == DiceNumber + 1) {		

					if (Player2.transform.localPosition == wayPoints [Player2CurrentPos - 1].transform.position) {

						print ("Player1CanMove Became false");

						Player2CanMove = false;

						P2num = 0;

						Player2CurrentPos--;

						print ("Player2CurrentPos:" + Player2CurrentPos);

						print ("P2num:" + P2num);

						Player2CompletelyMoved = true;

						Player2CompletelyMovedForSnake = true;

					}
				}
			}
			if (Player2CompletelyMoved == true) {

				CheckLadder (Player2);

			}

			if (Player2CompletelyMovedForSnake == true) {

				CheckSnakeAtThisPosition (Player2);

			}
			if (Player2CompletelyMoved==false && Player2CompletelyMovedForSnake==false && snc==2) 
			{
				Player1Turn = true;

				Player2Turn = false;

				counter = 0;

				snc = 0;
			}
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

			if (Player1CompletelyMoved == true)
			{
				Player1CurrentPos = MasterIRemoteIValue [ladderCounter];

				print ("After climbing the Player1CurrentPos:" + Player1CurrentPos); 
			} 

			else if (Player2CompletelyMoved == true) 
			{
				Player2CurrentPos = MasterIRemoteIValue [ladderCounter];

				print ("After climbing the Player2CurrentPos:" + Player2CurrentPos); 
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

				Player1CompletelyMoved = false;

				Player2CompletelyMoved = false;

				ladderCounter = 0;

				print("isLadder:"+isLadder+" Player1CompletelyMoved:"+Player1CompletelyMoved+" Player2CompletelyMoved:"+Player2CompletelyMoved+" ladderCounter:"+ladderCounter);
			}
		}
		else 
		{

			Player1CompletelyMoved = false;

			Player2CompletelyMoved = false;

			print ("Player1CompletelyMoved:" + Player1CompletelyMoved + " Player2CompletelyMoved:" + Player2CompletelyMoved);

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

			if (Player1CompletelyMovedForSnake==true) {

				print ("Player.transform.name " + Player.transform.name);

				Player1CurrentPos = MasterIRemoteSnakeValue [snakeCounter];

				print ("After Beaten by snake MasterI:" + Player1CurrentPos);

			} else if (Player2CompletelyMovedForSnake==true) {

				print ("Player.transform.name " + Player.transform.name);

				Player2CurrentPos = MasterIRemoteSnakeValue [snakeCounter];

				print ("After Beaten by snake RemoteI:" + Player2CurrentPos);

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

				Player1CompletelyMovedForSnake = false;

				Player2CompletelyMovedForSnake = false;

				if (Player.gameObject.name == "Master") 
				{
					snc = 1;
				}
				if (Player.gameObject.name == "Remote") 
				{
					snc = 2;
				}

				print ("isMasterCompletelyMovedForSnake" + Player1CompletelyMovedForSnake+"isRemoteCompletelyMovedForSnake"+Player2CompletelyMovedForSnake	);	

			}
		}

		else 
		{

			Player1CompletelyMovedForSnake = false;

			Player2CompletelyMovedForSnake = false;

			if (Player.gameObject.name == "Master") 
			{
				snc = 1;
			}
			if (Player.gameObject.name == "Remote") 
			{
				snc = 2;
			}

			print ("isMasterCompletelyMovedForSnake" + Player1CompletelyMovedForSnake+"isRemoteCompletelyMovedForSnake"+Player2CompletelyMovedForSnake	);	

		}
	}
	public void StartGame()
	{
		
		Maze.SetActive (true);
		Player1.SetActive (true);
		Player2.SetActive (true);
		PlayerNameMenu.SetActive (false);
		GameMenu.SetActive (true);
		FakePlayer1.SetActive (false);
		Waypoint.SetActive (true);
	}
	public void Player1Name(string P1Name)
	{
		NamePlayer1 = P1Name;
	}
	public void Player1Dice()
	{
		if (Player1Turn == true) 
		{

			StartCoroutine (SpinDice (1));

			counter = 1;

			print ("Hello");
		
		}
	}
	public void Player2Dice()
	{
		if (Player2Turn == true) 
		{
		
			StartCoroutine (SpinDice (2));

			print ("Hello");
			counter = 2;
		
		}
	}
	IEnumerator SpinDice(int num)
	{
		
		int RandomDice=0;
	
		int finalRoll=0;

		if (num == 1) 
		{

			for (int i = 0; i < 5; i++)
			{

				RandomDice = Random.Range (0, 6);

				sp.sprite = sprites [RandomDice];

				yield return new WaitForSeconds (.1f);


			}
		}
		if (num == 2) 
		{
			for (int i = 0; i < 5; i++)
			{

				RandomDice = Random.Range (0, 6);

				sp2.sprite = sprites [RandomDice];

				yield return new WaitForSeconds (.1f);


			}
		}

		finalRoll = RandomDice + 1;

		if (num == 1) 
		{
			
			DiceNumber = finalRoll;

			Player1CanMove = true;

		}
		else if (num == 2) 
		{
		
			DiceNumber = finalRoll;

			Player2CanMove = true;

		}
		print (finalRoll);
	}
	IEnumerator AutoPlay()
	{
		yield return new WaitForSeconds (1f);

	}
}
