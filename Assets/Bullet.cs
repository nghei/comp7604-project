using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	
	bool facingLeft;
	public GameObject explosion;
	BoxerControllerScript playerControl;
	public int dmg = 5;


	// Use this for initialization
	void Start () {
		

	}

	void OnExplode(){
			// Create a quaternion with a random rotation in the z-axis.
			Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

			// Instantiate the explosion where the rocket is with the random rotation.
			Instantiate(explosion, transform.position, randomRotation);
	}
	
	void OnTriggerEnter2D (Collider2D col) {
		Debug.Log("Collided");

		// If it hits an enemy...
		if(col.tag == "Player" || col.tag == "Enemy" )
		{
			// ... find the Enemy script and call the Hurt function. ... NOT YET IMPLEMEENTED
			Debug.Log("Prepared to exploded");
			playerControl.Damage(damage);
			// Call the explosion instantiation.
			OnExplode();

			// Destroy the rocket.
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1,1));
		if(transform.position.y > max.x){
			Destroy(gameObject);
		}

	}
}
