using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
	public int lives;
	public float speed;
	public float waitingTimeMin;
	public float waitingTimeMax;
	public float routeStart;
	public float routeEnd;
	
	enum State {free, waiting, moving };
    State state;
	int direction;
	float newX;
	
	Renderer r;
    void Start()
    {
		state = State.free;
		r = GetComponent<Renderer>();
    }

    void Update()
    {
		
		if(state == State.free)
		{
			int randAction = Random.Range(0,4);
			if(randAction == 0)
			{
				state = State.waiting;
				float t = Random.Range(waitingTimeMin,waitingTimeMax);
				StartCoroutine(WaitInPoint(t));
				
			}
			else
			{
				state = State.moving;
				newX = Random.Range(routeStart, routeEnd);
				while(Mathf.Abs(newX - transform.position.x) < (routeEnd - routeStart) / 4f) // distance to walk must be minimum 1/4 of route length
					newX = Random.Range(routeStart, routeEnd);
				if(newX - transform.position.x > 0)
					direction = 1;
				else
					direction = -1;
			}
		}
		
		if(state == State.moving)
		{
			if(Mathf.Abs(transform.position.x - newX) > 0.5f)
				transform.Translate(new Vector3(direction * speed * Time.deltaTime, 0f, 0f));
			else
				state = State.free;
			
		}
    }
	
	
	IEnumerator WaitInPoint(float t)
	{
		yield return new WaitForSeconds(t);
		state = State.free;
	}	
	
	public void OnTriggerEnter(Collider col) 
	{
		if(col.gameObject.CompareTag("Bullet"))
		{
			Destroy(col.gameObject);
			Shot();
			if(lives == 0)
				Death();
		}
		if(col.gameObject.CompareTag("Player1") || col.gameObject.CompareTag("Player2"))
		{
			//SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}
	
	IEnumerator ChangeColorForTime(float time, Color color)
	{
		Color prevColor = r.material.color;
		r.material.color = color;
		yield return new WaitForSeconds(time);
		r.material.color = prevColor;
	}
	
	void Shot()
	{
		lives--;
		StartCoroutine(ChangeColorForTime(1f, Color.red));
	}
	
	void Death()
	{
		Destroy(this.gameObject);
	}
}
