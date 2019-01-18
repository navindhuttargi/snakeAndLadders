using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Networking;

public class RegistrationControllerScript : MonoBehaviour 
{
//	public List<GameObject> registrationElements;

	public GameObject dropDownForCountry;

//	string username,password,mobile,state,city,player1,player2,player3,birthdate,email;

	public List<string> countryNames;

	string countryListUrl = "http://shehnaiya.com/pharomyd/index.php/webservice/countryList";

	string msgData ;

	string countryName;

	int resultCount;

	JSONNode resultNode = new JSONNode();

	void Start()
	{

		GetCountryNameData ();
	}

	public void FillUpCountryList()
	{
		dropDownForCountry.GetComponent<Dropdown> ().AddOptions (countryNames);
	}

	public void GetCountryNameData()
	{
		WWWForm form = new WWWForm ();

//		WWW www = new WWW(countryListUrl);

		StartCoroutine (GetStatusData ( countryListUrl ));
	}

	IEnumerator GetStatusData( string url )
	{

		UnityWebRequest www = UnityWebRequest.Get (countryListUrl);
		
		www.chunkedTransfer = false;
		
		www.downloadHandler = new DownloadHandlerBuffer ();
		
		yield return www.Send ();
		
		if ( www.error != null  ) {
			Debug.Log("unity WWW Error: "+ www.error);
			
		} else {

			
			msgData =  www.downloadHandler. text;

			resultNode = SimpleJSON.JSONData.Parse( msgData ) ;
			
			resultCount = resultNode ["result"].Count;
			
			foreach(JSONNode o in resultNode ["result"].Childs )
			{
				countryName = o [1];
				
				//				print (o [1] );
				
				countryNames.Add (countryName);
			}
			
			FillUpCountryList ();
		}

//		Debug.Log ("unity inside GetStatusData ONLINE PLAYERS ");

//		yield return www;
////
//		if (www.error == null)
//		{
////			Debug.Log("unity Online player response : " + www.text );
//
//
//		} 
//		else
//		{
//		}    
	}
		
//	public void GetDataInRegistrationElements()
//	{
//		if(	registrationElements [0].GetComponent<InputField> ().text.Length !=0 &&
//			registrationElements [1].GetComponent<InputField> ().text.Length !=0 &&
//			registrationElements [2].GetComponent<InputField> ().text.Length !=0 &&
//			registrationElements [3].GetComponent<InputField> ().text.Length !=0 &&
//			registrationElements [4].GetComponent<InputField> ().text.Length !=0 &&
//			registrationElements [5].GetComponent<InputField> ().text.Length !=0 &&
//			registrationElements [6].GetComponent<InputField> ().text.Length !=0 &&
//			registrationElements [7].GetComponent<InputField> ().text.Length !=0 &&
//			registrationElements [8].GetComponent<InputField> ().text.Length !=0 &&
//			registrationElements [9].GetComponent<InputField> ().text.Length !=0)
//			{
//
//					username = registrationElements [0].GetComponent<InputField> ().text;
//			
//					password= registrationElements [1].GetComponent<InputField> ().text;
//			
//					mobile= registrationElements [2].GetComponent<InputField> ().text;
//			
//					state= registrationElements [3].GetComponent<InputField> ().text;
//			
//					city= registrationElements [4].GetComponent<InputField> ().text;
//			
//					player1= registrationElements [5].GetComponent<InputField> ().text;
//			
//					player2= registrationElements [6].GetComponent<InputField> ().text;
//			
//					player3= registrationElements [7].GetComponent<InputField> ().text;
//			
//					birthdate= registrationElements [8].GetComponent<InputField> ().text;
//			
//					email= registrationElements [9].GetComponent<InputField> ().text;
//			}
//			else
//			{
//				Debug.Log("Please fillup all details");
//			}
//	}
}
