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
		//if(Input.GetButtonDown("Reset"))
		//{
		//	IsComplete = true;
		//}
	}
	
	override protected void Clean()
	{
	}

	override public GameState State 
	{ 
		get
		{
			return GameState.End;
		}
	}
}
