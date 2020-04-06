using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawnerScript : MonoBehaviour
{
	public GameObject enemy;
	/*public int lives;
	public float speed;
	public float waitingTimeMin;
	public float waitingTimeMax;
	public Vector3 chasingDist;*/
	
	EnemiesSpotScript[] spots;
	
    void Start()
    {
        spots = FindObjectsOfType<EnemiesSpotScript>();		
		SpawnEnemies();
    }

	void SpawnEnemies()
	{
		Physics.IgnoreLayerCollision(10,10);
		Physics.IgnoreLayerCollision(10,11);
		for(int s = 0; s < spots.Length; s++)
		{
			for(int i = 0; i < spots[s].enemiesNumber; i++)
			{
				float x = spots[s].RandFromRoute();
				float y = spots[s].transform.position.y + spots[s].transform.localScale.y/2 + enemy.transform.localScale.y;
				GameObject e = Instantiate(enemy, new Vector3(x, y, 0f), enemy.transform.rotation);
				e.gameObject.layer = 10;
				EnemyScript es = e.GetComponent<EnemyScript>();
				es.lives = spots[s].lives;
				es.speed = spots[s].speed;
				es.waitingTimeMin = spots[s].waitingTimeMin;
				es.waitingTimeMax = spots[s].waitingTimeMax;
				es.chasingDist = spots[s].chasingDist;
				es.routeStart = spots[s].GetRouteStart();
				es.routeEnd = spots[s].GetRouteEnd();
				es.canOpenDoor = spots[s].canOpenDoor;
				es.canClimbingLadder = spots[s].canClimbingLadder;
			}
		}
	}
}
