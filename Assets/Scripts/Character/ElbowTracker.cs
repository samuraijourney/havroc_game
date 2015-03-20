using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class ElbowTracker : MonoBehaviour 
{
	public enum Arm { Left, Right };
	
	public Arm arm = Arm.Right;
	
	public float yaw   = 0.0f; // Rotation about x
	public float pitch = 0.0f; // Rotation about z
	public float roll  = 0.0f; // Rotation about y
	
	public Vector3 xPoseGlobalRotation;
	public Vector3 yPoseGlobalRotation;
	public Vector3 zPoseGlobalRotation;
	
	public Vector3 xPosePlayerRotation;
	public Vector3 yPosePlayerRotation;
	public Vector3 zPosePlayerRotation;
	
	public Vector3 posePlayerScales;
	
	private IMUCalibrator m_calibrator;
	private IMUInitializer m_initializer;
	
	private HVR_Tracking.ElbowCallback m_callback;
	
	private bool m_trackingOn = false;
	private float m_deltaTime = 0.0f;
	
	private int m_calibrationDuration = 100;
	
	// Use this for initialization
	public void Start () 
	{	
		m_calibrator = new IMUCalibrator(xPoseGlobalRotation, yPoseGlobalRotation, zPoseGlobalRotation);
		
		m_initializer = new IMUInitializer(ref m_calibrator, m_calibrationDuration);
		
		m_callback = new HVR_Tracking.ElbowCallback(OnElbowEvent);
		HVR_Tracking.RegisterElbowCallback(m_callback);
	}
	
	public void Update () 
	{
		m_deltaTime = Time.deltaTime;
		
		xPosePlayerRotation = m_calibrator.PlayerXPose;
		yPosePlayerRotation = m_calibrator.PlayerYPose;
		zPosePlayerRotation = m_calibrator.PlayerZPose;
		
		posePlayerScales = m_calibrator.PlayerPoseScales;
		
		if(m_initializer.Complete)
		{
			transform.eulerAngles = m_calibrator.ComputeRotation(roll, yaw, pitch);
		}
		else
		{
			switch(m_initializer.CurrentPose)
			{
			case IMUCalibrator.Pose.X:
			{
				//m_initializer.Update(m_deltaTime, 0, 0, 180/10);
				transform.eulerAngles = xPoseGlobalRotation;
				break;
			}
			case IMUCalibrator.Pose.Y:
			{
				//m_initializer.Update(m_deltaTime, 0, 0, -90/10);
				transform.eulerAngles = yPoseGlobalRotation;
				break;
			}
			case IMUCalibrator.Pose.Z:
			{
				//m_initializer.Update(m_deltaTime, 90/10, 90/10, -90/10);
				transform.eulerAngles = zPoseGlobalRotation;
				break;
			}
			default:
			{
				break;
			}
			}
		}
	}
	
	public void OnElbowEvent(float s_yaw, float s_pitch, float s_roll, byte side)
	{
		if((side == 1 && arm == Arm.Right) || (side == 2 && arm == Arm.Left))
		{
			yaw   = s_yaw;
			pitch = s_pitch;
			roll  = s_roll;
			
			if(!m_trackingOn)
			{
				Debug.Log("Starting elbow offset calibration");
			}
			
			m_trackingOn = true;
			
			if(!m_initializer.Complete)
			{
				m_initializer.Update(m_deltaTime, s_roll, s_yaw, s_pitch);
			}
		}
	}
}
