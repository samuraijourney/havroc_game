using UnityEngine;
using System.Collections;

public class TextSwiper : MonoBehaviour, IIntroStateMember
{
	public GameObject swiperPrefab;

	private enum SwipeState { ToMid, AtMid, ToEnd, Finished };

	private Transform m_textTransform;
	private TextMesh m_textMesh;

	private Vector3 m_localStartPosition = new Vector3(0.25f,0,0);
	private Vector3 m_localStopPosition = new Vector3(-0.25f,0,0);

	private string[] m_texts = new string[]{};

	private float m_timePercentageToMid = 0.2f;
	private float m_timePercentageAtMid = 0.6f;
	private float m_timePercentageToEnd = 0.2f;

	private int m_currentIndex = 0;

	private bool m_enabled = false;

	private SwipeState m_swipeState = SwipeState.ToMid;
	private GameObject m_swiper;

	// Use this for initialization
	void Start () 
	{
	}

	private float m_accum = 0f;
	private float m_xTranslation = 0.0f;

	void Update () 
	{
		if(m_enabled)
		{
			if(m_swipeState == SwipeState.ToMid)
			{
				m_accum +=  Time.deltaTime * 2.0f;
				m_xTranslation = Mathf.Lerp(m_localStartPosition.x,0.01f,m_accum);

				if(Mathf.Abs(m_xTranslation - 0.01f) < 0.00001)
				{
					m_swipeState = SwipeState.AtMid;
					m_accum = 0;
				}
			}
			else if(m_swipeState == SwipeState.AtMid)
			{
				m_accum +=  Time.deltaTime;
				m_xTranslation = Mathf.Lerp(0.01f,-0.01f,m_accum);

				if(Mathf.Abs(m_xTranslation + 0.01f) < 0.00001)
				{
					m_swipeState = SwipeState.ToEnd;
					m_accum = 0;
				}
			}
			else if(m_swipeState == SwipeState.ToEnd)
			{
				m_accum +=  Time.deltaTime * 2.0f;
				m_xTranslation = Mathf.Lerp(-0.01f,m_localStopPosition.x,m_accum);

				if(Mathf.Abs(m_xTranslation - m_localStopPosition.x) < 0.00001)
				{
					m_swipeState = SwipeState.Finished;
					m_accum = 0;
				}
			}
			else if(m_swipeState == SwipeState.Finished)
			{
				m_currentIndex++;

				if(m_currentIndex == m_texts.Length)
				{
					m_enabled = false;
					m_currentIndex = 0;
					
					DestroySwiper();
				}
				else
				{
					m_swipeState = SwipeState.ToMid;
					m_textMesh.text = m_texts[m_currentIndex];
				}

				//Debug.Log ("In Finished State Index:" + m_currentIndex + " Length:" + m_texts.Length);
			}

			m_textTransform.localPosition = new Vector3(m_xTranslation,0,0);
		}
	}

	private void CreateSwiper()
	{		
		m_swiper = (GameObject)Instantiate(swiperPrefab);
		m_swiper.transform.parent = transform;
		m_swiper.transform.localPosition = new Vector3(0,0,0);
		m_swiper.transform.localRotation = Quaternion.identity;
		m_textTransform = m_swiper.transform.Find ("Text");
		m_textMesh = m_swiper.GetComponentInChildren<TextMesh>();
	}

	private void DestroySwiper()
	{
		if(m_swiper != null)
		{
			GameObject.Destroy (m_swiper);
		}
	}

	public void SetPhrase(string[] text)
	{
		m_texts = text;
	}

	public void StartSwiper()
	{
		CreateSwiper();
		m_swipeState = SwipeState.ToMid;
		m_currentIndex = 0;
		m_textMesh.text = m_texts[m_currentIndex];
		m_enabled = true;
	}
	
	public void OnStateBaseStart(GameState state)
	{
		if(state == GameState.Intro)
		{
			StartSwiper();
		}
	}
	
	public void OnStateBaseEnd(GameState state)
	{
		if(state == GameState.Intro)
		{
			m_enabled = false;
			DestroySwiper();
		}
	}
}
