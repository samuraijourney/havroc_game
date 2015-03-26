using UnityEngine;
using System.Collections;

public class EndState : BaseState 
{
	private Transform m_endTransform;
	private Transform m_fightTransform;

	private GameObject m_oculusCamera;
	private Transform m_oculusParent;

	private bool m_return = false;

	void Start () 
	{
		m_oculusCamera = GameObject.Find ("OVRPlayerController");

		m_endTransform = transform.Find ("End View");
		m_fightTransform = (GameObject.Find ("Fight View")).transform;
	}

	override protected void Setup()
	{
		m_return = false;

		m_oculusParent = m_oculusCamera.transform.parent;
		m_oculusCamera.transform.parent = transform;
	}
	
	override protected void UpdateState() 
	{
		if(m_return)
		{
			m_oculusCamera.transform.position = Vector3.Lerp (m_oculusCamera.transform.position, m_fightTransform.position, Time.deltaTime);
			m_oculusCamera.transform.rotation = Quaternion.Lerp (m_oculusCamera.transform.rotation, m_fightTransform.rotation, Time.deltaTime);

			if((m_oculusCamera.transform.position - m_fightTransform.position).magnitude < 0.1)
			{
				m_oculusCamera.transform.parent = m_oculusParent;

				IsComplete = true;
			}
		}
		else
		{
			m_oculusCamera.transform.position = Vector3.Lerp (m_oculusCamera.transform.position, m_endTransform.position, Time.deltaTime);
			m_oculusCamera.transform.rotation = Quaternion.Lerp (m_oculusCamera.transform.rotation, m_endTransform.rotation, Time.deltaTime);
		}

		if(Input.GetButtonDown("Reset"))
		{
			m_return = true;
		}
	}
	
	override protected void Clean()
	{
	}

	override public GameState State 
	{ 
		get
		{
			return GameState.End;
		}
	}
}
