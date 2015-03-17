using UnityEngine;

public class IMUCalibrator
{
	public enum Pose { X, Y, Z };
	
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
	}
	
	public Vector3 ComputeRotation(float xRotation, float yRotation, float zRotation)
	{
		float xPoseDiffX = Mathf.Abs(m_xPosePlayerRotationAvg.x - xRotation);
		float xPoseDiffY = Mathf.Abs(m_xPosePlayerRotationAvg.y - yRotation);
		float xPoseDiffZ = Mathf.Abs(m_xPosePlayerRotationAvg.z - zRotation);
		float yPoseDiffX = Mathf.Abs(m_yPosePlayerRotationAvg.x - xRotation);
		float yPoseDiffY = Mathf.Abs(m_yPosePlayerRotationAvg.y - yRotation);
		float yPoseDiffZ = Mathf.Abs(m_yPosePlayerRotationAvg.z - zRotation);
		float zPoseDiffX = Mathf.Abs(m_zPosePlayerRotationAvg.x - xRotation);
		float zPoseDiffY = Mathf.Abs(m_zPosePlayerRotationAvg.y - yRotation);
		float zPoseDiffZ = Mathf.Abs(m_zPosePlayerRotationAvg.z - zRotation);
		
		float xDiffTotal = xPoseDiffX + yPoseDiffX + zPoseDiffX;
		float yDiffTotal = xPoseDiffY + yPoseDiffY + zPoseDiffY;
		float zDiffTotal = xPoseDiffZ + yPoseDiffZ + zPoseDiffZ;

		float xPoseWeightX = (xDiffTotal - xPoseDiffX) / xDiffTotal;
		float xPoseWeightY = (xDiffTotal - xPoseDiffY) / xDiffTotal;
		float xPoseWeightZ = (xDiffTotal - xPoseDiffZ) / xDiffTotal;
		float yPoseWeightX = (yDiffTotal - yPoseDiffX) / yDiffTotal;
		float yPoseWeightY = (yDiffTotal - yPoseDiffY) / yDiffTotal;
		float yPoseWeightZ = (yDiffTotal - yPoseDiffZ) / yDiffTotal;
		float zPoseWeightX = (zDiffTotal - zPoseDiffX) / zDiffTotal;
		float zPoseWeightY = (zDiffTotal - zPoseDiffY) / zDiffTotal;
		float zPoseWeightZ = (zDiffTotal - zPoseDiffZ) / zDiffTotal;

		
		float weightedX = (m_xPoseCharacterRotation.x * xPoseWeightX +
						   m_yPoseCharacterRotation.x * yPoseWeightX +
						   m_zPoseCharacterRotation.x * zPoseWeightX) /
						  (xPoseWeightX + yPoseWeightX + zPoseWeightX);
		float weightedY = (m_xPoseCharacterRotation.y * xPoseWeightY +
		                   m_yPoseCharacterRotation.y * yPoseWeightY +
		                   m_zPoseCharacterRotation.y * zPoseWeightY) /
						  (xPoseWeightY + yPoseWeightY + zPoseWeightY);
		float weightedZ = (m_xPoseCharacterRotation.z * xPoseWeightZ +
		                   m_yPoseCharacterRotation.z * yPoseWeightZ +
		                   m_zPoseCharacterRotation.z * zPoseWeightZ) /
						  (xPoseWeightZ + yPoseWeightZ + zPoseWeightZ);
		
		return new Vector3(weightedX, weightedY, weightedZ);
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

