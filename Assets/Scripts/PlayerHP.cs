using UnityEngine;
using System.Collections;

public class PlayerHP : MonoBehaviour
{
	private BoxerControllerScript playerControl;	// get boxer controller script

	void Awake ()
	{
		// Setting up the reference.
		playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxerControllerScript>();
	}


	void Update ()
	{
		// Set the score text.
		GetComponent<GUIText>().text = "HP: " + Mathf.FloorToInt(playerControl.getHP());
	}

}
