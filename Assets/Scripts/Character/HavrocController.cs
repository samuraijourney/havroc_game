using UnityEngine;
using System.Collections;

public class HavrocController : MonoBehaviour {

	public float damageModifier = 20.0f;

	private EnemyController m_enemyController;

	private bool m_isAttacking = false;

	private bool m_pass = true;
	private int m_damageCount = 0;

	private bool m_dead = false;
	private float m_deadTimer = 0f;
	private float m_deadStartAngle = 0f;
	private float m_fallSpeed = 1.4f;

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

		if(!m_dead)
		{
			transform.LookAt (new Vector3(m_enemy.position.x,transform.position.y,m_enemy.position.z));
			m_deadStartAngle = transform.eulerAngles.x;
		}
		else
		{
			m_deadTimer += Time.deltaTime*m_fallSpeed;
			float x = Mathf.PI*m_deadTimer;
			float inc = Mathf.Lerp(m_deadStartAngle,12.0f*Mathf.Sin(x)/x - 90.0f,m_deadTimer);
			transform.eulerAngles = new Vector3(inc,transform.eulerAngles.y,transform.eulerAngles.z);
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
				float damage = ComputeDamage(data.collision.relativeVelocity);
				m_healthBar.SendMessage("ApplyDamage", damage);
				m_pass = false;

				Debug.Log("Damage: " + damage);
			}
		}
		else
		{
			float damage = ComputeDamage(data.collision.relativeVelocity);
			m_healthBar.SendMessage("ApplyDamage", damage);

			Debug.Log("Damage: " + damage);
		}

		m_isAttacking = IsArmMotorNode(data.motorIndex);

		Vector3 vel = data.collision.relativeVelocity;
		//Debug.Log ("Motor " + data.motorIndex + " Hit");
	}

	float ComputeDamage(Vector3 velocity)
	{
		Vector3 velProj = Vector3.Project (velocity, transform.forward);

		return velProj.magnitude * damageModifier;
	}

	void Dead()
	{
		m_dead = true;
	}
}
