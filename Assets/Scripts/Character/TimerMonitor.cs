using UnityEngine;
using System.Collections;

public class TimerMonitor : MonoBehaviour 
{
	public int time = 90;

	private TextMesh m_textMesh;

	private float m_timerAccum = 0f;
	private float m_flashAccum = 0f;
	private float m_flashPeriod = 0.5f;

	private Color m_textColor;
	private bool m_isOriginalColor = true;

	// Use this for initialization
	void Start () 
	{
		m_textMesh = gameObject.GetComponent<TextMesh>();
		m_textColor = m_textMesh.color;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(time > 0)
		{
			m_timerAccum += Time.deltaTime;
			
			if(m_timerAccum >= 1.0f)
			{
				time -= 1;
				
				m_textMesh.text = time.ToString();
				
				m_timerAccum -= 1.0f;
			}
			
			if(time <= 10)
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
