using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class BaseState : MonoBehaviour 
{
	public bool complete = false;
	public float timedTransition = 1.0f;

	public bool ignoreState = false;

	private bool m_running = false;
	private bool m_complete = false;

	private IBaseStateMember[] m_members;

	public abstract GameState State { get; }

	protected abstract void Setup();
	protected abstract void Clean();
	protected abstract void UpdateState();

	public delegate void GameStateCompleteCallback(GameState state);
	public event GameStateCompleteCallback OnGameStateCompleteEvent;

	private float m_transitionTimer = float.NegativeInfinity;

	protected T[] GetAllInterfaceInstances<T>()
	{
		GameObject[] gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

		List<T> list = new List<T>();

		foreach(GameObject obj in gameObjects)
		{
			if (obj.activeInHierarchy)
			{
				list.AddRange(GetInterfaces<T>(obj));
			}
		}

		return list.ToArray();
	}

	private T[] GetInterfaces<T>(this GameObject obj)
	{
		var mObjs = obj.GetComponents<MonoBehaviour>();
		
		return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
	}

	void Awake()
	{
		GameManager.Instance.OnGameStateChangeEvent += OnGameStateChange;

		m_members = GetAllInterfaceInstances<IBaseStateMember>();
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

				if(!value)
				{
					FireMemberOnStateEnd();
					Clean(); // Clean at end of state
				}
				else
				{
					Setup(); // Setup at beginning of state
					FireMemberOnStateStart();
				}
			}

			m_running = value;
		}
	}
	
	void Update () 
	{
		if(IsRunning)
		{
			if(ignoreState)
			{
				IsComplete = true;

				return;
			}

			UpdateState();

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

	private void FireMemberOnStateStart()
	{
		foreach(IBaseStateMember member in m_members)
		{
			member.OnStateBaseStart(State);
		}
	}
	
	private void FireMemberOnStateEnd()
	{
		foreach(IBaseStateMember member in m_members)
		{
			member.OnStateBaseEnd(State);
		}
	}
}
