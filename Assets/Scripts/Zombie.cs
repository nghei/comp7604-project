using UnityEngine;
using System.Collections;

public class Zombie : BaseEnemy {


	// Use this for initialization
	void Start () {
		base.Start ();
		speed = 1;
		damage = 1;
		hp = 500;
		attackRange = 0.5f;
		// Test for being attacked
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	void Damage(int damage){
		base.Damage(damage);
	}

}
