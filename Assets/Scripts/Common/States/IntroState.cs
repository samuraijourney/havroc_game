using UnityEngine;
using System.Collections;

public class IntroState : BaseState 
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
		//Debug.Log ("IntroState");
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
			return GameState.Intro;
		}
	}
}
