using UnityEngine;
using System.Collections;

public class EndState : BaseState 
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
		Debug.Log ("EndState");
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
			return GameState.End;
		}
	}
}
