using UnityEngine;

public class IMUCalibrator
{
	private Vector3 m_poseScales;
	private Vector3 m_poseIterations;

	private Vector3 m_poseSign;
	
	private Vector4 m_xPoseCharacterRotation;
	private Vector4 m_yPoseCharacterRotation;
	private Vector4 m_zPoseCharacterRotation;
	
	private Vector4 m_xPosePlayerRotationAccum;
	private Vector4 m_yPosePlayerRotationAccum;
	private Vector4 m_zPosePlayerRotationAccum;
	
	private Vector4 m_xPosePlayerRotationAvg;
	private Vector4 m_yPosePlayerRotationAvg;
	private Vector4 m_zPosePlayerRotationAvg;
	
	public IMUCalibrator(Vector3 xPoseCharacterRotation,
	                     Vector3 yPoseCharacterRotation,
	                     Vector3 zPoseCharacterRotation)
	{
		m_xPoseCharacterRotation = xPoseCharacterRotation;
		m_yPoseCharacterRotation = yPoseCharacterRotation;
		m_zPoseCharacterRotation = zPoseCharacterRotation;
		
		m_xPosePlayerRotationAccum = new Vector4(0,0,0);
		m_yPosePlayerRotationAccum = new Vector4(0,0,0);
		m_zPosePlayerRotationAccum = new Vector4(0,0,0);
		
		m_xPosePlayerRotationAvg = new Vector4(0,0,0);
		m_yPosePlayerRotationAvg = new Vector4(0,0,0);
		m_zPosePlayerRotationAvg = new Vector4(0,0,0);

		m_poseScales = new Vector3(0,0,0);
		m_poseIterations = new Vector3(0,0,0);

		m_poseSign = new Vector3(1,1,1);
	}
	
	public void Update(CalibrationPose pose, float w, float x, float y, float z)
	{
		switch(pose)
		{
			case CalibrationPose.X:
			{
				m_xPosePlayerRotationAccum.x += x;
				m_xPosePlayerRotationAccum.y += y;
				m_xPosePlayerRotationAccum.z += z;
				m_xPosePlayerRotationAccum.w += w;
				
				m_poseIterations.x++;
				
				m_xPosePlayerRotationAvg.x = m_xPosePlayerRotationAccum.x / m_poseIterations.x;
				m_xPosePlayerRotationAvg.y = m_xPosePlayerRotationAccum.y / m_poseIterations.x;
				m_xPosePlayerRotationAvg.z = m_xPosePlayerRotationAccum.z / m_poseIterations.x;
				m_xPosePlayerRotationAvg.w = m_xPosePlayerRotationAccum.w / m_poseIterations.x;
				
				break;
			}
			case CalibrationPose.Y:
			{
				m_yPosePlayerRotationAccum.x += x;
				m_yPosePlayerRotationAccum.y += y;
				m_yPosePlayerRotationAccum.z += z;
				m_yPosePlayerRotationAccum.w += w;
				
				m_poseIterations.y++;
				
				m_yPosePlayerRotationAvg.x = m_yPosePlayerRotationAccum.x / m_poseIterations.y;
				m_yPosePlayerRotationAvg.y = m_yPosePlayerRotationAccum.y / m_poseIterations.y;
				m_yPosePlayerRotationAvg.z = m_yPosePlayerRotationAccum.z / m_poseIterations.y;
				m_yPosePlayerRotationAvg.w = m_yPosePlayerRotationAccum.w / m_poseIterations.y;
				
				break;
			}
			case CalibrationPose.Z:
			{
				m_zPosePlayerRotationAccum.x += x;
				m_zPosePlayerRotationAccum.y += y;
				m_zPosePlayerRotationAccum.z += z;
				m_zPosePlayerRotationAccum.w += w;
				
				m_poseIterations.z++;
				
				m_zPosePlayerRotationAvg.x = m_zPosePlayerRotationAccum.x / m_poseIterations.z;
				m_zPosePlayerRotationAvg.y = m_zPosePlayerRotationAccum.y / m_poseIterations.z;
				m_zPosePlayerRotationAvg.z = m_zPosePlayerRotationAccum.z / m_poseIterations.z;
				m_zPosePlayerRotationAvg.w = m_zPosePlayerRotationAccum.w / m_poseIterations.z;
				
				break;
			}
		}

		float rollAngleCharacterRange = Mathf.Acos(Vector3.Dot (m_yPoseCharacterRotation, m_zPoseCharacterRotation) / (m_yPoseCharacterRotation.magnitude * m_zPoseCharacterRotation.magnitude));
		float yawAngleCharacterRange = Mathf.Acos(Vector3.Dot (m_yPoseCharacterRotation, m_zPoseCharacterRotation) / (m_yPoseCharacterRotation.magnitude * m_zPoseCharacterRotation.magnitude));
		float pitchAngleCharacterRange = Mathf.Acos(Vector3.Dot (m_xPoseCharacterRotation, m_yPoseCharacterRotation) / (m_xPoseCharacterRotation.magnitude * m_yPoseCharacterRotation.magnitude));

		float rollAnglePlayerRange = Mathf.Acos(Vector3.Dot (m_yPosePlayerRotationAvg, m_zPosePlayerRotationAvg) / (m_yPosePlayerRotationAvg.magnitude * m_zPosePlayerRotationAvg.magnitude));
		float yawAnglePlayerRange = Mathf.Acos(Vector3.Dot (m_yPosePlayerRotationAvg, m_zPosePlayerRotationAvg) / (m_yPosePlayerRotationAvg.magnitude * m_zPosePlayerRotationAvg.magnitude));
		float pitchAnglePlayerRange = Mathf.Acos(Vector3.Dot (m_xPosePlayerRotationAvg, m_yPosePlayerRotationAvg) / (m_xPosePlayerRotationAvg.magnitude * m_yPosePlayerRotationAvg.magnitude));
		
		m_poseScales.x = (m_yPoseCharacterRotation.x - m_zPoseCharacterRotation.x) / (m_yPosePlayerRotationAvg.x - m_zPosePlayerRotationAvg.x);
		m_poseScales.y = (m_yPoseCharacterRotation.y - m_zPoseCharacterRotation.y) / (m_yPosePlayerRotationAvg.y - m_zPosePlayerRotationAvg.y);
		m_poseScales.z = (m_yPoseCharacterRotation.z - m_xPoseCharacterRotation.z) / (m_yPosePlayerRotationAvg.z - m_xPosePlayerRotationAvg.z);
	}

