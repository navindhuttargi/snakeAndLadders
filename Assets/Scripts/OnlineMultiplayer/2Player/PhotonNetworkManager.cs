using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class PhotonNetworkManager : Photon.PunBehaviour
{
	[SerializeField] private GameObject MasterPlayer,RemotePlayer;
	[SerializeField] private GameObject SpawnPointMaster,SpawnPointSlave;
	[SerializeField] public static int name=0;
	public InputField PlayerNameField;
	public InputField RoomNameField;
	public string roomName="MyRoom";
	public static bool isMaster,isMaster1,isRemote;
	public void Awake()
	{
		PhotonNetwork.ConnectUsingSettings("0.9");
	}
	//Button to Create room
	public void Create()
	{
		PhotonNetwork.CreateRoom(this.roomName, new RoomOptions() { MaxPlayers = 2 }, null);
	}


	//Button to Join Room
	public void Join()
	{
		PhotonNetwork.JoinRoom(this.roomName);
	}
	public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
		print("OnJoinedRoom");
		if (!isMaster) 
		{
			PhotonNetwork.LoadLevel(1);
			StartCoroutine (SpawnRemote ());
		}
    }
	public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        PhotonNetwork.LoadLevel(1);
        isMaster = true;
		print ("IsMaster" + isMaster);
		StartCoroutine (SpawnMaster ());
		print ("OnCreatedRoom");
    }

	public void EnterPName(string PName)
	{
		PhotonNetwork.playerName = PName;
	}
	public void EnterRName(string RName)
	{
		this.roomName = RName;
	}
	void Update () 
	{
		print (isMaster);

		if (PhotonNetwork.playerList.Length == 2)
		{
			isRemote = true;
			isMaster1 = true;
//			UIManager.OtherPlayerToconnect.SetActive (false);
			print ("New Player is Added");
			print ("isRemote:" + isRemote);
		}
		else
		{
			isRemote = false;

		}
	}
	IEnumerator SpawnMaster()
	{
		yield return new WaitForSeconds (1f);
		PhotonNetwork.Instantiate (MasterPlayer.name, SpawnPointMaster.transform.position, SpawnPointMaster.transform.rotation, 0);
	}
	IEnumerator SpawnRemote()
	{
		yield return new WaitForSeconds (1f);
		PhotonNetwork.Instantiate (RemotePlayer.name, SpawnPointSlave.transform.position, SpawnPointSlave.transform.rotation, 0);
	}
}
