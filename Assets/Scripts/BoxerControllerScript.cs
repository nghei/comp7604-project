using UnityEngine;
using System.Collections;

public class BoxerControllerScript : MonoBehaviour {

	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.
	[HideInInspector]
	public float jumpTime = 0.0f;				// Tracks last time the player takes off ground.
	[HideInInspector]
	public bool isJumping = false;				// Whether the player is currently jumping.

	public float maxSpeed = 10f;
	[HideInInspector]
	bool facingLeft = true;

	private GameObject hero;
	public Rigidbody2D character;
	Animator anim;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	public float jumpForce = 5000;

	private bool attacking = false;

	private float attackTimer = 0;
	private float attackCd = 0.3f;

	public Collider2D attackTrigger;


	void Awake(){
	}

	// Use this for initialization
	void Start () {
		// Setting up references.
		hero = GameObject.FindGameObjectWithTag("Player");
		character = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();
		attackTrigger.enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool ("Ground", grounded);

		anim.SetFloat ("vSpeed", character.velocity.y);



		float move = Input.GetAxis ("Horizontal");
		anim.SetFloat ("Speed", Mathf.Abs (move));
		character.velocity = new Vector2 (move * maxSpeed, character.velocity.y);

		if (move > 0 && facingLeft)
			Flip ();
		else if (move < 0 && !facingLeft)
			Flip ();
	}

	void Update(){
		HandleJumping();
		// should not get input directly, should do it in input manager
		if (grounded && Input.GetKeyDown (KeyCode.UpArrow)) {
			anim.SetBool ("Ground", false);
			character.AddForce(new Vector2(0,jumpForce));
			jump = true;
			jumpTime = Time.time;
			isJumping = true;
			hero.layer = LayerMask.NameToLayer("PassThru");
		}

		if (Input.GetKeyDown(KeyCode.DownArrow) && !attacking && !isJumping) {
			Debug.Log ("attack!");
			attacking = true;
			attackTimer = attackCd;

			attackTrigger.enabled = true;
		}

		if (attacking) {
			if(attackTimer > 0){
				attackTimer -= Time.deltaTime;
			}else{
				attacking = false;
				attackTrigger.enabled = false;
			}
		}

		anim.SetBool ("Attacking", attacking);
	}

	void Flip(){
		facingLeft = !facingLeft;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	void HandleJumping()
	{
		// If the player is jumping up, allow it to pass through the Ground layer (by moving the player to the layer "PassThru").
		if (!isJumping || GetComponent<Rigidbody2D>().velocity.y < 0)
		{
			// If player was jumping, move it back to Player layer
			if (Time.time > jumpTime + 0.1f)
			{
				if (hero.layer != LayerMask.NameToLayer("Player"))
				{
					hero.layer = LayerMask.NameToLayer("Player");
				}
			}
		}
	}
}
