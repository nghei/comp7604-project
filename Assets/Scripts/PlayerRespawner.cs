using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerRespawner : MonoBehaviour
{
	public float respawnTime = 0f;			// Initial respawn time.
	public float respawnTimeIncrement = 1f;		// Increase in respawn time after each death.
	
	public GameObject playerObject;
	private BoxerControllerScript playerControl;	// Player Control.

	public GameObject healthBarObject;

	private bool isRespawnInProgress = false;

	public int respawnLimit = 3;
	private int spawnCount = 0;

	void Start ()
	{
		// Find reference to Player Control script.
		playerControl = playerObject.GetComponent<BoxerControllerScript>();
	}

	void FixedUpdate()
	{
		// If hero died
		if (!isRespawnInProgress && playerControl.IsPlayerDead())
		{
			Debug.Log("Detected player being dead");
			// Set HealthBar to inactive
			healthBarObject.SetActive(false);
			// Calculate new respawn time.
			respawnTime += respawnTimeIncrement;
			Debug.Log("Your player will respawn in " + respawnTime + " seconds.");
			// Respawn
			isRespawnInProgress = true;
			StartCoroutine(Respawn());
		}
		else if (!playerControl.IsPlayerDead())
		{
			isRespawnInProgress = false;
		}
	}

	private IEnumerator Respawn()
	{
		if (spawnCount >= respawnLimit) {
			SceneManager.LoadScene ("Scenes/GameOver");
			yield break;
		}
		spawnCount++;
		yield return new WaitForSeconds(respawnTime);
		Debug.Log("Calling ResetPlayer");
		playerControl.ResetPlayer();
		Debug.Log("Moving Player to the spawn location");
		playerObject.transform.position = transform.position;
		healthBarObject.SetActive(true);
	}

	public int lives() {
		return respawnLimit - spawnCount;
	}
}

