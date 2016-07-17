using UnityEngine;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour
{
	public Vector3 offset;			// The offset at which the Health Bar follows the player.

	public Transform enemy;		// Reference to the player.
	public BaseEnemy enemyControl;


	void Awake ()
	{
		
	}

	void Update ()
	{
		// Set the position to the player's position with the offset.
		try
		{
			transform.position = enemy.position + offset;
		}
		catch (MissingReferenceException e) { }
	}
}
