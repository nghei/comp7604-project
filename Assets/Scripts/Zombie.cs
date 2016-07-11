using UnityEngine;
using System.Collections;

public class Zombie : BaseEnemy {

	// Use this for initialization
	void Start () {
		base.Start ();
		speed = 1;
		damage = 1;
		hp = 25;
		attackRange = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();

		if (hp <= 0) {
			Destroy (gameObject);
		}
	}

	public void Damage(int damage){
		Debug.Log("Zombie is attacked");
		hp -= damage;
		gameObject.GetComponent<Animation> ().Play ("ZombieHit");
	}
}
