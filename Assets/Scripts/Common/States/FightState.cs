using UnityEngine;
using System.Collections;

public class FightState : BaseState 
{
	public bool complete = false;

	void Start () 
	{
		
	}
	
	override protected void UpdateState() 
	{
		Debug.Log ("FightState");

		IsComplete = complete;
	}
	
	override protected void Reset()
	{
		complete = IsComplete;
	}
	
	override public GameState State 
	{ 
		get
		{
			return GameState.Fight;
		}
	}
}
