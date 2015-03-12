using UnityEngine;
using System.Collections;

public class HavrocController : MonoBehaviour {

	public float damagePerHit = 1.0f;
	public float animSpeed = 1.0f;

	private bool lose = false;
	private bool win = false;
	
	private EnemyController m_enemyController;

	private bool m_isAttacking = false;

	private bool m_pass = true;
	private int m_damageCount = 0;
	
	private Animator m_anim;
	private AnimatorStateInfo m_currentBaseState;	

	private GameObject m_healthBar;
	private Transform m_enemy;

	int m_idleState = Animator.StringToHash("Base Layer.Idle");	
	int m_loseState = Animator.StringToHash("Base Layer.Lose");
	int m_winState = Animator.StringToHash("Base Layer.Win");
	
	void Start () 
	{
		m_anim = GetComponent<Animator>();
		m_enemyController = GameObject.Find ("Enemy Player").GetComponent<EnemyController> ();
		m_healthBar = GameObject.Find ("Health Bar Havroc");
		m_enemy = GameObject.Find("Enemy Player").transform;	
	}

	void FixedUpdate()
	{
		m_anim.speed = animSpeed;
		m_currentBaseState = m_anim.GetCurrentAnimatorStateInfo(0);
		
		AnimateLose(lose);
		AnimateWin(win);
	}

	private void AnimateLose(bool on)
	{
		Animate ("Lose", on);
	}
	
	private void AnimateWin(bool on)
	{
		Animate ("Win", on);
	}
	
	private void Animate(string state, bool on)
	{
		m_anim.SetBool (state, on);
	}

	void Update () 
	{
		if(m_damageCount < m_enemyController.AttackCount)
		{
			m_pass = true;
			m_damageCount++;
		}

		if(!lose && !win)
		{
			transform.LookAt (new Vector3(m_enemy.position.x,transform.position.y,m_enemy.position.z));
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
				m_healthBar.SendMessage("ApplyDamage", damagePerHit);
				m_pass = false;

				Debug.Log("Damage: " + damagePerHit);
			}
		}
		else
		{
			m_healthBar.SendMessage("ApplyDamage", damagePerHit);

			Debug.Log("Damage: " + damagePerHit);
		}

		m_isAttacking = IsArmMotorNode(data.motorIndex);

		Vector3 vel = data.collision.relativeVelocity;
		//Debug.Log ("Motor " + data.motorIndex + " Hit");
	}

	void Lose()
	{
		lose = true;
	}

	void Win()
	{
		win = true;
	}
}
