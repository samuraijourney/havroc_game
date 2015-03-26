using UnityEngine;
using System;

public class IMUInitializer
{
	private int m_xPoseIterations = 0;
	private int m_yPoseIterations = 0;

	private IMUCalibrator m_calibrator;
	private CalibrationPose m_currentPose = CalibrationPose.X;

	private int m_calibrationDuration = 100;

	private float m_timerCountdownStart = 10.0f;
	private float m_timerCountdown = 0.0f;

	private bool m_complete = false;

	private DateTime m_lastDatetime;
	private bool m_syncLastDateTime = true;

	public IMUInitializer(ref IMUCalibrator calibrator)
	{
		m_calibrator = calibrator;
	}

	public void Reset(bool clearCalibrator)
	{
		m_xPoseIterations = 0;
		m_yPoseIterations = 0;

		m_timerCountdown = 0f;

		m_complete = false;

		if(clearCalibrator)
		{
			m_calibrator.Reset();
		}
	}
	
	public void Update(float w, float x, float y, float z)
	{
		if(m_syncLastDateTime)
		{
			m_lastDatetime = DateTime.Now;

			m_syncLastDateTime = false;
		}

		TimeSpan delta = DateTime.Now - m_lastDatetime;
		float deltaTime = (float)delta.TotalMilliseconds / 1000.0f;
		m_lastDatetime = DateTime.Now;

		if(m_timerCountdown < 0.1f)
		{
			m_syncLastDateTime = true;

			if(m_xPoseIterations < m_calibrationDuration)
			{
				if(m_xPoseIterations == 0)
				{
					Debug.Log ("Please orient yourself to match the character, you have " + m_timerCountdownStart + " seconds");
					
					m_timerCountdown = m_timerCountdownStart;
				}

				m_calibrator.Update(CalibrationPose.X, w, x, y, z);
				m_xPoseIterations++;

				if(CalibrationState.xPosePack == null)
				{
					CalibrationState.xPosePack = CalibrationState.SavePose(GameObject.Find ("Havroc Player"));
				}
				
				if(m_xPoseIterations == m_calibrationDuration)
				{
					m_currentPose = CalibrationPose.Y;
					
					m_timerCountdown = m_timerCountdownStart;
					Debug.Log ("Please orient yourself to match the character, you have " + m_timerCountdownStart + " seconds");
				}
			}
			else if(m_yPoseIterations < m_calibrationDuration)
			{
				m_calibrator.Update(CalibrationPose.Y, w, x, y, z);
				m_yPoseIterations++;

				if(CalibrationState.yPosePack == null)
				{
					CalibrationState.yPosePack = CalibrationState.SavePose(GameObject.Find ("Havroc Player"));
				}

				if(m_yPoseIterations == m_calibrationDuration)
				{
					m_currentPose = CalibrationPose.None;
					m_complete = true;
				}
			}

			Debug.Log ("XIterations: " + m_xPoseIterations + " YIterations: " + m_yPoseIterations);
		}
		else
		{
			m_timerCountdown -= deltaTime;
		}
	}

	public int CalibrationDuration
	{
		get
		{
			return m_calibrationDuration;
		}
		set
		{
			m_calibrationDuration = value;
		}
	}

	public float TimerDuration
	{
		get
		{
			return m_timerCountdownStart;
		}
		set
		{
			m_timerCountdownStart = value;
		}
	}

	public CalibrationPose CurrentPose
	{
		get
		{
			return m_currentPose;
		}
	}

	public bool Waiting
	{
		get
		{
			return m_timerCountdown > 0.1f;
		}
	}

	public bool Complete
	{ 
		get
		{
			return m_complete;
		}
	}
}

