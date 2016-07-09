using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour {

	public float maxSpeed = 10f;
	bool facingLeft = false;

	public Rigidbody2D character;
	Animator anim;
	bool grounded;
	//bool friendly = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	public float jumpForce = 600;

	void Awake(){
		character = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();

	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void FixedUpdate () {
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool ("MGround", grounded);
		Debug.Log (grounded);
		anim.SetFloat ("vMSpeed", character.velocity.y);



		float move = Input.GetAxis ("Horizontal");
		anim.SetFloat ("MSpeed", Mathf.Abs (move));
		character.velocity = new Vector2 (move * maxSpeed, character.velocity.y);

		if (move > 0 && facingLeft)
			Flip ();
		else if (move < 0 && !facingLeft)
			Flip ();
	}
		
	void Update(){
		// should not get input directly, should do it in input manager
		if (grounded && Input.GetKeyDown (KeyCode.UpArrow)) {
			anim.SetBool ("MGround", false);
			character.AddForce(new Vector2(0,jumpForce));

		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			Attack();
		}

	}

	void Flip(){
		facingLeft = !facingLeft;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void Attack(){
		anim.SetBool("MAttack",true);

	}
	//this is used when event is triggered in ZombieAttack animation
	void AttackDone(){
		anim.SetBool ("MAttack", false);
	}

}
