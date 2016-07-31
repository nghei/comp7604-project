using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayAgainButtonController : MonoBehaviour {

	Button thisButton;

	// Use this for initialization
	void Start () {
		thisButton = GetComponent<Button> ();
		thisButton.onClick.AddListener (() => {
			SceneManager.LoadScene("Scenes/newmap");
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