	public Vector3 ComputeRotation(float w, float x, float y, float z, Vector3 rotation, Transform transform, Joint joint)
	{
		//Vector3 delta;
		//Vector3 rawRotation;

		//if(joint == Joint.Shoulder)
		//{
			//rawRotation = new Vector3 (roll, yaw, pitch);
			//delta = rawRotation - m_xPosePlayerRotationAvg;
		//}
		//else
		//{
		//	rawRotation = new Vector3 (roll, pitch, yaw);
		//	delta = rawRotation - new Vector3(m_xPosePlayerRotationAvg.x, m_xPosePlayerRotationAvg.z, m_xPosePlayerRotationAvg.y);
		//}

		//transform.eulerAngles = m_xPoseCharacterRotation;
		//transform.Rotate (new Vector3(Sign.x * delta.x, Sign.y * delta.y, Sign.z * delta.z));
		transform.rotation = new Quaternion (Sign.x * x, Sign.y * y, Sign.z * z, w);// * Quaternion.Inverse(new Quaternion(m_xPosePlayerRotationAvg.x, 
		                                                                           // //		 m_xPosePlayerRotationAvg.z, 
		                                                                           // 		 m_xPosePlayerRotationAvg.y, 
		                                                                           // 		 m_xPosePlayerRotationAvg.w));  // Subtract quaternions
	//	transform.rotation = Quaternion.Inverse (transform.rotation);
	//	transform.Rotate (m_xPoseCharacterRotation);
		transform.Rotate (new Vector3(Sign.x * rotation.x, 0, 0));
		transform.Rotate (new Vector3(0, Sign.y * rotation.y, 0));
		transform.Rotate (new Vector3(0, 0, Sign.z * rotation.z));
		
		return new Vector3(0,0,0);
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

	public Vector3 Sign 
	{
		get
		{
			return m_poseSign;
		}
		set
		{
			m_poseSign = value;
		}
	}
	public Vector3 Rotation { get; set; }

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

	public int GetIterationsOfPose(CalibrationPose pose)
	{
		switch(pose)
		{
			case CalibrationPose.X:
			{
				return (int)m_poseIterations.x;
			}
			case CalibrationPose.Y:
			{
				return (int)m_poseIterations.y;
			}
			case CalibrationPose.Z:
			{
				return (int)m_poseIterations.z;
			}
		}

		return -1;
	}
}

