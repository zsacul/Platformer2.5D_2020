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
	public float chasingSpeed;
	public Vector3 chasingDist;
	
	public enum State {free, waiting, moving, chasing };
    public State state;
	int direction;
	float newX;
	float movingTime;
	float movingTimeMax;
	
	GameObject boy;
	
	Renderer r;
	Rigidbody rb;
    void Start()
    {
		state = State.free;
		r = GetComponent<Renderer>();
		rb = GetComponent<Rigidbody>();
		boy = GameObject.FindWithTag("Player1");
		movingTimeMax = (routeEnd - routeStart)/speed;
    }

	bool StartChase()
	{
		float distX = Mathf.Abs(boy.transform.position.x - transform.position.x);
		float distY = Mathf.Abs(boy.transform.position.y - transform.position.y);
		float distZ = Mathf.Abs(boy.transform.position.z - transform.position.z);
		
		if(distX <= chasingDist.x && distY <= chasingDist.y && distZ <= chasingDist.z)
			return true;
		else
			return false;
	}
	
    void Update()
    {
		if(StartChase())
		{
			state = State.chasing;
			//Debug.Log(state);
		}
		else
		{
			if(state == State.chasing)
				state = State.free;
		}
		
		if(state == State.chasing)
		{
			if(boy.transform.position.x - transform.position.x > 0)
				direction = 1;
			else
				direction = -1;
			
			transform.Translate(new Vector3(direction * chasingSpeed * Time.deltaTime, 0f, 0f));
			//rb.MovePosition(transform.position+  new Vector3(direction * chasingSpeed * Time.deltaTime,0f,0f));
		}
		
		if(state == State.free)
		{
			int randAction = Random.Range(0,4);
			if(randAction == 0)
			{
				state = State.waiting;
				//Debug.Log(state);
				//rb.velocity = new Vector3(0f,0f,0f);
				float t = Random.Range(waitingTimeMin,waitingTimeMax);
				StartCoroutine(WaitInPoint(t));
				
			}
			else
			{
				state = State.moving;
				movingTime = Time.time;
				Debug.Log(state);
				newX = Random.Range(routeStart, routeEnd);
				while(Mathf.Abs(newX - transform.position.x) < (routeEnd - routeStart) / 4f) // distance to walk must be minimum 1/4 of route length
					newX = Random.Range(routeStart, routeEnd);
				//Debug.Log(newX);
				if(newX - transform.position.x > 0)
					direction = 1;
				else
					direction = -1;
			}
		}
		
		if(state == State.moving)
		{
			if(Mathf.Abs(transform.position.x - newX) > 0.5f && Time.time - movingTime < movingTimeMax)
				transform.Translate(new Vector3(direction * speed * Time.deltaTime, 0f, 0f));
				//rb.velocity = new Vector3(direction * speed, 0f,0f);
				//rb.MovePosition(transform.position + new Vector3(direction * speed * Time.deltaTime, 0f, 0f));
			else
				state = State.free;
			
		}
    }
	
	
	IEnumerator WaitInPoint(float t)
	{
		yield return new WaitForSeconds(t);
		state = State.free;
	}	
	
	void OnCollisionEnter(Collision col) 
	{
		if(col.gameObject.CompareTag("Bullet"))
		{
			Destroy(col.gameObject);
			Shot();
			if(lives == 0)
				Death();
		}
		if(col.gameObject.CompareTag("Player1"))
		{
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			state = State.free;
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
