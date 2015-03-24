using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class HipTracker : MonoBehaviour
{
	public Vector3 rotationSign = new Vector3(1,1,1);
	public Vector3 rotation;
	
	private IMUCalibrator m_calibrator;

	private ShoulderTracker m_shoulderTrackerR;
	private ShoulderTracker m_shoulderTrackerL;
	private ElbowTracker m_elbowTrackerR;
	private ElbowTracker m_elbowTrackerL;

	// Use this for initialization
	public void Start () 
	{	
		ShoulderTracker[] shoulderTrackers = Transform.FindObjectsOfType<ShoulderTracker> ();
		foreach(ShoulderTracker tracker in shoulderTrackers)
		{
			if(tracker.arm == Arm.Left)
			{
				m_shoulderTrackerL = tracker;
			}
			else if(tracker.arm == Arm.Right)
			{
				m_shoulderTrackerR = tracker;
			}
		}

		ElbowTracker[] elbowTrackers = Transform.FindObjectsOfType<ElbowTracker> ();
		foreach(ElbowTracker tracker in elbowTrackers)
		{
			if(tracker.arm == Arm.Left)
			{
				m_elbowTrackerL = tracker;
			}
			else if(tracker.arm == Arm.Right)
			{
				m_elbowTrackerR = tracker;
			}
		}
	}
	
	public void LateUpdate () 
	{
		IMUCalibrator shoulderRCalibrator = m_shoulderTrackerR.Calibrator;
		IMUCalibrator shoulderLCalibrator = m_shoulderTrackerL.Calibrator;
		IMUCalibrator elbowRCalibrator = m_elbowTrackerR.Calibrator;
		IMUCalibrator elbowLCalibrator = m_elbowTrackerL.Calibrator;

		if(shoulderRCalibrator != null &&
		   shoulderLCalibrator != null &&
		   elbowRCalibrator != null &&
		   elbowLCalibrator != null)
		{
			float deltaShoulderR = shoulderRCalibrator.PlayerXPose.y - m_shoulderTrackerR.yaw;
			float deltaShoulderL = shoulderLCalibrator.PlayerXPose.y - m_shoulderTrackerL.yaw;
			float deltaElbowR = elbowRCalibrator.PlayerXPose.y - m_elbowTrackerR.yaw;
			float deltaElbowL = elbowLCalibrator.PlayerXPose.y - m_elbowTrackerL.yaw;

			if(deltaShoulderR < 0 && deltaShoulderL < 0 && deltaElbowR < 0 && deltaElbowL < 0)
			{
			}
		}

		transform.Rotate (rotation);
	}
}
