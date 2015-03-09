using UnityEngine;
using System.Collections;

public class CollisionData
{
	public int motorIndex;
	public Collision collision;
}

public class MotorNode : MonoBehaviour 
{
    public int motorIndex = -1;

	private float m_hit_delay = 1.0f;
	private float m_hit_end_time = Mathf.Infinity;

	private Color m_original_color;
	private GameObject m_havrocPlayer;

	// Use this for initialization
	void Start () 
    {
		m_havrocPlayer = GameObject.Find ("Havroc Player");
		m_original_color = gameObject.renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (Time.time > m_hit_end_time) 
		{
			gameObject.renderer.material.color = m_original_color;
			m_hit_end_time = Mathf.Infinity;
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (motorIndex != 0) 
		{
			Vector3 vel = col.relativeVelocity;
			
			Debug.Log ("Motor hit - Index:" + motorIndex + " Speed:(" + vel.x + "," + vel.y + "," + vel.z + ")");

			gameObject.renderer.material.color = Color.red;
			
			m_hit_end_time = Time.time + m_hit_delay;

			CollisionData data = new CollisionData();
			data.motorIndex = motorIndex;
			data.collision = col;

			m_havrocPlayer.SendMessage("PassCollisionData", data);

			if (col.gameObject.CompareTag("Bullet"))
			{
				Destroy (col.gameObject);
			}
		}
	}
}
