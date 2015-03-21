using UnityEngine;
using System.Collections;

public class HeadTracker : MonoBehaviour, IBaseStateMember
{
	private GameObject m_ovrCentreAnchor;
	private GameObject m_havrocPlayer;

	private bool m_enabled = false;

	// Use this for initialization
	void Start () 
	{
		m_ovrCentreAnchor = GameObject.Find ("CenterEyeAnchor");
		m_havrocPlayer = GameObject.Find ("Havroc Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_enabled)
		{
			transform.rotation = m_ovrCentreAnchor.transform.rotation;
			transform.Rotate (new Vector3 (0, 90, -90));	
		}
	}

	public void OnStateBaseStart(GameState state)
	{
		if(state == GameState.Fight)
		{
			m_enabled = true;
		}
	}

	public void OnStateBaseEnd(GameState state)
	{
		if(state == GameState.Fight)
		{
			m_enabled = false;
		}
	}
}
