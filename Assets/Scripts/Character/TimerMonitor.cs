using UnityEngine;
using System.Collections;

public class TimerMonitor : MonoBehaviour, IFightStateMember
{
	public int startTime = 90;

	private int m_time;

	private TextMesh m_textMesh;

	private float m_timerAccum = 0f;
	private float m_flashAccum = 0f;
	private float m_flashPeriod = 0.5f;

	private Color m_textColor;
	private bool m_isOriginalColor = true;

	private bool m_enabled = false;

	// Use this for initialization
	void Start () 
	{
		m_textMesh = gameObject.GetComponent<TextMesh>();
		m_textColor = m_textMesh.color;

		m_time = startTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_enabled)
		{
			if(m_time > 0)
			{
				m_timerAccum += Time.deltaTime;
				
				if(m_timerAccum >= 1.0f)
				{
					m_time -= 1;
					
					m_textMesh.text = m_time.ToString();
					
					m_timerAccum -= 1.0f;
				}
				
				if(m_time <= 10)
				{
					m_flashAccum += Time.deltaTime;
					
					if(m_flashAccum >= m_flashPeriod)
					{
						if(m_isOriginalColor)
						{
							m_textMesh.color = Color.clear;
							m_isOriginalColor = false;
						}
						else
						{
							m_textMesh.color = m_textColor;
							m_isOriginalColor = true;
						}
						
						m_flashAccum -= m_flashPeriod;
					}
				}
			}
			else
			{
				m_textMesh.color = m_textColor;
			}
		}
	}

	public void OnStateBaseStart(GameState state)
	{
		if(state == GameState.Fight)
		{
			m_time = startTime;
			m_enabled = true;

			m_textMesh.text = m_time.ToString();
		}
	}
	
	public void OnStateBaseEnd(GameState state)
	{
		if(state == GameState.Fight)
		{
			m_time = startTime;
			m_enabled = false;

			m_textMesh.text = m_time.ToString();
		}
	}
}
