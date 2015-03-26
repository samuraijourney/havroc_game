using System;

public class FPSMonitor
{
	private int m_frameCount = 0;
	private float m_dt = 0.0f;
	private float m_updateRate;

	private DateTime m_lastDatetime;
	private bool m_syncLastDateTime = true;

	public FPSMonitor(float updateRate = 4.0f)
	{
		m_updateRate = updateRate;

		m_syncLastDateTime = true;
	}

	public void Update(float delta = 0.0f) 
	{
		float deltaTime;

		if(delta == 0.0f)
		{
			if(m_syncLastDateTime)
			{
				m_lastDatetime = DateTime.Now;
				
				m_syncLastDateTime = false;
			}
			
			TimeSpan deltaTimeSpan = DateTime.Now - m_lastDatetime;
			deltaTime = (float)deltaTimeSpan.TotalMilliseconds / 1000.0f;
			m_lastDatetime = DateTime.Now;
		}
		else
		{
			deltaTime = delta;
			m_syncLastDateTime = true;
		}

		m_frameCount++;
		m_dt += deltaTime;
		if (m_dt > 1.0f/m_updateRate)
		{
			FPS = m_frameCount / m_dt ;
			m_frameCount = 0;
			m_dt -= 1.0f/m_updateRate;

			NewUpdate = true;
		}
		else
		{
			NewUpdate = false;
		}
	}

	public bool NewUpdate 
	{
		get;
		set;
	}
	
	public float FPS 
	{
		get;
		set;
	}
}
