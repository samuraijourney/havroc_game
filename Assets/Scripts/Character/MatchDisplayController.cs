using UnityEngine;
using System.Collections;

public class MatchDisplayController : MonoBehaviour 
{
	private GameObject m_ovrCentreAnchor;
	private float m_distance;

	private Vector3 m_scale;
	private Vector3 m_rotation;

	// Use this for initialization
	void Start () 
	{
		m_ovrCentreAnchor = GameObject.Find ("CenterEyeAnchor");
		m_distance = (m_ovrCentreAnchor.transform.position - transform.position).magnitude;

		m_scale = transform.localScale;
		m_rotation = transform.eulerAngles;
	}

	// Update is called once per frame
	void Update () 
	{
		transform.localScale = m_scale;
		transform.eulerAngles = new Vector3(m_ovrCentreAnchor.transform.eulerAngles.x,m_ovrCentreAnchor.transform.eulerAngles.y,m_rotation.z);
	}
}
