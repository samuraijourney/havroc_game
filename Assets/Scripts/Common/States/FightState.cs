using UnityEngine;
using System.Collections;

public class FightState : BaseState 
{
	TimerMonitor m_timerMonitor;

	void Start () 
	{
		m_timerMonitor = GameObject.Find ("Countdown Timer").GetComponent<TimerMonitor>();
	}

	override protected void Setup()
	{
		SetTimedTransition (m_timerMonitor.startTime);
	}
	
	override protected void UpdateState() 
	{
		//Debug.Log ("FightState");

		IsComplete = complete;
	}
	
	override protected void Clean()
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
