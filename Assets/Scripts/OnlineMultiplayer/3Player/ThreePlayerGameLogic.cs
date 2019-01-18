using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using SimpleJSON;
using ExitGames.Client.Photon;

public class ThreePlayerGameLogic : PunBehaviour
{
	public GameObject Player1, Player2,Player3;
	public static GameObject MasterName, RemoteName, Remote2Name;
	// Use this for initialization
	void Start () 
	{
		MasterName = GameObject.Find ("MasterName");
		RemoteName = GameObject.Find ("RemoteName");
		Remote2Name = GameObject.Find ("Remote2Name");
		print (MasterName.transform.position);
		print (RemoteName.transform.position);
		print (Remote2Name.transform.position);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (PhotonNetwork.playerList.Length == 3)
		{
				

//				RemoteName.GetComponent<Text> ().text = PhotonNetwork.playerList [1].NickName;

//				Remote2Name.GetComponent<Text> ().text = PhotonNetwork.playerList [2].NickName;
		}	
	}
}
