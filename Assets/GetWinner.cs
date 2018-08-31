using UnityEngine;
using System.Collections;

public class GetWinner : MonoBehaviour {	
	public int winState = 0;
	void Start()
	{
		//winState = -1;
	}
	void OnTriggerEnter(Collider mycol)
	{		
		if(mycol.gameObject.transform.parent.gameObject.name == "player1")
		{
			winState = 1;
		}
		else if(mycol.gameObject.name == "player2")
		{
			winState = 2;
		}
	}
}
