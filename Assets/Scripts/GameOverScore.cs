using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverScore : MonoBehaviour {

	Text scoreText;

	// Use this for initialization
	void Start () {
		scoreText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.text = "Your Score is " + Score.score;
	}
}
