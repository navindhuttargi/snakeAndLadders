using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;


public class GetCountryList : MonoBehaviour 
{

	//Declaration Part
	public string url="http://shehnaiya.com/pharomyd/index.php/webservice/countryList";//Assigning url

	JSONNode rootNode=new JSONNode();

	JSONNode resultNode=new JSONNode();

	public List<string> nodes;

	// Use this for initialization
	void Start ()
	{
		
		StartCoroutine (Web ());
	}

	IEnumerator Web()
	{
		UnityWebRequest	www = UnityWebRequest.Get (url);
		www.chunkedTransfer = false;
		www.downloadHandler = new DownloadHandlerBuffer ();
		yield return www.SendWebRequest ();

		string msg = www.downloadHandler.text;

		if (www.error != null) {
			
		} else {
			
			rootNode = SimpleJSON.JSONData.Parse (msg);


			nodes.Add (rootNode [0].ToString());
			nodes.Add (rootNode [1].ToString() );

			print (rootNode[1].Count );
			resultNode = JSONNode.Parse (rootNode [1].ToString() );

			foreach (JSONNode o in resultNode.Childs) 
			{
				print (o[1]);
			}
		}
	}
}
