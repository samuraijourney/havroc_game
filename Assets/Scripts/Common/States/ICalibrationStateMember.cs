using UnityEngine;

public interface ICalibrationStateMember : IBaseStateMember
{
	void OnStateCalibrationPoseUpdate(CalibrationPose pose); 
	void OnStateCalibrationDone(IMUCalibrator calibrator);

	Vector3 GlobalXPosition{ get; }
	Vector3 GlobalYPosition{ get; }
	Vector3 GlobalZPosition{ get; }

	Arm Arm{ get; }
	Joint Joint{ get; }
}

