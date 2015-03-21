using UnityEngine;
using System.Collections;

public class CalibrationState : BaseState 
{
	public bool complete = false;

	void Start () 
	{
	
	}

	override protected void UpdateState() 
	{
		Debug.Log ("CalibrationState");

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
			return GameState.Calibration;
		}
	}
}
