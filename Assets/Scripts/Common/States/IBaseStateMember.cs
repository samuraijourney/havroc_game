using UnityEngine;

public interface IBaseStateMember
{
	void OnStateBaseStart(GameState state);
	void OnStateBaseEnd(GameState state);
}

