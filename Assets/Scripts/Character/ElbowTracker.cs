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
	
	public float timerDuration = 10.0f;
	public int calibrationDuration = 100;
	
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

	private Vector3 m_lastRotation;
	
	private bool m_trackingOn = false;
	private float m_speed = 1.0f;

	private DateTime m_lastDatetime;

	// Use this for initialization
	public void Start () 
	{	
		m_calibrator = new IMUCalibrator(xPoseGlobalRotation, yPoseGlobalRotation, zPoseGlobalRotation);
		
		m_initializer = new IMUInitializer(ref m_calibrator);
		
		m_callback = new HVR_Tracking.ElbowCallback(OnElbowEvent);
		HVR_Tracking.RegisterElbowCallback(m_callback);

		m_lastRotation = xPoseGlobalRotation;
	}
	
	public void LateUpdate () 
	{
		xPosePlayerRotation = m_calibrator.PlayerXPose;
		yPosePlayerRotation = m_calibrator.PlayerYPose;
		zPosePlayerRotation = m_calibrator.PlayerZPose;
		
		posePlayerScales = m_calibrator.PlayerPoseScales;

		m_initializer.TimerDuration = timerDuration;
		m_initializer.CalibrationDuration = calibrationDuration;

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
				//m_initializer.Update(m_deltaTime, 0, 0, -180/10);
				m_lastRotation = Vector3.Lerp(m_lastRotation, xPoseGlobalRotation, Time.deltaTime * m_speed);
				transform.eulerAngles = m_lastRotation;
				break;
			}
			case IMUCalibrator.Pose.Y:
			{
				//m_initializer.Update(m_deltaTime, 0, 0, -90/10);
				m_lastRotation = Vector3.Lerp(m_lastRotation, yPoseGlobalRotation, Time.deltaTime * m_speed);
				transform.eulerAngles = m_lastRotation;
				break;
			}
			case IMUCalibrator.Pose.Z:
			{
				//m_initializer.Update(m_deltaTime, 90/10, 90/10, -90/10);
				m_lastRotation = Vector3.Lerp(m_lastRotation, zPoseGlobalRotation, Time.deltaTime * m_speed);
				transform.eulerAngles = m_lastRotation;
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

				m_lastDatetime = DateTime.Now;
			}
			
			m_trackingOn = true;
			
			if(!m_initializer.Complete)
			{
				TimeSpan delta = DateTime.Now - m_lastDatetime;
				m_initializer.Update(((float)delta.TotalMilliseconds) / 1000.0f, s_roll, s_yaw, s_pitch);
				m_lastDatetime = DateTime.Now;
			}
		}
	}
}
