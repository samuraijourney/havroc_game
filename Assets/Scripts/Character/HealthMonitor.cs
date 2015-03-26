using UnityEngine;
using System.Collections;

public class HealthMonitor : MonoBehaviour, IFightStateMember
{
	public float maxHealth = 100f;
	public float health = 100f;
	public PlayerType playerType;

	private float m_scaleMax = 1.33f;
	private float m_scaleMin = 0.0f;

	private Transform m_scalarTransform;
	private Transform m_healthBarTransform;
	private Transform m_deadXTransform;
	private TextMesh m_healthText;

	public delegate void KnockoutCallback(PlayerType type);
	public event KnockoutCallback OnKnockoutEvent;

	private bool m_enabled = false;
	private bool m_destroy = false;

	// Use this for initialization
	void Start () 
	{
		m_scalarTransform = transform.Find ("Bar/Scalar");
		m_healthBarTransform = transform.Find ("Bar/Scalar/Health");
		m_deadXTransform = transform.Find ("Bar/Name Bar/Name/X");
		m_healthText = transform.Find ("Health Text").GetComponent<TextMesh> ();

		m_deadXTransform.GetComponent<Renderer>().material.color = Color.clear;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_enabled)
		{
			if(m_destroy)
			{
				health--;
			}

			float scaledHealth = health / maxHealth;
			m_healthText.text = (int)(scaledHealth*100.0f) + "%";
			
			float scale = (m_scaleMax - m_scaleMin) * scaledHealth + m_scaleMin;
			float r = 1.0f - scaledHealth;
			float g = scaledHealth;
			float b = 0;
			
			m_scalarTransform.localScale = new Vector3 (scale, m_scalarTransform.localScale.y, m_scalarTransform.localScale.z);
			m_healthBarTransform.GetComponent<Renderer>().material.color = new Color (r,g,b);
			
			if (health < 1.0f) 
			{
				m_deadXTransform.GetComponent<Renderer>().material.color = Color.red;
				
				if(OnKnockoutEvent != null)
				{
					OnKnockoutEvent(playerType);
				}
			} 
			else 
			{
				m_deadXTransform.GetComponent<Renderer>().material.color = Color.clear;
			}
		}
	}

	void ApplyDamage(float damage)
	{
		if(!m_destroy)
		{
			health -= damage;
			health = health > 0 ? health : 0;
		}
	}

	public void OnStateBaseStart(GameState state)
	{
		if(state == GameState.Intro)
		{
			m_enabled = true;
		}
	}
	
	public void OnStateBaseEnd(GameState state)
	{
		if(state == GameState.Fight)
		{
			m_enabled = false;
			m_destroy = false;
			health = maxHealth;
		}
	}

	public void OnStateFightWin(PlayerType type)
	{
	}

	public void OnStateFightLose(PlayerType type)
	{
	}

	public void OnStateFightTimeout()
	{
		m_destroy = true;
	}
}
