using UnityEngine;
using System.Collections;

public class CalibrationCameraPanner : MonoBehaviour, IBaseStateMember 
{
	public float animationSpeed = 0.5f;

	private Animation m_anim;

	// Use this for initialization
	void Start () 
	{
		m_anim = GetComponent<Animation>();
		m_anim.wrapMode = WrapMode.Once;
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void OnStateBaseStart(GameState state)
	{
		if(state == GameState.Calibration)
		{
			m_anim.Play();
		}
	}

	public void OnStateBaseEnd(GameState state)
	{
		m_anim.Stop();
	}
}
