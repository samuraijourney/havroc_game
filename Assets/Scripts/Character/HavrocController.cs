using UnityEngine;
using System.Collections;

public class HavrocController : MonoBehaviour {

	private EnemyController m_enemyController;

	private bool m_isAttacking = false;

	private bool m_isBeingAttacked = false;
	private bool m_wasBeingAttacked = false;

	private bool m_pass = true;

	private GameObject m_healthBar;
	
	void Start () 
	{
		m_enemyController = GameObject.Find ("Enemy Player").GetComponent<EnemyController> ();
		m_healthBar = GameObject.Find ("Health Bar Havroc");
	}

	void Update () 
	{
		m_wasBeingAttacked = m_isBeingAttacked;
		m_isBeingAttacked = m_enemyController.IsAttacking;

		if(!m_wasBeingAttacked && m_isBeingAttacked)
		{
			m_pass = true;
		}
	}

	public bool IsAttacking
	{
		get
		{
			return m_isAttacking;
		}
	}

	bool IsArmMotorNode(int index)
	{
		return false;
	}

	void PassCollisionData(CollisionData data)
	{
		if(data.collision.gameObject.CompareTag("Player"))
		{
			if(m_pass)
			{
				m_healthBar.SendMessage("ApplyDamage", ComputeDamage(data.collision.relativeVelocity));
				m_pass = false;
			}
		}
		else
		{
			m_healthBar.SendMessage("ApplyDamage", ComputeDamage(data.collision.relativeVelocity));
		}

		m_isAttacking = IsArmMotorNode(data.motorIndex);
	}

	float ComputeDamage(Vector3 velocity)
	{
		return 1.0f;
	}
}
