using UnityEngine;

public class IMUInitializer
{
	private int m_xPoseIterations = 0;
	private int m_yPoseIterations = 0;
	private int m_zPoseIterations = 0;

	private IMUCalibrator m_calibrator;
	private IMUCalibrator.Pose m_currentPose = IMUCalibrator.Pose.X;
	private int m_calibrationDuration = 100;

	private float m_timerCountdownStart = 10.0f;
	private float m_timerCountdown = 0.0f;

	private bool m_complete = false;

	public IMUInitializer(ref IMUCalibrator calibrator)
	{
		m_calibrator = calibrator;
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
				if(m_xPoseIterations == 0)
				{
					Debug.Log ("Please orient yourself to match the character, you have " + m_timerCountdownStart + " seconds");
					
					m_timerCountdown = m_timerCountdownStart;
				}

				m_calibrator.Update(IMUCalibrator.Pose.X, xRotation, yRotation, zRotation);
				m_xPoseIterations++;
				
				if(m_xPoseIterations == m_calibrationDuration)
				{
					m_currentPose = IMUCalibrator.Pose.Y;
					
					m_timerCountdown = m_timerCountdownStart;
					Debug.Log ("Please orient yourself to match the character, you have " + m_timerCountdownStart + " seconds");
				}
			}
			else if(m_yPoseIterations < m_calibrationDuration)
			{
				m_calibrator.Update(IMUCalibrator.Pose.Y, xRotation, yRotation, zRotation);
				m_yPoseIterations++;

				if(m_yPoseIterations == m_calibrationDuration)
				{
					m_currentPose = IMUCalibrator.Pose.Z;

					m_timerCountdown = m_timerCountdownStart;
					Debug.Log ("Please orient yourself to match the character, you have " + m_timerCountdownStart + " seconds");
				}
			}
			else if(m_zPoseIterations < m_calibrationDuration)
			{
				m_calibrator.Update(IMUCalibrator.Pose.Z, xRotation, yRotation, zRotation);
				m_zPoseIterations++;

				if(m_zPoseIterations == m_calibrationDuration)
				{
					m_currentPose = IMUCalibrator.Pose.None;
					m_complete = true;
				}
			}

			Debug.Log ("XIterations: " + m_xPoseIterations + " YIterations: " + m_yPoseIterations + " ZIterations: " + m_zPoseIterations);
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

	public IMUCalibrator.Pose CurrentPose
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

