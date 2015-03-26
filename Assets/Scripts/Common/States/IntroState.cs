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
		float[] swipeDelays = new float[]{0.5f,0.3f};

		TextSwiper swiper = GameObject.Find ("Swiper").GetComponent<TextSwiper>();
		swiper.SetPhrase (swipeTexts,swipeDelays);

	}
	
	override protected void UpdateState() 
	{
		//Debug.Log ("IntroState");
	}
	
	override protected void Clean()
	{
		complete = IsComplete;
	}

	override public GameState State 
	{ 
		get
		{
			return GameState.Intro;
		}
	}
}
