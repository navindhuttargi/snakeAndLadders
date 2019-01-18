using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDice : MonoBehaviour 
{
	public SpriteRenderer sp;
	private Sprite[] sprites;
	// Use this for initialization
	void Start () 
	{
//		sp = GetComponent<SpriteRenderer>();
		sprites = Resources.LoadAll<Sprite> ("DiceSprites/");

	}
	
	// Update is called once per frame
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
		print (finalRoll);
	}
}
