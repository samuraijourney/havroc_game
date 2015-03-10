using UnityEngine;
using System.Collections;

public class HealthMonitor : MonoBehaviour 
{
	public float maxHealth = 100f;
	public float health = 100f;
	public Transform player;

	private float m_scaleMax = 1.33f;
	private float m_scaleMin = 0.0f;

	private Transform m_scalarTransform;
	private Transform m_healthBarTransform;
	private Transform m_deadXTransform;
	private TextMesh m_healthText;

	// Use this for initialization
	void Start () 
	{
		m_scalarTransform = transform.Find ("Bar/Scalar");
		m_healthBarTransform = transform.Find ("Bar/Scalar/Health");
		m_deadXTransform = transform.Find ("Bar/Name Bar/Name/X");
		m_healthText = transform.Find ("Health Text").GetComponent<TextMesh> ();

		m_deadXTransform.renderer.material.color = Color.clear;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float scaledHealth = health / maxHealth;
		m_healthText.text = (int)(scaledHealth*100.0f) + "%";
		
		float scale = (m_scaleMax - m_scaleMin) * scaledHealth + m_scaleMin;
		float r = 1.0f - scaledHealth;
		float g = scaledHealth;
		float b = 0;
		
		m_scalarTransform.localScale = new Vector3 (scale, m_scalarTransform.localScale.y, m_scalarTransform.localScale.z);
		m_healthBarTransform.renderer.material.color = new Color (r,g,b);
		
		if (health < 1.0f) 
		{
			m_deadXTransform.renderer.material.color = Color.red;

			player.SendMessage("Dead");
		} 
		else 
		{
			m_deadXTransform.renderer.material.color = Color.clear;
		}
	}

	void ApplyDamage(float damage)
	{
		health -= damage;
		health = health > 0 ? health : 0;
	}
}
