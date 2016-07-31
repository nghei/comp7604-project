using UnityEngine;
using System.Collections;

public class UIPlayerLives : MonoBehaviour {

	public PlayerRespawner playerRespwaner;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<GUIText> ().text = "Remaining Lives: " + playerRespwaner.lives ();
	}
}
