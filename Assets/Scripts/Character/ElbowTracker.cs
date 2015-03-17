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

	public float xAddition = 0.0f;
	public float yAddition = 0.0f;
	public float zAddition = 0.0f;

	public int xSign = 1;
	public int ySign = 1;
	public int zSign = 1;
	
	private float m_initialX = 0f;
	private float m_initialY = 0f;
	private float m_initialZ = 0f;
	
	private float m_lastYaw = 0f;
	private float m_lastPitch = 0f;
	private float m_lastRoll = 0f;
	
	private HVR_Tracking.ElbowCallback m_callback;
	private bool m_tracking_on = false;
	
	private Vector3 m_offset;
	
	private int m_calibrationDuration = 100; // Num of frames to calibrate with
	private int m_calibrationCurrent = 0;
	
	// Use this for initialization
	public void Start () 
	{
		m_callback = new HVR_Tracking.ElbowCallback(OnElbowEvent);
		m_offset = new Vector3 (0, 0, 0);
		
		HVR_Tracking.RegisterElbowCallback(m_callback);
		
		m_initialX = transform.eulerAngles.x;
		m_initialY = transform.eulerAngles.y;
		m_initialZ = transform.eulerAngles.z;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		if(!m_tracking_on)
		{
			transform.eulerAngles = new Vector3 (m_initialX, m_initialY, m_initialZ);
		}
		else if(m_calibrationCurrent < m_calibrationDuration)
		{
			m_offset.x += roll;
			m_offset.y += yaw;
			m_offset.z += pitch;
			
			m_calibrationCurrent++;
			
			if(m_calibrationCurrent == m_calibrationDuration)
			{
				m_offset.x /= m_calibrationDuration;
				m_offset.y /= m_calibrationDuration;
				m_offset.z /= m_calibrationDuration;
				
				Debug.Log ("Offset is complete, x:" + m_offset.x + " y:" + m_offset.y + " z:" + m_offset.z);
			}
		}
		else
		{
			float x = xSign*(roll - m_offset.x + m_initialX + xAddition);
			float y = ySign*(yaw - m_offset.y + m_initialY + yAddition);
			float z = zSign*(pitch - m_offset.z + m_initialZ + zAddition);
			
			transform.eulerAngles = new Vector3 (x, y, z);
		}
	}
	
	public void OnElbowEvent(float s_yaw, float s_pitch, float s_roll, byte side)
	{
		if((side == 1 && arm == Arm.Right) || (side == 2 && arm == Arm.Left))
		{
			yaw   = s_yaw;
			pitch = s_pitch;
			roll  = s_roll;
			
			if(!m_tracking_on)
			{
				Debug.Log("Starting elbow offset calibration");
			}
			
			m_tracking_on = true;
		}
	}
}
