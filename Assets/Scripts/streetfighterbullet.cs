using UnityEngine;
using System.Collections;

public class StreetFighterBullet : MonoBehaviour {

	public float appearTime = 3f;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, appearTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D (Collider2D col) 
	{
		// If it hits an enemy...
		if (col.tag == "Enemy")
		{
			// Destroy the bullet.
			Destroy (gameObject);
		}
		// Otherwise if the player manages to shoot himself...
		else if (col.gameObject.tag != "Player")
		{
			// Destroy the bullet.
			Destroy (gameObject);
		}
	}
}
