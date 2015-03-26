using UnityEngine;
using System.Collections;

public class IntroState : BaseState 
{

	void Start () 
	{
	}

	override protected void Setup()
	{
		float duration = SoundManager.Instance.PlayRoundOneFight();

		SetTimedTransition(duration + 0.5f);

		SoundManager.Instance.PlayTheme(duration);

		string[] swipeTexts = new string[]{"Round 1","Fight!"};

		TextSwiper swiper = GameObject.Find ("Swiper").GetComponent<TextSwiper>();
		swiper.SetPhrase (swipeTexts);
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
			return GameState.Intro;
		}
	}
}
