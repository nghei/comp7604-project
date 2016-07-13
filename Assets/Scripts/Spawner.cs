using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public float spawnTime = 5f;		// The amount of time between each spawn.
	public float spawnDelay = 3f;		// The amount of time before spawning starts.
	public GameObject[] enemies;		// Array of enemy prefabs.

	public int maxInstances = 10;		// Default maximum number of instances on the map.

	private GameObject[] enemyInstances = null;	// Array of pre-instantiated enemy instances.

	void Start ()
	{
		// Pre-instantiate.
		PreInstantiate();
		// Start calling the Spawn function repeatedly after a delay .
		InvokeRepeating("Spawn", spawnDelay, spawnTime);
	}


	private void PreInstantiate()
	{
		// Pre-instantiate instances on the map.
		if (enemyInstances == null)
		{
			enemyInstances = new GameObject[maxInstances];
		}
		for (int i = 0; i < maxInstances; ++i)
		{
			if (enemyInstances[i] == null)
			{
				int enemyIndex = Random.Range(0, enemies.Length);
				enemyInstances[i] = Instantiate(enemies[enemyIndex], transform.position, transform.rotation) as GameObject;
				enemyInstances[i].SetActive(false);
			}
		}
	}

	private int GetFirstInactive()
	{
		PreInstantiate();
		for (int i = 0; i < maxInstances; ++i)
		{
			if (!enemyInstances[i].activeSelf)
			{
				return i;
			}
		}
		return -1;
	}

	void Spawn ()
	{
		// If some enemy is inactive, then spawn
		int inactiveIndex = GetFirstInactive();
		if (inactiveIndex >= 0)
		{
			enemyInstances[inactiveIndex].transform.position = transform.position;
			enemyInstances[inactiveIndex].SetActive(true);
		}

		/*
		// Play the spawning effect from all of the particle systems.
		foreach(ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
		{
			p.Play();
		}
		*/
	}
}
