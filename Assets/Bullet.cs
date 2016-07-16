using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	
	bool facingLeft;


	// Use this for initialization
	void Start () {
		
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1,1));
		if(transform.position.y > max.x){
			Destroy(gameObject);
		}

	}
}
