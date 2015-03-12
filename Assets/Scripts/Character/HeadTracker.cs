using UnityEngine;
using System.Collections;

public class HeadTracker : MonoBehaviour {

	private GameObject m_ovrCentreAnchor;
	private GameObject m_havrocPlayer;

	// Use this for initialization
	void Start () 
	{
		m_ovrCentreAnchor = GameObject.Find ("CenterEyeAnchor");
		m_havrocPlayer = GameObject.Find ("Havroc Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.rotation = m_ovrCentreAnchor.transform.rotation;
		transform.Rotate (new Vector3 (0, 90, -90));
	}
}
