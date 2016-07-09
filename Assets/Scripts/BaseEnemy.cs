using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour
{

	protected bool dead = false;
	protected bool grounded = false;
	protected bool canJump = false;

	public Transform groundCheck;

	protected Rigidbody2D body;

	protected int speed = 1;
	protected int damage = 0;
	protected int hp = 1;
	protected float jumpForce = 200;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	Transform player;

	// Use this for initialization
	protected void Start ()
	{
		body = GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		groundCheck = transform.Find("GroundCheck");
	}
	
	// Update is called once per frame
	protected void Update ()
	{
//		Debug.Log (player.position.x + "," + player.position.y + "," + player.position.z);
		grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

		if (grounded) {
			canJump = true;
		}

	}

	protected void FixedUpdate ()
	{
		
		bool playerOnLeft = transform.position.x > player.position.x;
		int dir = playerOnLeft ? -1 : 1;
		body.velocity = new Vector2 (transform.localScale.x * speed * dir, body.velocity.y);

		if (canJump && Mathf.Abs (transform.position.x - player.position.x) < 0.5) {
			canJump = false;
			Jump ();
		}
	}

	protected void Jump ()
	{
		body.AddForce (new Vector2 (0, jumpForce));
	}


}
