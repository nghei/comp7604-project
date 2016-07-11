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
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();

		if (hp <= 0) {
			Debug.Log ("Zombie Die");
			gameObject.GetComponent<Animation> ().Play ("ZombieDead");
			Destroy (gameObject);
		}
	}

	public void Damage(int damage){
		
		Debug.Log("WTF Zombie is attacked");
		hp -= damage;
		gameObject.GetComponent<Animation> ().Play ("ZombieHit");
		Debug.Log (gameObject);
		Debug.Log ("hp: " + hp);
		Debug.Log ("damage: " + damage);
	}
}
