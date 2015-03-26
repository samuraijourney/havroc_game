using UnityEngine;
using System.Collections;

public class EndState : BaseState 
{
	void Start () 
	{
	}

	override protected void Setup()
	{
	}
	
	override protected void UpdateState() 
	{
		if(Input.GetButtonDown("Reset1"))
		{
			IsComplete = true;
		}
	}
	
	override protected void Clean()
	{
		complete = IsComplete;
	}

	override public GameState State 
	{ 
		get
		{
			return GameState.End;
		}
	}
}
