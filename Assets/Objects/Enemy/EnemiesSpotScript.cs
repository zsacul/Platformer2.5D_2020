using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpotScript : MonoBehaviour
{
	public int enemiesNumber;
	public int lives;
	public float speed;
	public float waitingTimeMin;
	public float waitingTimeMax;
	public Vector3 chasingDist;
	public float chasingSpeed;
	public bool canOpenDoor;
	public bool canClimbingLadder;
	float routeStart;
	float routeEnd;
    void Awake()
    {
        routeStart = transform.position.x - transform.localScale.x / 2;
		routeEnd = transform.position.x + transform.localScale.x / 2;		
    }
	
	public float RandFromRoute()
	{
		float r = Random.Range(routeStart, routeEnd);
		return r;
	}
	
	public float GetRouteStart()
	{
		return routeStart;
	}
	
	public float GetRouteEnd()
	{
		return routeEnd;
	}
	
}
