using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour
{

	protected bool dead = false;
	protected bool grounded = false;
	protected bool canJump = false;
	protected bool facingLeft = false;

	protected Transform groundCheck;

	protected Rigidbody2D body;
	Animator anim;

	protected int speed = 1;
	protected int damage = 0;
	protected int hp = 1;
	protected float jumpForce = 200;
	protected float attackRange = 0.5f;

	Transform player;

	// Use this for initialization
	protected void Start ()
	{
		body = GetComponent<Rigidbody2D> ();
		// Don't find player here - Find in Update()
		// player = GameObject.FindGameObjectWithTag ("Player").transform;
		groundCheck = transform.Find("GroundCheck");
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	protected void Update ()
	{
//		Debug.Log (player.position.x + "," + player.position.y + "," + player.position.z);
		grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
		anim.SetBool ("MGround", grounded);

		if (grounded) {
			canJump = true;
		}

	}

	protected void FindPlayer()
	{
		if (player != null)
			return;

		GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
		if (playerObject != null)
		{
			player = playerObject.transform;
		}
	}

	protected bool isPlayerOnLeft() {
		return player != null ? transform.position.x > player.position.x : false;
	}

	protected void correctDirection() {
		if (player == null)
			return;

		bool playerOnLeft = isPlayerOnLeft();

		if ((Mathf.Abs(transform.position.x - player.position.x) > 0.5) && ((!facingLeft && playerOnLeft) || (facingLeft && !playerOnLeft))) {
			Flip ();
		}
	}

	protected bool isSameGroundLevel() {
		return player != null ? Mathf.Abs (transform.position.y - player.position.y) < 0.5 : false;
	}

	protected bool isInAttackRange() {
		return player != null ? Mathf.Abs (transform.position.x - player.position.x) < attackRange && isSameGroundLevel() : false;
	}

	protected void FixedUpdate ()
	{
		FindPlayer();
		
		correctDirection ();

//		int dir = isPlayerOnLeft() ? -1 : 1;
		float hSpeed = transform.localScale.x * speed;
		body.velocity = new Vector2 (hSpeed, body.velocity.y);
		anim.SetFloat ("MSpeed", Mathf.Abs (hSpeed));

//		Debug.Log (transform.position.y + " vs " + player.position.y);

		if (player != null && canJump && Mathf.Abs (transform.position.x - player.position.x) < 1 && transform.position.y + 0.5 < player.position.y) {
			canJump = false;
			Jump ();
		}

		if (isInAttackRange ()) {
			Attack ();
		}

	}

	protected void Attack(){
		anim.SetBool("MAttack",true);

	}

	//this is used when event is triggered in ZombieAttack animation
	protected void AttackDone(){
		anim.SetBool ("MAttack", false);
	}

	protected void Flip(){
		facingLeft = !facingLeft;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	protected void Jump ()
	{
		body.AddForce (new Vector2 (0, jumpForce));
	}


}
