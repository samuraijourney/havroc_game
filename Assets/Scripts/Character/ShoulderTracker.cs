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

	private HVR_Tracking.ShoulderCallback m_callback;

	// Use this for initialization
	public void Start () 
	{
		m_callback = new HVR_Tracking.ShoulderCallback (OnShoulderEvent);

		HVR_Tracking.RegisterShoulderCallback(m_callback);

		yaw   = transform.eulerAngles.x;
		pitch = transform.eulerAngles.y;
		roll  = transform.eulerAngles.z;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		transform.eulerAngles = new Vector3 (roll, yaw, pitch);
	}

	public void OnShoulderEvent(float s_yaw, float s_pitch, float s_roll, byte side)
	{
        if((side == 1 && arm == Arm.Right) || (side == 2 && arm == Arm.Left))
		{
			yaw   = s_yaw;
			pitch = s_pitch;
			roll  = s_roll;
		}
	}
}
