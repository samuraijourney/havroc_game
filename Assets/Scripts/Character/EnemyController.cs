using UnityEngine;
using System.Collections;

// Require these components when using this script
[RequireComponent(typeof (Animator))]
public class EnemyController : MonoBehaviour
{
	public float animSpeed = 1.0f;						// a public setting for overall animator animation speed
	public float damagePerHit = 1.0f;

	private bool lose = false;
	private bool win = false;

	private Animator anim;								// a reference to the animator on the character
	private AnimatorStateInfo currentBaseState;			// a reference to the current state of the animator, used for base layer
	private AnimatorStateInfo nextBaseState;
	private GameObject healthBar;
	private HavrocController havrocController;
	private Transform enemy;								// a transform to Lerp the camera to during head look

	private int attackCount = 0;

	int idleState = Animator.StringToHash("Base Layer.Idle");	
	int hitByJabState = Animator.StringToHash("Base Layer.Hit By Jab");			// these integers are references to our animator's states
	int leftJabState = Animator.StringToHash("Base Layer.Left Jab");				// and are used to check state for various actions to occur
	int rightCrossState = Animator.StringToHash("Base Layer.Right Cross");		// within our FixedUpdate() function below
	int blockState = Animator.StringToHash("Base Layer.Block");
	int knockoutState = Animator.StringToHash("Base Layer.Knockout");
	int knockoutCountdownState = Animator.StringToHash("Base Layer.Knockout Countdown");
	int winState = Animator.StringToHash("Base Layer.Win");

	void Start ()
	{
		anim = GetComponent<Animator>();			
		enemy = GameObject.Find("Havroc Player").transform;	
		healthBar = GameObject.Find ("Health Bar Enemy");
		havrocController = GameObject.Find ("Havroc Player").GetComponent<HavrocController> ();
	}

	void FixedUpdate ()
	{
		anim.speed = animSpeed;									// set the speed of our animator to the public variable 'animSpeed'
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// set our currentState variable to the current state of the Base Layer (0) of animation
		nextBaseState = anim.GetNextAnimatorStateInfo (0);

		if(!win && !lose)
		{
			AnimateBlock(Input.GetButton("Block"));
		}

		if (currentBaseState.nameHash == idleState)
		{
			if(win && nextBaseState.nameHash != winState)
			{
				AnimateWin(true);
			}
			else if(lose && nextBaseState.nameHash != knockoutState)
			{
				AnimateLose(true);
			}
			else if(Input.GetButtonDown("Right Punch") && nextBaseState.nameHash != rightCrossState)
			{
				AnimateRightPunch(true);
			}
			else if(Input.GetButtonDown("Left Punch") && nextBaseState.nameHash != leftJabState)
			{
				AnimateLeftPunch(true);
			}
		}

		else if (currentBaseState.nameHash == rightCrossState)
		{
			AnimateRightPunch(false);

			if(Input.GetButtonDown("Left Punch") && nextBaseState.nameHash != leftJabState)
			{
				AnimateLeftPunch(true);
			}
		}

		else if (currentBaseState.nameHash == leftJabState)
		{
			AnimateLeftPunch(false);

			if(Input.GetButtonDown("Right Punch") && nextBaseState.nameHash != rightCrossState)
			{
				AnimateRightPunch(true);
			}
		}

		else if (currentBaseState.nameHash == knockoutCountdownState)
		{
			AnimateKnockdown(false);
		}

		/*else if (currentBaseState.nameHash == winState)
		{
			AnimateWin(false);
		}
		
		else if (currentBaseState.nameHash == knockoutState)
		{
			AnimateLose(false);
		}*/

		transform.LookAt (enemy.position);
	}

	private void AnimateBlock(bool on)
	{
		Animate ("Block", on);
	}

	private void AnimateRightPunch(bool on)
	{
		if(on)
		{
			attackCount++;
		}

		Animate ("RightPunch", on);
	}

	private void AnimateLeftPunch(bool on)
	{
		if(on)
		{
			attackCount++;
		}

		Animate ("LeftPunch", on);
	}

	private void AnimateKnockdown(bool on)
	{
		Animate ("Knockdown", on);
	}

	private void AnimateWin(bool on)
	{
		Animate ("Win", on);
	}

	private void AnimateLose(bool on)
	{
		Animate ("Lose", on);
	}

	private void Animate(string state, bool on)
	{
		anim.SetBool (state, on);
	}

	public int AttackCount
	{
		get
		{
			return attackCount;
		}
	}

	public bool IsAttacking
	{
		get
		{
			return 	(currentBaseState.nameHash == rightCrossState) ||
					(currentBaseState.nameHash == leftJabState);
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (havrocController.IsAttacking)
		{
			healthBar.SendMessage("ApplyDamage", damagePerHit);
		}
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
