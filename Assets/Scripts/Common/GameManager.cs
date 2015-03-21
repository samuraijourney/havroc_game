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
		IntroState 		 introState 	  = GameObject.Find("Intro State").GetComponent<IntroState>();
		CalibrationState calibrationState = GameObject.Find("Calibration State").GetComponent<CalibrationState>();
		FightState 		 fightState 	  = GameObject.Find("Fight State").GetComponent<FightState>();
		EndState 		 endState 		  = GameObject.Find("End State").GetComponent<EndState>();

		introState.OnGameStateCompleteEvent 	  += OnGameStateComplete;
		calibrationState.OnGameStateCompleteEvent += OnGameStateComplete;
		fightState.OnGameStateCompleteEvent 	  += OnGameStateComplete;
		endState.OnGameStateCompleteEvent 		  += OnGameStateComplete;
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
