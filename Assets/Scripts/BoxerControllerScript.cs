using UnityEngine;
using System.Collections;

public class BoxerControllerScript : MonoBehaviour {

	public float maxSpeed = 10f;
	bool facingLeft = true;

	public Rigidbody2D character;
	Animator anim;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	public float jumpForce = 5000;

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
		// should not get input directly, should do it in input manager
		if (grounded && Input.GetKeyDown (KeyCode.UpArrow)) {
			anim.SetBool ("Ground", false);
			character.AddForce(new Vector2(0,jumpForce));
			
		}
	}

	void Flip(){
		facingLeft = !facingLeft;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
