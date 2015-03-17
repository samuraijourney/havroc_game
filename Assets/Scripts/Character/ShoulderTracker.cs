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

	private IMUCalibrator m_calibrator;
	private IMUInitializer m_initializer;

	private Vector3 m_xPoseGlobalRotation;
	private Vector3 m_yPoseGlobalRotation;
	private Vector3 m_zPoseGlobalRotation;

	private HVR_Tracking.ShoulderCallback m_callback;

	private bool m_trackingOn = false;
	private float m_deltaTime = 0.0f;

	// Use this for initialization
	public void Start () 
	{
		float xPoseLocalRotationX = 0.0f;
		float xPoseLocalRotationY = 0.0f;
		float xPoseLocalRotationZ = 0.0f;
		
		float yPoseLocalRotationX = 0.0f;
		float yPoseLocalRotationY = 0.0f;
		float yPoseLocalRotationZ = 0.0f;
		
		float zPoseLocalRotationX = 0.0f;
		float zPoseLocalRotationY = 0.0f;
		float zPoseLocalRotationZ = 0.0f;

		switch(arm)
		{
			case Arm.Right:
			{
				xPoseLocalRotationX = -5.050629f;
				xPoseLocalRotationY = 26.61955f;
				xPoseLocalRotationZ = 15.66278f;

				yPoseLocalRotationX = -5.050629f;
				yPoseLocalRotationY = 26.61955f;
				yPoseLocalRotationZ = 102.4f;

				zPoseLocalRotationX = -80.03735f;
				zPoseLocalRotationY = -55.55f;
				zPoseLocalRotationZ = 178.6759f;

				break;
			}
			case Arm.Left:
			{
				xPoseLocalRotationX = 0.0f;
				xPoseLocalRotationY = 0.0f;
				xPoseLocalRotationZ = 0.0f;
				
				yPoseLocalRotationX = 0.0f;
				yPoseLocalRotationY = 0.0f;
				yPoseLocalRotationZ = 0.0f;
				
				zPoseLocalRotationX = 0.0f;
				zPoseLocalRotationY = 0.0f;
				zPoseLocalRotationZ = 0.0f;

				break;
			}
		}
		
		m_xPoseGlobalRotation = transform.TransformPoint(new Vector3(xPoseLocalRotationX, xPoseLocalRotationY, xPoseLocalRotationZ));
		m_yPoseGlobalRotation = transform.TransformPoint(new Vector3(yPoseLocalRotationX, yPoseLocalRotationY, yPoseLocalRotationZ));
		m_zPoseGlobalRotation = transform.TransformPoint(new Vector3(zPoseLocalRotationX, zPoseLocalRotationY, zPoseLocalRotationZ));

		m_calibrator = new IMUCalibrator(m_xPoseGlobalRotation, m_yPoseGlobalRotation, m_zPoseGlobalRotation);

		m_initializer = new IMUInitializer(ref m_calibrator, 500);

		m_callback = new HVR_Tracking.ShoulderCallback(OnShoulderEvent);
		
		HVR_Tracking.RegisterShoulderCallback(m_callback);
	}

	public void Update () 
	{
		m_deltaTime = Time.deltaTime;

		if(m_initializer.Complete)
		{
			transform.eulerAngles = m_calibrator.ComputeRotation(roll, yaw, pitch);
		}
	}

	public void OnShoulderEvent(float s_yaw, float s_pitch, float s_roll, byte side)
	{
        if((side == 1 && arm == Arm.Right) || (side == 2 && arm == Arm.Left))
		{
			yaw   = s_yaw;
			pitch = s_pitch;
			roll  = s_roll;

			if(!m_trackingOn)
			{
				Debug.Log("Starting shoulder offset calibration");
			}

			m_trackingOn = true;

			if(!m_initializer.Complete)
			{
				m_initializer.Update(m_deltaTime, s_roll, s_yaw, s_pitch);
			}
		}
	}
}
