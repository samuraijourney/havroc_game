using UnityEngine;
using System;
using System.Collections;

public class HavrocController : MonoBehaviour, IFightStateMember, IEndStateMember {

	public float damagePerTorsoNode = 0.1f;
	public float damagePerFistNode = 0.01f;
	public float damagePerArmNode = 0.03f;

	public float animSpeed = 1.0f;

	private bool lose = false;
	private bool win = false;
	
	private EnemyController m_enemyController;

	private bool m_isAttacking = false;

	private bool m_pass = false;
	private float m_damageAccum = 0.0f;
	private int m_damageCount = 0;
	
	private Animator m_anim;
	private AnimatorStateInfo m_currentBaseState;	

	private GameObject m_healthBar;
	private Transform m_enemy;

	private bool m_heartbeat = false;
	private float m_heartbeatDelay = 1.0f;
	private float m_heartbeatAccum = 0.0f;

	private bool m_enable = false;

	private int m_lastAttackCount = 0;
	private bool m_disableTorsoHit = false;
	private bool m_disableUpperArmHit = false;

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
		if(m_enable)
		{
			if(m_lastAttackCount != m_enemyController.AttackCount)
			{
				m_disableTorsoHit = false;
				m_disableUpperArmHit = false;
			}

			m_lastAttackCount = m_enemyController.AttackCount;
			
			if(!lose && !win)
			{
				transform.LookAt (new Vector3(m_enemy.position.x,transform.position.y,m_enemy.position.z));
			}

			if(m_damageAccum > 0)
			{
				m_healthBar.SendMessage("ApplyDamage", m_damageAccum);
				m_damageAccum = 0;
			}

			if(m_heartbeat)
			{
				if(m_heartbeatAccum > m_heartbeatDelay)
				{
					HVR_Network.SendMotorCommand(GetHeartMotorIndices(),GetHeartMotorIntensities(), 4);
					
					m_heartbeatAccum -= m_heartbeatDelay;
				}
				else
				{
					m_heartbeatAccum += Time.deltaTime;
				}
			}
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
		return IsUpperArmMotorNode(index) || IsLowerArmMotorNode(index);
	}

	bool IsUpperArmMotorNode(int index)
	{
		if(index >= 10 && index <= 15)
		{
			return true;
		}
		
		if(index >= 58 && index <= 63)
		{
			return true;
		}
		
		return false;
	}

	bool IsLowerArmMotorNode(int index)
	{
		if(index >= 4 && index <= 9)
		{
			return true;
		}
		
		if(index >= 64 && index <= 69)
		{
			return true;
		}
		
		return false;
	}

	bool IsFistMotorNode(int index)
	{
		if(index >= 1 && index <= 3)
		{
			return true;
		}
		
		if(index >= 70 && index <= 72)
		{
			return true;
		}
		
		return false;
	}

	bool IsTorsoMotorNode(int index)
	{
		if(index >= 16 && index <= 57)
		{
			return true;
		}
		
		return false;
	}

	private byte[] GetHeartMotorIndices()
	{
		return new byte[]{21,52,24,49};
	}

	private byte[] GetHeartMotorIntensities()
	{
		return new byte[]{200,200,200,200};
	}

	void PassCollisionData(CollisionData data)
	{
		m_isAttacking = false; // Gotta update this somehow

		if(data.collision.gameObject.tag != "Player")
		{
			return;
		}
		
		if(IsLowerArmMotorNode(data.motorIndex))
		{
			m_damageAccum += damagePerArmNode;
			m_disableTorsoHit = true;
			m_disableUpperArmHit = true;

			byte[] motorIndexArr = new byte[]{(byte)data.motorIndex};
			byte[] motorIntensityArr = new byte[]{0};
			HVR_Network.SendMotorCommand(motorIndexArr,motorIntensityArr,1);

			data.motorScript.Hit();
		}
		else if(!m_disableUpperArmHit && IsUpperArmMotorNode(data.motorIndex))
		{
			m_damageAccum += damagePerArmNode;
			m_disableTorsoHit = true;
			
			byte[] motorIndexArr = new byte[]{(byte)data.motorIndex};
			byte[] motorIntensityArr = new byte[]{0};
			HVR_Network.SendMotorCommand(motorIndexArr,motorIntensityArr,1);
			
			data.motorScript.Hit();
		}

		if(!m_disableTorsoHit && IsTorsoMotorNode(data.motorIndex))
		{
			m_damageAccum += damagePerTorsoNode;

			byte[] motorIndexArr = new byte[]{(byte)data.motorIndex};
			byte[] motorIntensityArr = new byte[]{0};
			HVR_Network.SendMotorCommand(motorIndexArr,motorIntensityArr,1);

			data.motorScript.Hit();
		}
	}

	public void OnStateFightLose(PlayerType type)
	{
		if(type == PlayerType.Havroc)
		{
			lose = true;
			win = false;
		}
	}
	
	public void OnStateFightWin(PlayerType type)
	{
		if(type == PlayerType.Havroc)
		{
			lose = false;
			win = true;
		}
	}

	public void OnStateFightTimeout()
	{
	}

	public void OnStateFightTimeoutCountdown()
	{
		m_heartbeat = true;
	}

	public void OnStateBaseStart(GameState state)
	{
		if(state == GameState.Intro)
		{
			win = false;
			lose = false;

			m_heartbeat = false;
			m_heartbeatDelay = 1.0f;
			m_heartbeatAccum = 0.0f;
			m_damageAccum = 0.0f;
			m_lastAttackCount = 0;

			m_disableUpperArmHit = false;
			m_disableTorsoHit = false;

			m_anim.enabled = true;
			m_enable = true;
		}

		if(state == GameState.Calibration)
		{
			m_anim.enabled = false; // Change this to end state to give some time for death anim to end
		}
	}
	
	public void OnStateBaseEnd(GameState state)
	{
		if(state == GameState.Fight)
		{
			m_enable = false;
		}
	}

	public void OnStateEndCameraPanAway()
	{
	}
	
	public void OnStateEndCameraPanBack()
	{
		win = false;
		lose = false;
	}
}
