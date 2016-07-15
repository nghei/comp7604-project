using UnityEngine;
using System.Collections;

public class Megaman : BaseEnemy {

	public GameObject bullet;
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
	}

	void Damage(int damage){
		base.Damage(damage);
	}

	void FixedUpdate() {

		base.FixedUpdate();

		if (isInAttackRange ()) {
			Debug.Log("Megaman attacks!");
			Debug.Log(attackRange);
			
			if (facingLeft){
				Debug.Log("Bullet prepare to shoot face left!");
				Rigidbody2D bullet01 = Instantiate(bullet, Firepoint.transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bullet01.velocity = new Vector2(8f, 0);
			}else{
				Debug.Log("Bullet prepare to shoot face right!");
				Rigidbody2D bullet01 = Instantiate(bullet, Firepoint.transform.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				bullet01.velocity = new Vector2(-8f, 0);
			}
			

		}	
	}

}
