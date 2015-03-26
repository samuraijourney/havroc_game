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

	public float w = 0.0f;
	public float x = 0.0f;
	public float y = 0.0f;
	public float z = 0.0f;

	public Vector3 rotationSign = new Vector3(-1,-1,-1);
	public Vector3 rotation;
	
	public Vector3 xPoseGlobalRotation;
	public Vector3 yPoseGlobalRotation;
	public Vector3 zPoseGlobalRotation;
	
	private HVR_Tracking.ElbowCallback m_callback;
	
	private IMUCalibrator m_calibrator;
	private CalibrationPose m_currentPose = CalibrationPose.None;
	
	private Vector3 m_lastRotation;
	
	private bool m_trackingOn = false;
	private bool m_calibrating = false;

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
			if(m_calibrator != null)
			{
				m_calibrator.Sign = rotationSign;
				
				m_calibrator.ComputeRotation(w, x, y, z, rotation, transform);
			}
		}
		else if(m_calibrating)
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
	
	public void OnElbowEvent(float s_w, float s_x, float s_y, float s_z, byte side)
	{
		if((side == 1 && arm == Arm.Right) || (side == 2 && arm == Arm.Left))
		{
			w = s_w;
			x = s_x;
			y = s_y;
			z = s_z;
			
			Vector3 euler = (new Quaternion(x,y,z,w)).eulerAngles;
			
			yaw   = euler.y;
			pitch = euler.z;
			roll  = euler.x;
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
		m_calibrating = false;
	}
	
	public void OnStateBaseStart(GameState state)
	{
		if(state == GameState.Calibration)
		{
			m_lastRotation = xPoseGlobalRotation;
			
			HVR_Tracking.UnregisterElbowCallback(m_callback);
			
			m_trackingOn = false;
			m_calibrating = true;
		}
	}
	
	public void OnStateBaseEnd(GameState state)
	{
		
	}

	public IMUCalibrator Calibrator
	{
		get
		{
			return m_calibrator;
		}
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
