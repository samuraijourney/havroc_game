using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class ShoulderTracker : MonoBehaviour 
{
	public enum Arm { Left, Right };

	public Arm arm = Arm.Right;

	public float yaw   = 0.0f; // Rotation about x
	public float pitch = 0.0f; // Rotation about z
	public float roll  = 0.0f; // Rotation about y

	private float m_initialX = 0f;
	private float m_initialY = 0f;
	private float m_initialZ = 0f;

	private float m_lastYaw = 0f;
	private float m_lastPitch = 0f;
	private float m_lastRoll = 0f;

	private HVR_Tracking.ShoulderCallback m_callback;
	private bool m_tracking_on = false;

	private Vector3 m_offset;

	private int m_calibrationDuration = 100; // Num of frames to calibrate with
	private int m_calibrationCurrent = 0;

	// Use this for initialization
	public void Start () 
	{
		m_callback = new HVR_Tracking.ShoulderCallback(OnShoulderEvent);
		m_offset = new Vector3 (0, 0, 0);

		HVR_Tracking.RegisterShoulderCallback(m_callback);

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
			float x = roll - m_offset.x + m_initialX;
			float y = yaw - m_offset.y + m_initialY;
			float z = -1*(pitch - m_offset.z + m_initialZ);

			transform.eulerAngles = new Vector3 (x, y, z);
		}
	}

	public void OnShoulderEvent(float s_yaw, float s_pitch, float s_roll, byte side)
	{
        if((side == 1 && arm == Arm.Right) || (side == 2 && arm == Arm.Left))
		{
			yaw   = s_yaw;
			pitch = s_pitch;
			roll  = s_roll;

			if(!m_tracking_on)
			{
				Debug.Log("Starting shoulder offset calibration");
			}

			m_tracking_on = true;
		}
	}
}
