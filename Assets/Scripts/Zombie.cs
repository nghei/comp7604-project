﻿using UnityEngine;
using System.Collections;

public class Zombie : BaseEnemy {

	// Use this for initialization
	void Start () {
		base.Start ();
		speed = 1;
		damage = 1;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}
}