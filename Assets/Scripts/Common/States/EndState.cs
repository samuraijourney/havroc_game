using UnityEngine;
using System.Collections;

public class EndState : BaseState 
{
	public bool complete = false;

	void Start () 
	{
		
	}
	
	override protected void UpdateState() 
	{
		Debug.Log ("EndState");

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
			return GameState.End;
		}
	}
}
