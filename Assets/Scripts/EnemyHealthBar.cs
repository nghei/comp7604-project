using UnityEngine;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour
{
	public Vector3 offset;			// The offset at which the Health Bar follows the player.

	public Transform enemy;		// Reference to the enemy.
	public BaseEnemy enemyControl;
	public GameObject healthBarObj;

	private SpriteRenderer healthBar;
	private SpriteRenderer healthOutline;

	float originalXScale;

	void Awake ()
	{
		healthBar = transform.Find("EnemyHealthBar").GetComponent<SpriteRenderer> ();
		healthOutline = transform.Find("HealthOutline").GetComponent<SpriteRenderer> ();
		originalXScale = healthBar.transform.localScale.x;
	}

	void Update ()
	{
		
		// Set the position to the player's position with the offset.
		try
		{
			float health = (float) enemyControl.hp / enemyControl.maxHp;
			healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health);
			healthBar.transform.localScale = new Vector3(originalXScale * health, 1, 1); 
			transform.position = enemy.position + offset;
			if (health <= 0)
			{
				// Set health bar size to 0
				healthBarObj.transform.localScale = new Vector3(0, 0, 0);
			}
		}
		catch (MissingReferenceException e) {
			Destroy (healthBarObj);
		}
	}
}
