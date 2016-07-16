using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour {

	public int dmg = 20;

	private Transform player;
	private BoxerControllerScript playerController;

	void Start()
	{
/*
		GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
		player = playerObject.transform;
		playerController = playerObject.GetComponent<BoxerControllerScript>();
*/
	}

	void OnTriggerEnter2D(Collider2D col){
		Debug.Log ("Trigger");
		if(col.isTrigger != true && col.CompareTag("Enemy")){
//			if (playerController.isFacingLeft() ^ (player.position.x > col.transform.position.x))
//			{
				col.SendMessageUpwards("Damage", dmg);
				Debug.Log ("AttackTrigger!");
//			}
		}
	}
}
