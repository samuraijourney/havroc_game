using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour, IIntroStateMember, IFightStateMember, IEndStateMember
{
	public float audioFadeFactor = 1.0f;

	private AudioSource m_guileThemeAudio;
	private bool m_fadeOut = false;

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
		
		m_guileThemeAudio = audioSources[0];
	}
	
	void Update () 
	{
		if(m_fadeOut)
		{
			m_guileThemeAudio.volume -= audioFadeFactor * Time.deltaTime;
		}
	}

	public void OnStateBaseStart(GameState state)
	{
		if(state == GameState.Fight)
		{
			m_fadeOut = false;
			m_guileThemeAudio.volume = 1.0f;
			m_guileThemeAudio.Play();
		}
	}

	public void OnStateBaseEnd(GameState state)
	{
	}

	public void OnStateFightWin(PlayerType type)
	{
	}

	public void OnStateFightLose(PlayerType type)
	{

	}

	public void OnStateFightTimeout()
	{
		m_fadeOut = true;
	}
}
