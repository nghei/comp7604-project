using UnityEngine;
using System.Collections;

public class Megaman : BaseEnemy {

	public Rigidbody2D Bullet;
	public GameObject Firepoint;

	// Use this for initialization
	void Start () {
		base.Start ();
		speed = 1;
		damage = 1;
		hp = 500;
		attackRange = 5.0f;
		// Test for being attacked
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
		if (isInAttackRange ()) {
			Debug.Log("Megaman attacks!");
			Debug.Log(attackRange);
			/*
			Rigidbody2D bullet01 = Instantiate(Bullet, Firepoint.transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;

			if (facingLeft){
				Debug.Log("Bullet prepare to shoot face left!");
				
				bullet01.velocity = new Vector2(-8f, 0);
			}else{
				Debug.Log("Bullet prepare to shoot face right!");
				bullet01.velocity = new Vector2(8f, 0);
			}
			*/

		}	

	}

	void Damage(int damage){
		base.Damage(damage);
	}

	void FixedUpdate() {

		base.FixedUpdate();

	}

}
