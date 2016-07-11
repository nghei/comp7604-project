using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour {

	public int dmg = 20;

	void OnTriggerEnter2D(Collider2D col){
		Debug.Log ("Trigger");
		if(col.isTrigger != true && col.CompareTag("Enemy")){
			col.SendMessageUpwards("Damage", dmg);
			Debug.Log ("AttackTrigger!");
		}
	}
}
