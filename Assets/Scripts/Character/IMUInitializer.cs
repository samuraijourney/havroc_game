using UnityEngine;

public class IMUInitializer
{
	private int m_xPoseIterations = 0;
	private int m_yPoseIterations = 0;
	private int m_zPoseIterations = 0;

	private IMUCalibrator m_calibrator;
	private int m_calibrationDuration;

	private float m_timerCountdownStart = 10.0f;
	private float m_timerCountdown = 0f;

	private bool m_complete = false;

	public IMUInitializer(ref IMUCalibrator calibrator, int calibrationDuration)
	{
		m_calibrator = calibrator;
		m_calibrationDuration = calibrationDuration;
	}

	public void Reset(bool clearCalibrator)
	{
		m_xPoseIterations = 0;
		m_yPoseIterations = 0;
		m_zPoseIterations = 0;

		m_timerCountdown = 0f;

		m_complete = false;

		if(clearCalibrator)
		{
			m_calibrator.Reset();
		}
	}
	
	public void Update(float deltaTime, float xRotation, float yRotation, float zRotation)
	{
		if(m_timerCountdown < 0.1f)
		{
			if(m_xPoseIterations < m_calibrationDuration)
			{
				m_calibrator.Update(IMUCalibrator.Pose.X, xRotation, yRotation, zRotation);
				m_xPoseIterations++;

				if(m_xPoseIterations == m_calibrationDuration)
				{
					m_timerCountdown = m_timerCountdownStart;
					Debug.Log ("Please orient yourself along the Y axis");
				}
			}
			else if(m_yPoseIterations < m_calibrationDuration)
			{
				m_calibrator.Update(IMUCalibrator.Pose.Y, xRotation, yRotation, zRotation);
				m_yPoseIterations++;

				if(m_yPoseIterations == m_calibrationDuration)
				{
					m_timerCountdown = m_timerCountdownStart;
					Debug.Log ("Please orient yourself along the Z axis");
				}
			}
			else if(m_zPoseIterations < m_calibrationDuration)
			{
				m_calibrator.Update(IMUCalibrator.Pose.Z, xRotation, yRotation, zRotation);
				m_zPoseIterations++;

				if(m_zPoseIterations == m_calibrationDuration)
				{
					m_complete = true;
				}
			}
		}
		else
		{
			m_timerCountdown -= deltaTime;
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

