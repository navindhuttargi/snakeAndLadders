using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerScript : MonoBehaviour 
{
	public void LoadLevel1()
	{
		SceneManager.LoadScene (1);
	}
	public void LoadLevel2()
	{
		SceneManager.LoadScene (2);
	}
	public void LoadLevel3()
	{
		SceneManager.LoadScene (3);
	}
	public void LoadLeve1Offline()
	{
		SceneManager.LoadScene (4);
	}
	public void LoadLeve2Offline()
	{
		SceneManager.LoadScene (5);
	}
	public void LoadMainMenu()
	{
		SceneManager.LoadScene (0);
	}
}
