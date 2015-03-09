using UnityEngine;
using System.Collections;

public class HeadTracker : MonoBehaviour {

	private GameObject _ovrController;

	// Use this for initialization
	void Start () 
	{
		_ovrController = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.rotation = _ovrController.transform.rotation;
		transform.Rotate (new Vector3 (0, -90, -90));
	}
}
