using UnityEngine;
using System.Collections;

public class HavrocController : MonoBehaviour {

	public float damageModifier = 20.0f;

	private EnemyController m_enemyController;

	private bool m_isAttacking = false;

	private bool m_pass = true;
	private int m_damageCount = 0;

	private GameObject m_healthBar;
	private Transform m_enemy;
	
	void Start () 
	{
		m_enemyController = GameObject.Find ("Enemy Player").GetComponent<EnemyController> ();
		m_healthBar = GameObject.Find ("Health Bar Havroc");
		m_enemy = GameObject.Find("Enemy Player").transform;	
	}

	void Update () 
	{
		if(m_damageCount < m_enemyController.AttackCount)
		{
			m_pass = true;
			m_damageCount++;
		}

		transform.LookAt (m_enemy.position);
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

		Vector3 vel = data.collision.relativeVelocity;
		Debug.Log ("Motor hit - Index:" + data.motorIndex + " Speed:(" + vel.x + "," + vel.y + "," + vel.z + ")");
	}

	float ComputeDamage(Vector3 velocity)
	{
		Vector3 velProj = Vector3.Project (velocity, transform.forward);

		return velProj.magnitude * damageModifier;
	}
}
