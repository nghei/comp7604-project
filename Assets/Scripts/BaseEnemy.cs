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
	protected bool isAttacking = false;

	protected float myWidth, myHeight;
	protected float playerWidth, playerHeight;

	protected Transform groundCheck;

	protected Rigidbody2D body;
	Animator anim;

	public int speed = 1;
	public float damage = 0;
	public float hp = 1;
	public float jumpForce = 200;
	public float attackRange = 0.5f;
	public float attackCd = 0.5f;

	protected float lastAttackTime = 0.0f;
	protected bool lastJumpFail;
	
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


		// Debug.Log("isInAttackRange: ");
		// Debug.Log(isInAttackRange());

		if (isInAttackRange() && Time.time >= lastAttackTime + attackCd) {
			Attack ();
			lastAttackTime = Time.time;
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
		// Debug.Log("Distnce between BaseEnemy and Player: "+distance+" attackRange: "+attackRange);
		return distance < attackRange && isSameGroundLevel();
	}

	protected void FixedUpdate ()
	{
		
		FindPlayer();

		

		HandleJumping();

//		int dir = isPlayerOnLeft() ? -1 : 1;
		float hSpeed;
		if(beingAttacked || isAttacking || isDying){
			hSpeed = 0;
		}else{
			hSpeed = transform.localScale.x * speed;
			correctDirection ();
		}
		
		body.velocity = new Vector2 (hSpeed, body.velocity.y);
		anim.SetFloat ("MSpeed", Mathf.Abs (hSpeed));

		anim.SetFloat ("vMSpeed", body.velocity.y);

//		Debug.Log (transform.position.y + " vs " + player.position.y);
	
		bool shouldJump = (Mathf.Abs (transform.position.x - player.position.x) < 5 && transform.position.y + 0.5 < player.position.y);

		// Debug.Log("BaseEnemy position x: "+ transform.position.x +" Player Position x: " +player.position.x + "BaseEnemy position y: "+ transform.position.y +" Player Position y: " +player.position.y);
		// Debug.Log("shouldJump?: " + shouldJump);

		if (player != null && !beingAttacked && !isDying && canJump && shouldJump) {
			canJump = false;
			Debug.Log("BaseEnemy Jump!");
			Jump ();
		}


		if(hp <= 0){
			isDying = true;
			anim.SetBool("IsDying", isDying);
		}
	}

	protected virtual void Attack(){
		isAttacking = true;
		anim.SetBool("MAttack",isAttacking);
		playerControl.Damage(damage);
		Debug.Log("BaseEnemy Attacks!");

	}

	//this is used when event is triggered in ZombieAttack animation
	protected void AttackDone(){
		isAttacking = false;
		anim.SetBool ("MAttack", isAttacking	);
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

	public void Damage(float damage){
	
		hp -= damage;
		beingAttacked = true;
		anim.SetBool ("BeingAttacked", beingAttacked);	
		//Debug.Log ("BaseEnemyhp: " + hp);
		//Debug.Log ("damage: " + damage);
	}
	
}
