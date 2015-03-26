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

	private Quaternion m_xPosePlayerRotationQuat;
	private Quaternion m_yPosePlayerRotationQuat;
	private Quaternion m_zPosePlayerRotationQuat;

	private bool m_globalQuaternionsInitialized = false;
	
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
	}

	private Quaternion PreprocessIMUQuaternionData (Vector4 quatParams)
	{
		return PreprocessIMUQuaternionData (quatParams.w, quatParams.x, quatParams.y, quatParams.z);
	}

	private Quaternion PreprocessIMUQuaternionData (float w, float x, float y, float z)
	{
		GameObject gameObject = new GameObject ();

		Transform newTransform = gameObject.transform;

		newTransform.eulerAngles = new Vector3 (0, 0, 0);
	
		newTransform.rotation = new Quaternion (x, y, z, w);// * 
								//new Quaternion (-0.707107f, 0, 0, 0.707107f) * 
								//new Quaternion (0, 0.707107f, 0, 0.707107f) *
								//new Quaternion (-0.707107f, 0, 0, 0.707107f);
		
		Quaternion newQuaternion = new Quaternion (-newTransform.rotation.y, newTransform.rotation.z, -newTransform.rotation.x, newTransform.rotation.w);

		GameObject.Destroy (gameObject);

		return newQuaternion;
	}

	public void ComputeRotation(float w, float x, float y, float z, Vector3 rotation, Transform transform)
	{
		if(!m_globalQuaternionsInitialized)
		{
			m_xPosePlayerRotationQuat = PreprocessIMUQuaternionData(m_xPosePlayerRotationAvg);
			m_yPosePlayerRotationQuat = PreprocessIMUQuaternionData(m_yPosePlayerRotationAvg);
			m_zPosePlayerRotationQuat = PreprocessIMUQuaternionData(m_zPosePlayerRotationAvg);

			m_globalQuaternionsInitialized = true;
		}

		transform.rotation = PreprocessIMUQuaternionData (w, x, y, z) * 
							 Quaternion.Inverse (m_yPosePlayerRotationQuat) *
							 new Quaternion (Mathf.Cos (Mathf.PI / 4), Mathf.Sin (Mathf.PI / 4), 0, 0);
		    
		transform.Rotate (rotation);
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

