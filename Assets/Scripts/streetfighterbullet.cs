using UnityEngine;
using System.Collections;

public class streetfighterbullet : MonoBehaviour {

	//public int bulletspeed = 5;
	//bool facingLeft = true;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 3);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D (Collider2D col) 
	{
		// If it hits an enemy...
		if(col.tag == "Enemy")
		{
			// ... find the Enemy script and call the Hurt function.
			//col.gameObject.GetComponent<Enemy>().Hurt();

			// Call the explosion instantiation.
			//OnExplode();

			// Destroy the rocket.
			Destroy (gameObject);
		}
		// Otherwise if it hits a bomb crate...
		else if(col.tag == "BombPickup")
		{
			// ... find the Bomb script and call the Explode function.
			col.gameObject.GetComponent<Bomb>().Explode();

			// Destroy the bomb crate.
			Destroy (col.transform.root.gameObject);

			// Destroy the rocket.
			Destroy (gameObject);
		}
		// Otherwise if the player manages to shoot himself...
		else if(col.gameObject.tag != "Player")
		{
			// Instantiate the explosion and destroy the rocket.
			//OnExplode();
			Destroy (gameObject);
		}
	}
}
