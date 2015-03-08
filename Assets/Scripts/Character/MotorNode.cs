using UnityEngine;
using System.Collections;

public class MotorNode : MonoBehaviour 
{
    public int motorIndex = -1;

	private float m_hit_delay = 1.0f;
	private float m_hit_end_time = Mathf.Infinity;

	private Color m_original_color;

	// Use this for initialization
	void Start () 
    {
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
			//if (col.gameObject.CompareTag("Bullet") || col.gameObject.CompareTag("Player")) 
			//{
			Vector3 vel = col.relativeVelocity;
			
			Debug.Log ("Motor hit - Index:" + motorIndex + " Speed:(" + vel.x + "," + vel.y + "," + vel.z + ")");
			
			m_original_color = gameObject.renderer.material.color;
			gameObject.renderer.material.color = Color.red;
			
			m_hit_end_time = Time.time + m_hit_delay;

			if (col.gameObject.CompareTag("Bullet"))
		    {
				Destroy (col.gameObject);
			}
		}
	}
}
