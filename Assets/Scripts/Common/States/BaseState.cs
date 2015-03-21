using UnityEngine;
using System.Collections;

public abstract class BaseState : MonoBehaviour 
{
	private bool m_running = false;
	private bool m_complete = false;

	public abstract GameState State { get; }
	protected abstract void Reset();
	protected abstract void UpdateState();

	public delegate void GameStateCompleteCallback(GameState state);
	public event GameStateCompleteCallback OnGameStateCompleteEvent;

	private float m_transitionTimer = float.NegativeInfinity;

	void Awake()
	{
		GameManager.Instance.OnGameStateChangeEvent += OnGameStateChange;
	}

	public bool IsRunning 
	{
		get
		{
			return m_running;
		}
		private set
		{
			if(m_running != value)
			{
				IsComplete = false;

				Reset();
			}

			m_running = value;
		}
	}
	
	void Update () 
	{
		if(IsRunning)
		{
			UpdateState();
		}

		if(m_transitionTimer > 0.0f)
		{
			m_transitionTimer -= Time.deltaTime;

			if(m_transitionTimer <= 0.0f)
			{
				m_transitionTimer = float.NegativeInfinity;

				IsComplete = true;
			}
		}
	}

	protected void SetTimedTransition(float duration)
	{
		m_transitionTimer = duration;
	}

	protected bool IsComplete 
	{ 
		get
		{
			return m_complete;
		}
		set
		{
			if(m_complete != value && value == true)
			{
				if(OnGameStateCompleteEvent != null)
				{
					OnGameStateCompleteEvent(State);
				}
			}

			m_complete = value;
		}
	}

	private void OnGameStateChange(GameState state)
	{
		if(state == State)
		{
			IsRunning = true;
		}
		else
		{
			IsRunning = false;
		}
	}
}
