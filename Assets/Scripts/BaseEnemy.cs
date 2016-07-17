using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour
{

	protected bool dead = false;
	protected bool grounded = false;
	protected bool canJump = false;
	protected bool isJumping = false;
	protected float jumpTime = 0.0f;
	protected float jumpDecisionTime = 0.0f;
	protected bool facingLeft = false;
	protected bool beingAttacked = false;
	protected bool isDying = false;

	protected float myWidth, myHeight;
	protected float playerWidth, playerHeight;

	protected Transform groundCheck;

	protected Rigidbody2D body;
	Animator anim;

	public int speed = 1;
	public int damage = 0;
	public float hp = 1;
	public float jumpProbMultiplier = 2.0f;
	public float jumpSigma = 10;
	public float jumpCd = 0.1f;
	public float jumpForce = 200;
	public float attackRange = 0.5f;
	public float attackCd = 0.5f;

	private float lastAttackTime = 0.0f;
	private bool lastJumpDecision = false;
	
	Transform player;
	BoxerControllerScript playerControl;

	private Score score;				// get Score script

	// Use this for initialization
	protected void Start ()
	{
		body = GetComponent<Rigidbody2D> ();
		// Don't find player here - Find in Update()
		// player = GameObject.FindGameObjectWithTag ("Player").transform;
		groundCheck = transform.Find("GroundCheck");
		anim = GetComponent<Animator> ();
		Vector3 sz = GetComponent<Renderer>().bounds.size;
		myWidth = sz.x;
		myHeight = sz.y;

		score = GameObject.Find("Score").GetComponent<Score>();
	}
	
	void OnCollisionEnter2D (Collision2D col)
	{
		if(hp <= 0){
			isDying = true;
			anim.SetBool("IsDying", isDying);
		}

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
/*
		if(hp <= 0){
			isDying = true;
			anim.SetBool("IsDying", isDying);
		}
*/
	}

	protected void FindPlayer()
	{
		// Finds the Player GameObject.
		if (player != null)
			return;

		GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
		if (playerObject != null)
		{
			player = playerObject.transform;
			Vector3 sz = playerObject.GetComponent<Renderer>().bounds.size;
			playerWidth = sz.x;
			playerHeight = sz.y;
			playerControl = playerObject.GetComponent<BoxerControllerScript>();
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
		if (player == null)
			return false;
		float distance = Mathf.Abs(transform.position.x - player.position.x) - (myWidth + playerWidth) / 2;
		return distance < attackRange && isSameGroundLevel();
	}

	protected bool ShouldJump()
	{
		float distance = transform.position.x - player.position.x;
		
		if (isJumping || Time.time < jumpDecisionTime + jumpCd)
			return lastJumpDecision;

		if (transform.position.y >= player.position.y)
			return false;

		float prob = jumpProbMultiplier / Mathf.Sqrt(2 * Mathf.PI * jumpSigma * jumpSigma) * Mathf.Exp(-0.5f * distance * distance / (jumpSigma * jumpSigma));
		jumpDecisionTime = Time.time;

		Debug.Log("Prob: " + prob);

		lastJumpDecision = Random.Range(0.0f, 1.0f) <= prob;

		Debug.Log("lastJumpDecision: " + lastJumpDecision);

		return lastJumpDecision;
	}

	protected void FixedUpdate ()
	{
		FindPlayer();

		correctDirection ();

		HandleJumping();

//		int dir = isPlayerOnLeft() ? -1 : 1;
		float hSpeed = transform.localScale.x * speed;
		body.velocity = new Vector2 (hSpeed, body.velocity.y);
		anim.SetFloat ("MSpeed", Mathf.Abs (hSpeed));

		anim.SetFloat ("vMSpeed", body.velocity.y);

//		Debug.Log (transform.position.y + " vs " + player.position.y);

		if (player != null && !beingAttacked && !isDying && canJump && ShouldJump()) {
			canJump = false;
			Jump ();
		}

		if (isInAttackRange() && Time.time >= lastAttackTime + attackCd) {
			Attack ();
			lastAttackTime = Time.time;
		}		

		if(hp <= 0){
			isDying = true;
			anim.SetBool("IsDying", isDying);
		}
	}

	protected void Attack(){
		anim.SetBool("MAttack",true);
	}

	//this is used when event is triggered in ZombieAttack animation
	protected void AttackDone(){
		anim.SetBool ("MAttack", false);
		Debug.Log ("isInAttackRange? " + isInAttackRange());
		if (isInAttackRange ()) {
			playerControl.Damage(damage);
		}
	}

	//this is used when event is triggered in ZombieHit animation
	protected void BeingAttackDone(){
		beingAttacked = false;
		Debug.Log("done being attacked!");
		anim.SetBool ("BeingAttacked", beingAttacked);
	}

	protected void DyingDone(){
		Destroy(gameObject);
		score.score += 100;
	}

	protected void Flip(){
		facingLeft = !facingLeft;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	protected void Jump ()
	{
		if (isJumping || Time.time < jumpTime + jumpCd)
			return;

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
			if (Time.time > jumpTime + jumpCd)
			{
				if (transform.gameObject.layer != LayerMask.NameToLayer("Enemies"))
				{
					transform.gameObject.layer = LayerMask.NameToLayer("Enemies");
				}
			}
		}
	}

	public void Damage(float damage){
		
		Debug.Log("WTF Zombie is attacked");
		hp -= damage;
		beingAttacked = true;
		anim.SetBool ("BeingAttacked", beingAttacked);	
		Debug.Log ("BaseEnemyhp: " + hp);
		Debug.Log ("damage: " + damage);
	}
	
}
