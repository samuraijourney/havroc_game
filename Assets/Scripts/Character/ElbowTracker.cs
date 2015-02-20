using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class ElbowTracker : MonoBehaviour 
{
	public enum Arm { Left, Right };
	
	public Arm arm = Arm.Right;
	
	public float yaw   = 0.0f; // Rotation about y
	public float pitch = 0.0f; // Rotation about z
	public float roll  = 0.0f; // Rotation about x
	
	private HVR_Tracking.ElbowCallback m_callback;
	
	// Use this for initialization
	public void Start () 
	{
		m_callback = new HVR_Tracking.ElbowCallback (OnElbowEvent);
		
		HVR_Tracking.RegisterElbowCallback(m_callback);

		yaw   = transform.eulerAngles.x;
		pitch = transform.eulerAngles.y;
		roll  = transform.eulerAngles.z;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		transform.eulerAngles = new Vector3 (roll, yaw, pitch);
	}
	
	public void OnElbowEvent(float e_yaw, float e_pitch, float e_roll, byte side)
	{
		if((side == 1 && arm == Arm.Right) || (side == 2 && arm == Arm.Left))
		{
			yaw   = e_yaw;
			pitch = e_pitch;
			roll  = e_roll;
		}
	}
}
