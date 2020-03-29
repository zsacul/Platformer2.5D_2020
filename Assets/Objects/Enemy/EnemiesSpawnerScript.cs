using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawnerScript : MonoBehaviour
{
	public GameObject enemy;
	public int lives;
	public float speed;
	public float waitingTimeMin;
	public float waitingTimeMax;
	public Vector3 chasingDist;
	
	EnemiesSpotScript[] spots;
	
    void Start()
    {
        spots = FindObjectsOfType<EnemiesSpotScript>();
		//Debug.Log(spots.Length);
		//Debug.Log(spots[0].routeStart);
		//Debug.Log(spots[0].routeEnd);
		
		SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void SpawnEnemies()
	{
		Physics.IgnoreLayerCollision(10,10);
		for(int s = 0; s < spots.Length; s++)
		{
			for(int i = 0; i < spots[s].enemiesNumber; i++)
			{
				//Debug.Log(spots[s].routeStart);
				//Debug.Log(spots[s].routeEnd);
				float x = spots[s].RandFromRoute();
				float y = spots[s].transform.position.y + spots[s].transform.localScale.y/2 + enemy.transform.localScale.y;
				GameObject e = Instantiate(enemy, new Vector3(x, y, 0f), enemy.transform.rotation);
				e.gameObject.layer = 10;
				EnemyScript es = e.GetComponent<EnemyScript>();
				es.lives = lives;
				es.speed = speed;
				es.waitingTimeMin = waitingTimeMin;
				es.waitingTimeMax = waitingTimeMax;
				es.chasingDist = chasingDist;
				es.routeStart = spots[s].GetRouteStart();
				es.routeEnd = spots[s].GetRouteEnd();
				
				//Debug.Log(es.routeStart);
				//Debug.Log(es.routeEnd);
			}
		}
	}
}
