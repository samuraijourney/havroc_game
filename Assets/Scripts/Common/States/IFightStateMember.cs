using UnityEngine;

public interface IFightStateMember : IBaseStateMember
{
	void OnStateFightWin(PlayerType type);
	void OnStateFightLose(PlayerType type);
	void OnStateFightTimeout();
	void OnStateFightTimeoutCountdown();
}
