using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CalibrationPose { X, Y, None };
public enum Arm 			{ Right = 1, Left = 2, None = 3 };
public enum Joint 			{ Shoulder, Elbow, Wrist, Hip };

public class CalibrationState : BaseState 
{
	public struct CalibratorPack
	{
		public IMUCalibrator Calibrator;
		public IMUInitializer Initializer;
		public ICalibrationStateMember Member;
		public bool Waiting;
		public bool Complete;
	};

	public class PosePack
	{
		public Dictionary<Transform, Vector3> Positions;
		public Dictionary<Transform, Quaternion> Rotations;
		public Dictionary<Transform, Vector3> Scales;

		public PosePack()
		{
			Positions = new Dictionary<Transform, Vector3>();
			Rotations = new Dictionary<Transform, Quaternion>();
			Scales = new Dictionary<Transform, Vector3>();
		}
	}
	
	public int   minCalibrationCompletions = 4;
	public int   calibrationDuration = 100;
	public float waitTime = 10.0f;

	public Vector3 rightShoulderXPosePlayerRotation;
	public Vector3 rightShoulderYPosePlayerRotation;

	public Vector3 rightElbowXPosePlayerRotation;
	public Vector3 rightElbowYPosePlayerRotation;

	public Vector3 leftShoulderXPosePlayerRotation;
	public Vector3 leftShoulderYPosePlayerRotation;

	public Vector3 leftElbowXPosePlayerRotation;
	public Vector3 leftElbowYPosePlayerRotation;

	private HVR_Tracking.ShoulderCallback m_shoulderCallback;
	private HVR_Tracking.ElbowCallback m_elbowCallback;

	private CalibratorPack m_rightShoulder;
	private CalibratorPack m_rightElbow;
	private CalibratorPack m_leftShoulder;
	private CalibratorPack m_leftElbow;

	private ICalibrationStateMember[] m_members;

	public static PosePack xPosePack;
	public static PosePack yPosePack;
	
	void Start () 
	{
		m_shoulderCallback = new HVR_Tracking.ShoulderCallback(OnShoulderEvent);
		m_elbowCallback = new HVR_Tracking.ElbowCallback(OnElbowEvent);

		m_members = GetAllInterfaceInstances<ICalibrationStateMember>();

		m_rightShoulder = new CalibratorPack();
		m_rightElbow    = new CalibratorPack();
		m_leftShoulder  = new CalibratorPack();
		m_leftElbow 	= new CalibratorPack();
	}

	override protected void Setup()
	{
		SoundManager.Instance.StopAll();

		HVR_Tracking.RegisterShoulderCallback(m_shoulderCallback);
		HVR_Tracking.RegisterElbowCallback(m_elbowCallback);
		
		foreach(ICalibrationStateMember member in m_members)
		{
			if(member.Arm == Arm.Right)
			{
				if(member.Joint == Joint.Shoulder)
				{
					m_rightShoulder.Calibrator  = new IMUCalibrator(member.GlobalXPosition, member.GlobalYPosition);
					m_rightShoulder.Initializer = new IMUInitializer(ref m_rightShoulder.Calibrator);

					m_rightShoulder.Initializer.CalibrationDuration = calibrationDuration;
					m_rightShoulder.Initializer.TimerDuration = waitTime;
					m_rightShoulder.Member = member;
					m_rightShoulder.Waiting = m_rightShoulder.Initializer.Waiting;
					m_rightShoulder.Complete = false;
				}
				else if(member.Joint == Joint.Elbow)
				{
					m_rightElbow.Calibrator  = new IMUCalibrator(member.GlobalXPosition, member.GlobalYPosition);
					m_rightElbow.Initializer = new IMUInitializer(ref m_rightElbow.Calibrator);

					m_rightElbow.Initializer.CalibrationDuration = calibrationDuration;
					m_rightElbow.Initializer.TimerDuration = waitTime;
					m_rightElbow.Member = member;
					m_rightElbow.Waiting = m_rightElbow.Initializer.Waiting;
					m_rightElbow.Complete = false;
				}
			}
			else if(member.Arm == Arm.Left)
			{
				if(member.Joint == Joint.Shoulder)
				{
					m_leftShoulder.Calibrator  = new IMUCalibrator(member.GlobalXPosition, member.GlobalYPosition);
					m_leftShoulder.Initializer = new IMUInitializer(ref m_leftShoulder.Calibrator);

					m_leftShoulder.Initializer.CalibrationDuration = calibrationDuration;
					m_leftShoulder.Initializer.TimerDuration = waitTime;
					m_leftShoulder.Member = member;
					m_leftShoulder.Waiting = m_leftShoulder.Initializer.Waiting;
					m_leftShoulder.Complete = false;
				}
				else if(member.Joint == Joint.Elbow)
				{
					m_leftElbow.Calibrator  = new IMUCalibrator(member.GlobalXPosition, member.GlobalYPosition);
					m_leftElbow.Initializer = new IMUInitializer(ref m_leftElbow.Calibrator);

					m_leftElbow.Initializer.CalibrationDuration = calibrationDuration;
					m_leftElbow.Initializer.TimerDuration = waitTime;
					m_leftElbow.Member = member;
					m_leftElbow.Waiting = m_leftElbow.Initializer.Waiting;
					m_leftElbow.Complete = false;
				}
			}
		}

		if(xPosePack != null)
		{
			RestorePose(xPosePack);
		}
	}

