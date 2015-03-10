using UnityEngine;
using System.Collections;

public class HeadTracker : MonoBehaviour {

	private GameObject m_ovrController;
	private GameObject m_havrocPlayer;

	// Use this for initialization
	void Start () 
	{
		m_ovrController = GameObject.FindGameObjectWithTag ("MainCamera");
		m_havrocPlayer = GameObject.Find ("Havroc Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.rotation = m_ovrController.transform.rotation;
		transform.Rotate (new Vector3 (0, 90, -90));

		if(Mathf.Abs(transform.localEulerAngles.x) > 90.0f)
		{
			//m_havrocPlayer.transform.LookAt(m_ovrController.transform.forward);
			//m_havrocPlayer.transform.Rotate (new Vector3 (-90, 0, 0));
			//float delta = -90.0f*diff/Mathf.Abs(diff);
			//m_havrocPlayer.transform.Rotate(new Vector3(0,-(diff+delta),0));
		}
	}
}
