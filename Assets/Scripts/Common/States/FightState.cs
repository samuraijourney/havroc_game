using UnityEngine;
using System.Collections;

public enum PlayerType { Havroc = 0, Enemy = 1 };

public class FightState : BaseState 
{
	private IFightStateMember[] m_members;

	void Start () 
	{
		TimerMonitor timerMonitor = GameObject.Find ("Countdown Timer").GetComponent<TimerMonitor>();
		timerMonitor.OnTimeoutEvent += OnTimeout;
		timerMonitor.OnTimeoutCountdownEvent += OnTimeoutCountdown;

		HealthMonitor healthMonitor = GameObject.Find ("Health Bar Havroc").GetComponent<HealthMonitor>();
		healthMonitor.OnKnockoutEvent += OnKnockout;

		healthMonitor = GameObject.Find ("Health Bar Enemy").GetComponent<HealthMonitor>();
		healthMonitor.OnKnockoutEvent += OnKnockout;

		m_members = GetAllInterfaceInstances<IFightStateMember>();
	}

	override protected void Setup()
	{
	}
	
	override protected void UpdateState() 
	{
	}
	
	override protected void Clean()
	{
	}

	override public GameState State 
	{ 
		get
		{
			return GameState.Fight;
		}
	}

	private void OnKnockout(PlayerType type)
	{
		foreach(IFightStateMember member in m_members)
		{
			if(type == PlayerType.Enemy)
			{
				member.OnStateFightLose(PlayerType.Enemy);
				member.OnStateFightWin(PlayerType.Havroc);
			}
			else
			{
				member.OnStateFightLose(PlayerType.Havroc);
				member.OnStateFightWin(PlayerType.Enemy);
			}
		}

		TextSwiper swiper = GameObject.Find ("Swiper").GetComponent<TextSwiper>();

		if(type == PlayerType.Havroc)
		{
			swiper.SetPhrase (new string[]{"Fatality"});
			swiper.StartSwiper();
			SoundManager.Instance.PlayFatality();
		}
		else if(type == PlayerType.Enemy)
		{
			swiper.SetPhrase (new string[]{"Excellent"});
			swiper.StartSwiper();
			SoundManager.Instance.PlayExcellent();
		}

		IsComplete = true;
	}

	private void OnTimeout()
	{
		foreach(IFightStateMember member in m_members)
		{
			member.OnStateFightTimeout();
		}
	}

	private void OnTimeoutCountdown()
	{
		foreach(IFightStateMember member in m_members)
		{
			member.OnStateFightTimeoutCountdown();
		}
	}
}
