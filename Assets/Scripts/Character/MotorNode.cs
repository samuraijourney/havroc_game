﻿using UnityEngine;
using System;
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

	private float m_maxDamage = 5.0f;
	private float m_motorMinIntensity = 150.0f;
	private float m_motorMaxIntensity = 255.0f;

	// Use this for initialization
	void Start () 
    {
		m_havrocPlayer = GameObject.Find ("Havroc Player");
		m_original_color = gameObject.GetComponent<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (Time.time > m_hit_end_time) 
		{
			gameObject.GetComponent<Renderer>().material.color = m_original_color;
			m_hit_end_time = Mathf.Infinity;
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag("Node"))
		{
			return;
		}

		if (motorIndex != 0) 
		{
			gameObject.GetComponent<Renderer>().material.color = Color.red;
			
			m_hit_end_time = Time.time + m_hit_delay;

			CollisionData data = new CollisionData();
			data.motorIndex = motorIndex;
			data.collision = col;

			m_havrocPlayer.SendMessage("PassCollisionData", data);

			if (col.gameObject.CompareTag("Bullet"))
			{
				Destroy (col.gameObject);
			}

			byte[] motorIndexArr = new byte[]{(byte)motorIndex};
			byte[] motorIntensityArr = new byte[]{ComputeMotorIntensity(col)};
			HVR_Network.SendMotorCommand(motorIndexArr,motorIntensityArr,1);

			if (col.gameObject.CompareTag("Fist"))
			{
				//col.rigidbody.velocity
				return;
			}
		}
	}

	private byte ComputeMotorIntensity(Collision col)
	{
		float intensity = 0;

		float scale = (m_motorMaxIntensity - m_motorMinIntensity) / m_maxDamage;

		intensity *= scale;
		intensity += m_motorMinIntensity;

		return Convert.ToByte(intensity);
	}
}
