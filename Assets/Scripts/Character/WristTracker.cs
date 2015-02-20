using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class WristTracker : MonoBehaviour 
{
	public enum Arm { Left, Right };
	
	public Arm arm = Arm.Right;
	
	public float yaw   = 0.0f; // Rotation about x
	public float pitch = 0.0f; // Rotation about z
	public float roll  = 0.0f; // Rotation about y
	
	private HVR_Tracking.WristCallback m_callback;
	
	// Use this for initialization
	public void Start () 
	{
		m_callback = new HVR_Tracking.WristCallback (OnWristEvent);
		
		HVR_Tracking.RegisterWristCallback(m_callback);

		yaw   = transform.eulerAngles.x;
		pitch = transform.eulerAngles.y;
		roll  = transform.eulerAngles.z;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		transform.eulerAngles = new Vector3 (roll, yaw, pitch);
	}
	
	public void OnWristEvent(float w_yaw, float w_pitch, float w_roll, byte side)
	{
		if((side == 1 && arm == Arm.Right) || (side == 2 && arm == Arm.Left))
		{
			yaw   = w_yaw;
			pitch = w_pitch;
			roll  = w_roll;
		}
	}
}
