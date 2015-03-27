using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour, IIntroStateMember, IFightStateMember, IEndStateMember
{
	public float volumeFadeFactor = 1.0f;
	public float pitchFadeFactor = 1.0f;

	private AudioSource m_themeAudio;
	private AudioSource m_fightAudio;
	private AudioSource m_oneAudio;
	private AudioSource m_twoAudio;
	private AudioSource m_threeAudio;
	private AudioSource m_fourAudio;
	private AudioSource m_roundAudio;
	private AudioSource m_excellentAudio;
	private AudioSource m_fatalityAudio;

	private AudioSource[] m_chainedRoundOneFight;
	private AudioSource[] m_chainedRoundTwoFight;
	private AudioSource[] m_chainedRoundThreeFight;
	private AudioSource[] m_chainedRoundFourFight;
	private AudioSource[] m_chainedFatality;
	private AudioSource[] m_chainedExcellent;

	private float[] m_delayRoundOneFight;
	private float[] m_delayRoundTwoFight;
	private float[] m_delayRoundThreeFight;
	private float[] m_delayRoundFourFight;
	private float[] m_delayFatality;
	private float[] m_delayExcellent;
	
	private bool m_fadeOut = false;
	private bool m_speedup = false;

	private static SoundManager m_instance;
	
	public static SoundManager Instance
	{
		get
		{
			if(m_instance == null)
			{
				m_instance = GameObject.FindObjectOfType<SoundManager>();
			}
			
			return m_instance;
		}
	}
	
	void Start () 
	{
		AudioSource[] audioSources = GetComponents<AudioSource>();
		
		m_themeAudio 		= audioSources[0];
		m_fightAudio 		= audioSources[1];
		m_oneAudio 			= audioSources[2];
		m_twoAudio 			= audioSources[3];
		m_threeAudio 		= audioSources[4];
		m_fourAudio 		= audioSources[5];
		m_roundAudio 		= audioSources[6];
		m_excellentAudio 	= audioSources[7];
		m_fatalityAudio 	= audioSources[8];

		m_chainedRoundOneFight = new AudioSource[]{m_roundAudio, m_oneAudio, m_fightAudio};
		m_chainedRoundTwoFight = new AudioSource[]{m_roundAudio, m_twoAudio, m_fightAudio};
		m_chainedRoundThreeFight = new AudioSource[]{m_roundAudio, m_threeAudio, m_fightAudio};
		m_chainedRoundFourFight = new AudioSource[]{m_roundAudio, m_fourAudio, m_fightAudio};
		m_chainedFatality = new AudioSource[]{m_fatalityAudio};
		m_chainedExcellent = new AudioSource[]{m_excellentAudio};

		m_delayRoundOneFight = new float[]{0,0.5f,0};
		m_delayRoundTwoFight = new float[]{0,0.5f,0};
		m_delayRoundThreeFight = new float[]{0,0.5f,0};
		m_delayRoundFourFight = new float[]{0,0.5f,0};
		m_delayFatality = new float[]{0};
		m_delayExcellent = new float[]{0};
	}
	
	void Update () 
	{
		if(m_fadeOut)
		{
			m_themeAudio.volume -= volumeFadeFactor * Time.deltaTime;
			m_themeAudio.pitch -= pitchFadeFactor * Time.deltaTime;
		}
		else if(m_speedup)
		{
			if(m_themeAudio.isPlaying)
			{
				if (m_themeAudio.pitch > 0)
				{
					m_themeAudio.pitch = 1.2f;
				}
			}
		}
	}

	private float PlayChainedAudioSources(AudioSource[] sources, float[] delay)
	{
		if(sources.Length != delay.Length)
		{
			return 0.0f;
		}

		float skip = 0.0f;

		for(int i = 0; i < sources.Length; i++)
		{
			sources[i].PlayDelayed(skip);

			skip += sources[i].clip.length;
			skip += delay[i];
		}

		return skip;
	}

	public float PlayRoundOneFight()
	{
		return PlayChainedAudioSources(m_chainedRoundOneFight, m_delayRoundOneFight);
	}

	public float PlayRoundTwoFight()
	{
		return PlayChainedAudioSources(m_chainedRoundTwoFight, m_delayRoundTwoFight);
	}

	public float PlayRoundThreeFight()
	{
		return PlayChainedAudioSources(m_chainedRoundThreeFight, m_delayRoundThreeFight);
	}
	
	public float PlayRoundFourFight()
	{
		return PlayChainedAudioSources(m_chainedRoundFourFight, m_delayRoundFourFight);
	}

	public float PlayFatality()
	{
		return PlayChainedAudioSources(m_chainedFatality, m_delayFatality);
	}

	public float PlayExcellent()
	{
		return PlayChainedAudioSources(m_chainedExcellent, m_delayExcellent);
	}

	public float PlayTheme(float delay)
	{
		m_themeAudio.PlayDelayed (delay);

		return m_themeAudio.clip.length;
	}

	public void StopAll()
	{
		m_themeAudio.Stop();
		m_fightAudio.Stop();
		m_oneAudio.Stop();
		m_twoAudio.Stop();
		m_threeAudio.Stop();
		m_fourAudio.Stop();
		m_roundAudio.Stop();
		m_excellentAudio.Stop();
		m_fatalityAudio.Stop();
	}

	public void OnStateBaseStart(GameState state)
	{
		if(state == GameState.Intro)
		{
			m_fadeOut = false;
			m_speedup = false;
			m_themeAudio.volume = 1.0f;
			m_themeAudio.pitch = 1.0f;
		}
	}
	
	public void OnStateBaseEnd(GameState state)
	{
	}
	
	public void OnStateFightWin(PlayerType type)
	{
		m_fadeOut = true;
	}
	
	public void OnStateFightLose(PlayerType type)
	{
		m_fadeOut = true;
	}
	
	public void OnStateFightTimeout()
	{
		m_fadeOut = true;
	}

	public void OnStateFightTimeoutCountdown()
	{
		m_speedup = true;
	}

	public void OnStateEndCameraPanAway()
	{
	}
	
	public void OnStateEndCameraPanBack()
	{
	}
}
