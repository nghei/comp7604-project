using UnityEngine;
using System.Collections;

public class Charizard : BaseEnemy {

	public Rigidbody2D Bullet;
	public GameObject Firepoint;
	public GameObject explosion;
	public float bullet_speed = 8;
	
	

	// Use this for initialization
	void Start () {
		base.Start ();

		// Test for being attacked
	}

	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	void Damage(int damage){
		base.Damage(damage);
	}

	void FixedUpdate() {
		base.FixedUpdate();

	}

	protected override void Attack(){

 		Debug.Log("Charizard attacks!");

 		base.Attack();

		
		Debug.Log(attackRange);
		
		Rigidbody2D bullet01 = Instantiate(Bullet, Firepoint.transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;

		if (facingLeft){
			Debug.Log("Bullet prepare to shoot face left!");
			
			bullet01.velocity = new Vector2(-bullet_speed, 0);
		}else{
			Debug.Log("Bullet prepare to shoot face right!");
			bullet01.velocity = new Vector2(bullet_speed, 0);
		}



	}
}
