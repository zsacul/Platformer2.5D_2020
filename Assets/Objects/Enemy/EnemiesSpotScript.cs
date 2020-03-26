using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpotScript : MonoBehaviour
{
	public int enemiesNumber;
	float routeStart = 1f;
	float routeEnd = 5f;
    void Start()
    {
        routeStart = transform.position.x - transform.localScale.x / 2;
		routeEnd = transform.position.x + transform.localScale.x / 2;
		
		//Debug.Log(routeStart);
		//Debug.Log(routeEnd);
		
    }
	
	public float RandFromRoute()
	{
		//Debug.Log(routeStart);
		//Debug.Log(routeEnd);
		float r = Random.Range(routeStart, routeEnd);
		//Debug.Log(r);
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
