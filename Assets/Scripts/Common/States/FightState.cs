using UnityEngine;
using System.Collections;

public class FightState : BaseState 
{
	void Start () 
	{
		SetTimedTransition (timedTransition);
	}

	override protected void Setup()
	{

	}
	
	override protected void UpdateState() 
	{
		//Debug.Log ("FightState");

		IsComplete = complete;
	}
	
	override protected void Clean()
	{
		complete = IsComplete;
		SetTimedTransition (timedTransition);
	}

	override public GameState State 
	{ 
		get
		{
			return GameState.Fight;
		}
	}
}
