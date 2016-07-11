using UnityEngine;
using System.Collections;

public class EnemySoldier : BaseEnemy {

	// Use this for initialization
	void Start () {
		base.Start ();
		speed = 1;
		damage = 1;
		jumpForce = 150;
		attackRange = 1.5f;
	}

	// Update is called once per frame
	void Update () {
		base.Update ();
	}
}
