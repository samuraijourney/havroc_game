using UnityEngine;
using System.Collections;

public enum GameState
{
	Intro 		= 0,
	Calibration = 1,
	Fight 		= 2,
	End 		= 3,
	None		= 4
};

public class GameManager : MonoBehaviour 
{
	public GameState gameState = GameState.None;
	public bool reverseOrder = false;

	public delegate void GameStateChangeCallback(GameState state);
	public event GameStateChangeCallback OnGameStateChangeEvent;

	private static readonly int m_numOfStates = 4;
	private ArrayList m_states;

	private GameState m_currentGameState = GameState.None;

	private static GameManager m_instance;

	public static GameManager Instance
	{
		get
		{
			if(m_instance == null)
			{
				m_instance = GameObject.FindObjectOfType<GameManager>();
			}

			return m_instance;
		}
	}

	void Awake()
	{
		m_states = new ArrayList ();

		IntroState 		 introState 	  = gameObject.AddComponent<IntroState> ();
		CalibrationState calibrationState = gameObject.AddComponent<CalibrationState> ();
		FightState 		 fightState 	  = gameObject.AddComponent<FightState> ();
		EndState 		 endState 		  = gameObject.AddComponent<EndState> ();

		introState.OnGameStateCompleteEvent 	  += OnGameStateComplete;
		calibrationState.OnGameStateCompleteEvent += OnGameStateComplete;
		fightState.OnGameStateCompleteEvent 	  += OnGameStateComplete;
		endState.OnGameStateCompleteEvent 		  += OnGameStateComplete;

		m_states.Add(introState);
		m_states.Add(calibrationState);
		m_states.Add(fightState);
		m_states.Add(endState);
	}
	
	void Start () 
	{
		CurrentGameState = GameState.Intro;
	}

	void Update () 
	{
		CurrentGameState = gameState;
	}

	public GameState CurrentGameState
	{
		get
		{
			return m_currentGameState;
		}
		private set
		{
			if(m_currentGameState != value)
			{
				if(OnGameStateChangeEvent != null)
				{
					OnGameStateChangeEvent(value);
				}
			}

			m_currentGameState = value;
		}
	}

	private void OnGameStateComplete(GameState state)
	{
		int index = (int)state;

		if(!reverseOrder)
		{
			index++;
			index %= m_numOfStates;
		}
		else
		{
			index--;
			index = index >= 0 ? index : m_numOfStates - 1;
		}

		CurrentGameState = (GameState)index;
		gameState = CurrentGameState;
	}
}
