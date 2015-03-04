using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour 
{
	public float speed = 5.0f; // m/s
	public float shotInterval = 0.2f; // interval between shots
	public Rigidbody bulletPrefab; // drag the bullet prefab here

	private Transform m_bulletSpawn;
	private Camera m_camera;
	
	//private float m_distance = 0f;
	
	// Use this for initialization
	void Start () 
	{
		m_bulletSpawn = transform.Find ("Bullet Spawn Point"); // only works if bulletSpawn is a child!
		m_camera = GameObject.Find("OVRCameraRig").camera;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			Rigidbody bullet = (Rigidbody)Instantiate(bulletPrefab, m_bulletSpawn.position, m_bulletSpawn.rotation);
			bullet.AddForce(transform.forward*speed); // shoot in the target direction

			//RaycastHit hit;
			
			//if(Physics.Raycast (ray, out hit, Mathf.Infinity))
			//{
			//	m_distance = hit.distance;
			//	hit.transform.SendMessage("ActuateNode", speed, SendMessageOptions.DontRequireReceiver);
			//	Debug.DrawRay(ray.origin, ray.direction * 100, Color.green, 1);
			//}
		}
	}
}