	override protected void UpdateState() 
	{
		int incompleteJoints = minCalibrationCompletions;

		if(m_rightShoulder.Initializer.Complete)
		{
			if(!m_rightShoulder.Complete)
			{
				m_rightShoulder.Member.OnStateCalibrationDone(m_rightShoulder.Calibrator);

				m_rightShoulder.Complete = true;
			}

			incompleteJoints--;
		}
		if(m_rightElbow.Initializer.Complete)
		{
			if(!m_rightElbow.Complete)
			{
				m_rightElbow.Member.OnStateCalibrationDone(m_rightElbow.Calibrator);
				
				m_rightElbow.Complete = true;
			}

			incompleteJoints--;
		}
		if(m_leftShoulder.Initializer.Complete)
		{
			if(!m_leftShoulder.Complete)
			{
				m_leftShoulder.Member.OnStateCalibrationDone(m_leftShoulder.Calibrator);
					
				m_leftShoulder.Complete = true;
			}

			incompleteJoints--;
		}
		if(m_leftElbow.Initializer.Complete)
		{
			if(!m_leftElbow.Complete)
			{
				m_leftElbow.Member.OnStateCalibrationDone(m_leftElbow.Calibrator);
						
				m_leftElbow.Complete = true;
			}

			incompleteJoints--;
		}

		if(!m_rightShoulder.Waiting && m_rightShoulder.Initializer.Waiting)
		{
			m_rightShoulder.Member.OnStateCalibrationPoseUpdate(m_rightShoulder.Initializer.CurrentPose);
		}
		if(!m_rightElbow.Waiting && m_rightElbow.Initializer.Waiting)
		{
			m_rightElbow.Member.OnStateCalibrationPoseUpdate(m_rightElbow.Initializer.CurrentPose);
		}
		if(!m_leftShoulder.Waiting && m_leftShoulder.Initializer.Waiting)
		{
			m_leftShoulder.Member.OnStateCalibrationPoseUpdate(m_leftShoulder.Initializer.CurrentPose);
		}
		if(!m_leftElbow.Waiting && m_leftElbow.Initializer.Waiting)
		{
			m_leftElbow.Member.OnStateCalibrationPoseUpdate(m_leftElbow.Initializer.CurrentPose);
		}

		rightShoulderXPosePlayerRotation = m_rightShoulder.Calibrator.PlayerXPose;
		rightShoulderYPosePlayerRotation = m_rightShoulder.Calibrator.PlayerYPose;
		
		rightElbowXPosePlayerRotation = m_rightElbow.Calibrator.PlayerXPose;
		rightElbowYPosePlayerRotation = m_rightElbow.Calibrator.PlayerYPose;
		
		leftShoulderXPosePlayerRotation = m_leftShoulder.Calibrator.PlayerXPose;
		leftShoulderYPosePlayerRotation = m_leftShoulder.Calibrator.PlayerYPose;
		
		leftElbowXPosePlayerRotation = m_leftElbow.Calibrator.PlayerXPose;
		leftElbowYPosePlayerRotation = m_leftElbow.Calibrator.PlayerYPose;

		m_rightShoulder.Waiting = m_rightShoulder.Initializer.Waiting;
		m_rightElbow.Waiting 	= m_rightElbow.Initializer.Waiting;
		m_leftShoulder.Waiting 	= m_leftShoulder.Initializer.Waiting;
		m_leftElbow.Waiting 	= m_leftElbow.Initializer.Waiting;

		if(incompleteJoints <= 0)
		{
			IsComplete = true;
		}

		m_rightShoulder.Initializer.Update(0, 0, 0, 0); // HAAACK
	}

	override protected void Clean()
	{
		HVR_Tracking.UnregisterShoulderCallback(m_shoulderCallback);
		HVR_Tracking.UnregisterElbowCallback(m_elbowCallback);
	}

	override public GameState State 
	{ 
		get
		{
			return GameState.Calibration;
		}
	}

	public static PosePack SavePose(GameObject obj)
	{
		PosePack package = new PosePack ();

		Transform[] transforms = obj.GetComponentsInChildren<Transform>();

		foreach (Transform t in transforms) 
		{
			package.Positions.Add(t, t.localPosition);
			package.Rotations.Add(t, t.localRotation);
			package.Scales.Add(t, t.localScale);
		}

		return package;
	}

	public static void RestorePose(PosePack package)
	{
		foreach(KeyValuePair<Transform, Vector3> entry in package.Positions)
		{
			entry.Key.localPosition = entry.Value;
		}

		foreach(KeyValuePair<Transform, Quaternion> entry in package.Rotations)
		{
			entry.Key.localRotation = entry.Value;
		}

		foreach(KeyValuePair<Transform, Vector3> entry in package.Scales)
		{
			entry.Key.localScale = entry.Value;
		}
	}

	private void OnShoulderEvent(float s_w, float s_x, float s_y, float s_z, byte side)
	{
		if((Arm)side == Arm.Right)
		{
			if(!m_rightShoulder.Initializer.Complete)
			{
				m_rightShoulder.Initializer.Update(s_w, s_x, s_y, s_z);
			}
		}
		else if((Arm)side == Arm.Left)
		{
			if(!m_leftShoulder.Initializer.Complete)
			{
				m_leftShoulder.Initializer.Update(s_w, s_x, s_y, s_z);
			}
		}
	}

	private void OnElbowEvent(float e_w, float e_x, float e_y, float e_z, byte side)
	{
		if((Arm)side == Arm.Right)
		{
			if(!m_rightElbow.Initializer.Complete)
			{
				m_rightElbow.Initializer.Update(e_w, e_x, e_y, e_z);
			}
		}
		else if((Arm)side == Arm.Left)
		{
			if(!m_leftElbow.Initializer.Complete)
			{
				m_leftElbow.Initializer.Update(e_w, e_x, e_y, e_z);
			}
		}
	}
}
