using UnityEngine;
using System.Collections;

public class BoxerControllerScript : MonoBehaviour {

	[HideInInspector]
	public bool jump = false;				// Condition for whether the player can jump.
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

	public float maxHp = 100;
	private float attackTimer = 0;
	public float attackCd = 0.3f;

	private float hp;

	public Collider2D attackTrigger;

	private SpriteRenderer healthBar;
	private Vector3 healthScale;

	public Rigidbody2D sfbullet;

	void Awake(){
	}

	// Use this for initialization
	void Start () {
		// Setting up references.
		hero = GameObject.FindGameObjectWithTag("Player");
		character = GetComponent<Rigidbody2D>();
		healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();
		healthScale = healthBar.transform.localScale;
		anim = GetComponent<Animator> ();
		attackTrigger.enabled = false;
		// Reset player.
		ResetPlayer();
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

		if (jump)
		{
			anim.SetBool ("Ground", false);
			character.AddForce(new Vector2(0, jumpForce));
			jump = false;
		}
		
	}

	void Update()
	{
		HandleJumping();
		
		// should not get input directly, should do it in input manager
		if (!isJumping || GetComponent<Rigidbody2D>().velocity.y < 0)
		{
			if (grounded && !attacking && Input.GetKeyDown (KeyCode.UpArrow)) {
				jump = true;
				jumpTime = Time.time;
				isJumping = true;
				hero.layer = LayerMask.NameToLayer("PassThru");
			}

			if (Input.GetKeyDown(KeyCode.DownArrow) && !attacking) {
				Debug.Log ("attack!");
				attacking = true;
				attackTimer = attackCd;

				//attackTrigger.enabled = true;	
				Rigidbody2D bulletInstance = Instantiate(sfbullet, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				if (facingLeft) {
					bulletInstance.velocity = new Vector2 (-8f, 0);
					Vector3 theScale = bulletInstance.transform.localScale;
					theScale.x *= -1;
					bulletInstance.transform.localScale = theScale;


				} else {
					bulletInstance.velocity = new Vector2(8f, 0);
				}



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
	}
	
	void OnCollisionEnter2D (Collision2D col)
	{
		Debug.Log(col.collider.tag);
		isJumping = false;
	}

	public bool isFacingLeft()
	{
		return facingLeft;
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

	public void Damage(float damage)
	{
		hp -= damage;
		UpdateHealthBar();
	}

	public void UpdateHealthBar ()
	{
		// Set the health bar's colour to proportion of the way between green and red based on the player's health.
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - hp * 0.01f);

		// Set the scale of the health bar to be proportional to the player's health.
		healthBar.transform.localScale = new Vector3(healthScale.x * hp * 0.01f, 1, 1);
	}

	public bool IsPlayerDead()
	{
		return hp <= 0;
	}

	public void KillPlayer()
	{
		hp = 0;
	}

	public void ResetPlayer()
	{
		hp = maxHp;
		Debug.Log("HP: " + hp);
		UpdateHealthBar();
	}

	public float getHP() {
		return hp;
	}

}
