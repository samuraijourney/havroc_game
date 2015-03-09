using UnityEngine;
using System.Collections;

public class HeadTracker : MonoBehaviour {

	private GameObject m_ovrController;

	// Use this for initialization
	void Start () 
	{
		m_ovrController = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.rotation = m_ovrController.transform.rotation;
		transform.Rotate (new Vector3 (0, -90, -90));
	}
}
