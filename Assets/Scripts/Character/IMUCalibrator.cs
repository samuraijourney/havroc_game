using UnityEngine;

public class IMUCalibrator
{
	public enum Pose { X, Y, Z };

	private Vector3 m_poseScales;
	private Vector3 m_poseIterations;
	
	private Vector3 m_xPoseCharacterRotation;
	private Vector3 m_yPoseCharacterRotation;
	private Vector3 m_zPoseCharacterRotation;
	
	private Vector3 m_xPosePlayerRotationAccum;
	private Vector3 m_yPosePlayerRotationAccum;
	private Vector3 m_zPosePlayerRotationAccum;
	
	private Vector3 m_xPosePlayerRotationAvg;
	private Vector3 m_yPosePlayerRotationAvg;
	private Vector3 m_zPosePlayerRotationAvg;
	
	public IMUCalibrator(Vector3 xPoseCharacterRotation,
	                     Vector3 yPoseCharacterRotation,
	                     Vector3 zPoseCharacterRotation)
	{
		m_xPoseCharacterRotation = xPoseCharacterRotation;
		m_yPoseCharacterRotation = yPoseCharacterRotation;
		m_zPoseCharacterRotation = zPoseCharacterRotation;
		
		m_xPosePlayerRotationAccum = new Vector3(0,0,0);
		m_yPosePlayerRotationAccum = new Vector3(0,0,0);
		m_zPosePlayerRotationAccum = new Vector3(0,0,0);
		
		m_xPosePlayerRotationAvg = new Vector3(0,0,0);
		m_yPosePlayerRotationAvg = new Vector3(0,0,0);
		m_zPosePlayerRotationAvg = new Vector3(0,0,0);

		m_poseScales = new Vector3(0,0,0);
		m_poseIterations = new Vector3(0,0,0);
	}
	
	public void Update(Pose pose, float xRotation, float yRotation, float zRotation)
	{
		switch(pose)
		{
			case Pose.X:
			{
				m_xPosePlayerRotationAccum.x += xRotation;
				m_xPosePlayerRotationAccum.y += yRotation;
				m_xPosePlayerRotationAccum.z += zRotation;
				
				m_poseIterations.x++;
				
				m_xPosePlayerRotationAvg.x = m_xPosePlayerRotationAccum.x / m_poseIterations.x;
				m_xPosePlayerRotationAvg.y = m_xPosePlayerRotationAccum.y / m_poseIterations.x;
				m_xPosePlayerRotationAvg.z = m_xPosePlayerRotationAccum.z / m_poseIterations.x;
				
				break;
			}
			case Pose.Y:
			{
				m_yPosePlayerRotationAccum.x += xRotation;
				m_yPosePlayerRotationAccum.y += yRotation;
				m_yPosePlayerRotationAccum.z += zRotation;
				
				m_poseIterations.y++;
				
				m_yPosePlayerRotationAvg.x = m_yPosePlayerRotationAccum.x / m_poseIterations.y;
				m_yPosePlayerRotationAvg.y = m_yPosePlayerRotationAccum.y / m_poseIterations.y;
				m_yPosePlayerRotationAvg.z = m_yPosePlayerRotationAccum.z / m_poseIterations.y;
				
				break;
			}
			case Pose.Z:
			{
				m_zPosePlayerRotationAccum.x += xRotation;
				m_zPosePlayerRotationAccum.y += yRotation;
				m_zPosePlayerRotationAccum.z += zRotation;
				
				m_poseIterations.z++;
				
				m_zPosePlayerRotationAvg.x = m_zPosePlayerRotationAccum.x / m_poseIterations.z;
				m_zPosePlayerRotationAvg.y = m_zPosePlayerRotationAccum.y / m_poseIterations.z;
				m_zPosePlayerRotationAvg.z = m_zPosePlayerRotationAccum.z / m_poseIterations.z;
				
				break;
			}
		}

		m_poseScales.x = (m_xPoseCharacterRotation.x - m_zPoseCharacterRotation.x) / (m_xPosePlayerRotationAvg.x - m_zPosePlayerRotationAvg.x);
		m_poseScales.y = (m_yPoseCharacterRotation.y - m_zPoseCharacterRotation.y) / (m_yPosePlayerRotationAvg.y - m_zPosePlayerRotationAvg.y);
		m_poseScales.z = (m_xPoseCharacterRotation.z - m_yPoseCharacterRotation.z) / (m_xPosePlayerRotationAvg.z - m_yPosePlayerRotationAvg.z);
	}

	public Vector3 ComputeRotation(float xRotation, float yRotation, float zRotation)
	{
		return new Vector3(xRotation * m_poseScales.x, yRotation * m_poseScales.y, zRotation * m_poseScales.z);
	}

	public void Reset()
	{
		m_xPosePlayerRotationAccum = new Vector3(0,0,0);
		m_yPosePlayerRotationAccum = new Vector3(0,0,0);
		m_zPosePlayerRotationAccum = new Vector3(0,0,0);
		
		m_xPosePlayerRotationAvg = new Vector3(0,0,0);
		m_yPosePlayerRotationAvg = new Vector3(0,0,0);
		m_zPosePlayerRotationAvg = new Vector3(0,0,0);
		
		m_poseIterations = new Vector3(0,0,0);
	}

	public Vector3 PlayerPoseScales
	{
		get
		{
			return m_poseScales;
		}
	}

	public Vector3 PlayerXPose
	{
		get
		{
			return m_xPosePlayerRotationAvg;
		}
	}

	public Vector3 PlayerYPose
	{
		get
		{
			return m_yPosePlayerRotationAvg;
		}
	}

	public Vector3 PlayerZPose
	{
		get
		{
			return m_zPosePlayerRotationAvg;
		}
	}

	public int GetIterationsOfPose(Pose pose)
	{
		switch(pose)
		{
			case Pose.X:
			{
				return (int)m_poseIterations.x;
			}
			case Pose.Y:
			{
				return (int)m_poseIterations.y;
			}
			case Pose.Z:
			{
				return (int)m_poseIterations.z;
			}
		}

		return -1;
	}
}

