using UnityEngine;
using System.Collections;

// Require these components when using this script
[RequireComponent(typeof (Animator))]
public class EnemyController : MonoBehaviour
{
	[System.NonSerialized]
	public Transform enemy;								// a transform to Lerp the camera to during head look

	public float animSpeed = 1.5f;						// a public setting for overall animator animation speed

	private Animator anim;								// a reference to the animator on the character
	private AnimatorStateInfo currentBaseState;			// a reference to the current state of the animator, used for base layer
	private GameObject healthBar;
	private HavrocController havrocController;

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

		anim.SetBool("Block", Input.GetButtonDown("Block"));

		if (currentBaseState.nameHash == idleState)
		{
			if(Input.GetButtonDown("Right Punch"))
			{
				anim.SetBool("RightPunch", true);
			}
			else if(Input.GetButtonDown("Left Punch"))
			{
				anim.SetBool("LeftPunch", false);
			}
		}

		else if (currentBaseState.nameHash == rightCrossState)
		{
			anim.SetBool("RightPunch", false);
		}

		else if (currentBaseState.nameHash == leftJabState)
		{
			anim.SetBool("LeftPunch", false);
		}

		else if (currentBaseState.nameHash == winState)
		{
			anim.SetBool("Win", false);
		}

		else if (currentBaseState.nameHash == knockoutCountdownState)
		{
			anim.SetBool("Knockdown", false);
		}

		else if (currentBaseState.nameHash == knockoutState)
		{
			anim.SetBool("Lose", false);
		}


		transform.LookAt (enemy.position);
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
			healthBar.SendMessage("ApplyDamage", ComputeDamage(col.relativeVelocity));
		}
	}

	float ComputeDamage(Vector3 velocity)
	{
		return 1.0f;
	}
}
