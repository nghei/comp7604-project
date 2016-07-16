using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour
{

	protected bool dead = false;
	protected bool grounded = false;
	protected bool canJump = false;
	protected bool isJumping = false;
	protected float jumpTime = 0.0f;
	protected bool facingLeft = false;
	protected bool beingAttacked = false;
	protected bool isDying = false;

	protected Transform groundCheck;

	protected Rigidbody2D body;
	Animator anim;

	public int speed = 1;
	public int damage = 0;
	public int hp = 1;
	public float jumpForce = 200;
	public float attackRange = 0.5f;

	private bool lastJumpFail;
	
	Transform player;

	// Use this for initialization
	protected void Start ()
	{
		body = GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		groundCheck = transform.Find("GroundCheck");
		anim = GetComponent<Animator> ();
	}
	
	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.collider.tag != "Player")
		{
			isJumping = false;
		}
	}

	// Update is called once per frame
	protected void Update ()
	{
		HandleJumping();
		
//		Debug.Log (player.position.x + "," + player.position.y + "," + player.position.z);
		grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
		anim.SetBool ("MGround", grounded);

		if (grounded) {
			canJump = true;
		}

		if(hp <= 0){
			isDying = true;
			anim.SetBool("IsDying", isDying);
		}

	}

	protected bool isPlayerOnLeft() {
		return transform.position.x > player.position.x;
	}

	protected void correctDirection() {
		bool playerOnLeft = isPlayerOnLeft();

		

		if ((Mathf.Abs(transform.position.x - player.position.x) > 0.5) && ((!facingLeft && playerOnLeft) || (facingLeft && !playerOnLeft))) {
			Flip ();
		}
	}

	protected bool isSameGroundLevel() {
		return Mathf.Abs (transform.position.y - player.position.y) < 0.5;
	}

	protected bool isInAttackRange() {
		return Mathf.Abs (transform.position.x - player.position.x) < attackRange && isSameGroundLevel();
	}

	protected void FixedUpdate ()
	{
		
		correctDirection ();

//		int dir = isPlayerOnLeft() ? -1 : 1;
		float hSpeed = transform.localScale.x * speed;
		body.velocity = new Vector2 (hSpeed, body.velocity.y);
		anim.SetFloat ("MSpeed", Mathf.Abs (hSpeed));

		anim.SetFloat ("vMSpeed", body.velocity.y);

//		Debug.Log (transform.position.y + " vs " + player.position.y);

		if (!beingAttacked && !isDying && canJump && Mathf.Abs (transform.position.x - player.position.x) < 1 && transform.position.y + 0.5 < player.position.y) {
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

	//this is used when event is triggered in ZombieHit animation
	protected void BeingAttackDone(){
		beingAttacked = false;
		Debug.Log("done being attacked!");
		anim.SetBool ("BeingAttacked", beingAttacked);
	}

	protected void DyingDone(){
		Destroy(gameObject);
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
		canJump = false;
		jumpTime = Time.time;
		isJumping = true;
		transform.gameObject.layer = LayerMask.NameToLayer("PassThru");
	}

	void HandleJumping()
	{
		// If the BaseEnemy is jumping up, allow it to pass through the Ground layer (by moving the BaseEnemy to the layer "PassThru").
		if (!isJumping || GetComponent<Rigidbody2D>().velocity.y < 0)
		{
			// If BaseEnemy was jumping, move it back to Enemies layer
			if (Time.time > jumpTime + 0.1f)
			{
				if (transform.gameObject.layer != LayerMask.NameToLayer("Enemies"))
				{
					transform.gameObject.layer = LayerMask.NameToLayer("Enemies");
				}
			}
		}
	}

	protected void Damage(int damage){
		
		Debug.Log("WTF Zombie is attacked");
		hp -= damage;
		beingAttacked = true;
		anim.SetBool ("BeingAttacked", beingAttacked);	
		Debug.Log ("BaseEnemyhp: " + hp);
		Debug.Log ("damage: " + damage);
	}
	
}
