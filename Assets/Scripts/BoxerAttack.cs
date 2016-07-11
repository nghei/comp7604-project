/*using UnityEngine;
using System.Collections;

public class BoxerAttack : MonoBehaviour {
	private bool attacking = false;

	private float attackTimer = 0;
	private float attackCd = 0.3f;

	public Collider2D attackTrigger;

	private Animator anim;

	void Awake(){
		anim = GetComponent<Animator> ();
		attackTrigger = GetComponent<Collider2D> ()
		attackTrigger.enabled = false;
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.DownArrow) && !attacking) {
			attacking = true;
			attackTimer = attackCd;

			attackTrigger.enabled = true;
		}

		if (attacking) {
			if(attackTimer > 0){
				attackTimer -= Time.deltaTime;
			}else{
				attacking = false;
				attackTrigger.enabled = false;
			}
		}

		anim.SetBool ("Attacking", attacking);
	
	}

}*/
