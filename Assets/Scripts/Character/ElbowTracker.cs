using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class ElbowTracker : MonoBehaviour, ICalibrationStateMember 
{
	public Arm arm = Arm.Right;
	
	public float yaw   = 0.0f; // Rotation about x
	public float pitch = 0.0f; // Rotation about z
	public float roll  = 0.0f; // Rotation about y
	
	public Vector3 xPoseGlobalRotation;
	public Vector3 yPoseGlobalRotation;
	public Vector3 zPoseGlobalRotation;
	
	private HVR_Tracking.ElbowCallback m_callback;
	
	private IMUCalibrator m_calibrator;
	private CalibrationPose m_currentPose;
	
	private Vector3 m_lastRotation;
	
	private bool m_trackingOn = false;
	private float m_speed = 1.0f;
	
	// Use this for initialization
	public void Start () 
	{	
		m_callback = new HVR_Tracking.ElbowCallback(OnElbowEvent);
	}
	
	public void LateUpdate () 
	{
		if(m_trackingOn)
		{
			transform.eulerAngles = m_calibrator.ComputeRotation(roll, yaw, pitch);
		}
		else
		{
			switch(m_currentPose)
			{
			case CalibrationPose.X:
			{
				m_lastRotation = Vector3.Lerp(m_lastRotation, xPoseGlobalRotation, Time.deltaTime * m_speed);
				transform.eulerAngles = m_lastRotation;
				break;
			}
			case CalibrationPose.Y:
			{
				m_lastRotation = Vector3.Lerp(m_lastRotation, yPoseGlobalRotation, Time.deltaTime * m_speed);
				transform.eulerAngles = m_lastRotation;
				break;
			}
			case CalibrationPose.Z:
			{
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
		}
	}
	
	public void OnStateCalibrationPoseUpdate(CalibrationPose pose)
	{
		m_currentPose = pose;
	}
	
	public void OnStateCalibrationDone(IMUCalibrator calibrator)
	{
		m_calibrator = calibrator;
		
		HVR_Tracking.RegisterElbowCallback(m_callback);
		
		m_trackingOn = true;
	}
	
	public void OnStateBaseStart(GameState state)
	{
		m_lastRotation = xPoseGlobalRotation;
		
		HVR_Tracking.UnregisterElbowCallback(m_callback);
		
		m_trackingOn = false;
	}
	
	public void OnStateBaseEnd(GameState state)
	{
		
	}
	
	public Vector3 GlobalXPosition
	{ 
		get
		{
			return xPoseGlobalRotation;
		}
	}
	
	public Vector3 GlobalYPosition
	{ 
		get
		{
			return yPoseGlobalRotation;
		}
	}
	
	public Vector3 GlobalZPosition
	{ 
		get
		{
			return zPoseGlobalRotation;
		}
	}
	
	public Arm Arm
	{ 
		get
		{
			return arm;
		}
	}
	
	public Joint Joint
	{ 
		get
		{
			return Joint.Elbow;
		}
	}
}
