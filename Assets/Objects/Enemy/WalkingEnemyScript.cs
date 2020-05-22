using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WalkingEnemyScript : MonoBehaviour
{
	public int lives;
	public float speed;
	public float chasingSpeed;
	public float climbingSpeed;
	public Vector3 chasingDist;
	public bool onlyPowerBullet;
	
	
	List<Vector3> pointsList = new List<Vector3>();
	public bool cycleWalking;
	public bool patrolAfterEnd;
	
	enum State {chasing, walkingRoute }; //hide in inspector
    State state = State.walkingRoute; //hide in inspector
	
	//Vector3 direction;	
	int currentPoint;
	bool back;
	float angle;
	
	[HideInInspector]
	public bool ladder;
	bool stairs;
	stairScript stScript;
	
	GameObject boy;
	
	Renderer r;
	Rigidbody rb;
	
	void GetPointsList()
	{
		foreach (Transform child in transform) 
             if (child.CompareTag ("Destination")) {
                 pointsList.Add(child.position);
				 Destroy(child.gameObject);
             }
	}

    void Start()
    {
		GetPointsList();
        currentPoint = 0;
		r = GetComponent<Renderer>();
		rb = GetComponent<Rigidbody>();
		boy = GameObject.FindWithTag("Player1");
		Physics.IgnoreCollision(GameObject.FindWithTag("Player2").GetComponent<Collider>(), GetComponent<Collider>());
		GameObject[] walls = GameObject.FindGameObjectsWithTag("CameraWall");
		foreach (GameObject wall in walls)
		{
			//UnityEngine.Debug.Log(wall.transform.position);
			Physics.IgnoreCollision(wall.GetComponent<Collider>(), GetComponent<Collider>());
		}
	}

    // Update is called once per frame
    void Update()
    {
		if(StartChase())
			state = State.chasing;
		else
			state = State.walkingRoute;
		
		if(state == State.walkingRoute)
		{
			if(CheckDist(pointsList[currentPoint], transform.position, false))
			{
				//Debug.Log("in point");
				if(!NextPoint())
				{
					RouteEnd();
				}
			}
			else
			{
				//Debug.Log("moving");
				//MoveToPoint(pointsList[currentPoint]);
				Vector3 point = pointsList[currentPoint];
				if(stairs)
				{
					if(CheckDist(stScript.other.transform.position, point, true))
					{
                        
						transform.position = stScript.other.transform.position;
					}
					//Debug.Log("dest " + point);
					//Debug.Log("stairs" + stScript.other.transform.position);
				}
				if(Mathf.Abs(transform.position.y - point.y) < 0.3f || !ladder)
				{
					SetRotationToPoint(point);
					transform.Translate(Vector3.forward * speed * Time.deltaTime);
				}
				else
				{
					if(ladder)
					{
						transform.eulerAngles = new Vector3(0f, 0f, 0f);
						int direction;
						if (point.y > transform.position.y)
							direction = 1;
						else
							direction = -1;
						transform.Translate(new Vector3(0f, climbingSpeed * Time.deltaTime * direction, 0f));
					}
				}
			}
		}
		
		if(state == State.chasing)
		{
			SetRotationToPoint(boy.transform.position);
			transform.Translate(Vector3.forward * chasingSpeed * Time.deltaTime);
		}
		
	}
	
	void SetRotationToPoint(Vector3 point)
	{
		float xDist = point.x - transform.position.x;
		float zDist = point.z - transform.position.z;
		if(zDist == 0)
		{
			if(xDist > 0)
				angle = 90f;
			else
				angle = -90f;
		}
		else
		{
			angle = Mathf.Atan(xDist/zDist) * Mathf.Rad2Deg;
			if(zDist < 0)
				angle += 180f;
		}
		angle = angle % 360;
		//Debug.Log(angle);
		transform.eulerAngles = new Vector3(0f, angle, 0f);
	}
	
	 void DrawRaysDebug(float rayMult)
    {
        Vector3 rayDirection = transform.forward;
        for (int i = 1; i <= 5; i++) // promienie w lewo
        {
            Vector3 currentRayDirection = new Vector3(rayDirection.x, rayDirection.y, rayDirection.z - rayMult * i);
            Debug.DrawRay(transform.position, currentRayDirection * 20, Color.green);
        }

        Debug.DrawRay(transform.position, transform.forward * 20, Color.green);

        for (int i = 1; i <= 5; i++) // promienie w prawo
        {
            Vector3 currentRayDirection = new Vector3(rayDirection.x, rayDirection.y, rayDirection.z + rayMult * i);
            Debug.DrawRay(transform.position, currentRayDirection * 20, Color.green);
        }
    }


    bool BoyIsSeen()
    {
        RaycastHit hit;
        Vector3 rayDirection = transform.forward;

        float rayMult = 0.05f;

        //DrawRaysDebug(rayMult);

        for(int i=1; i<=5; i++) // promienie w lewo
        {
            Vector3 currentRayDirection = new Vector3(rayDirection.x, rayDirection.y, rayDirection.z - rayMult * i);
            if (Physics.Raycast(transform.position, currentRayDirection, out hit, Mathf.Infinity, LayerMask.GetMask("Player1")))
            {
                return true;
            }
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Player1"))) //promień w przód
        {
            return true;
        }

        for (int i = 1; i <= 5; i++) // promienie w prawo
        {
            Vector3 currentRayDirection = new Vector3(rayDirection.x, rayDirection.y, rayDirection.z + rayMult * i);
            if (Physics.Raycast(transform.position, currentRayDirection, out hit, Mathf.Infinity, LayerMask.GetMask("Player1")))
            {
                return true;
            }
        }

        return false;
    }


	bool StartChase()
	{
		float distX = Mathf.Abs(boy.transform.position.x - transform.position.x);
		float distY = Mathf.Abs(boy.transform.position.y - transform.position.y);
		float distZ = Mathf.Abs(boy.transform.position.z - transform.position.z);

        if (distX <= chasingDist.x && distY <= chasingDist.y && distZ <= chasingDist.z)
        {
            if (BoyIsSeen())
                return true;
            else
                return false;
        }
        else
            return false;
	}
	
	bool CheckDist(Vector3 point, Vector3 point2, bool s)
	{
		float diff = 0.3f;
		if(s)
		{
			//Debug.Log("Checkdist stairs");
			diff = 1.5f;
		}
		bool x = Mathf.Abs(point2.x - point.x) < diff;
		bool y = Mathf.Abs(point2.y - point.y) < diff * 1.5f;
		bool z = Mathf.Abs(point2.z - point.z) < diff;
		if(x && y && z)
			return true;
		else
			return false;
	}
	void MoveToPoint(Vector3 point)
	{
		if(Mathf.Abs(transform.position.y - point.y) < 0.1f)
		{
			SetRotationToPoint(point);
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}
			
	}
	bool NextPoint()
	{
		if(back)
		{
			if(currentPoint > 0)
			{
				currentPoint -=1;
				return true;
			}
			else
			{
				back = false;
			}
		}
		
		if(!back && currentPoint + 1 < pointsList.Count)
		{
			currentPoint += 1;
			return true;
		}
		else
		{
			if(cycleWalking)
			{
				back = true;
				currentPoint -= 1;
				return true;
			}
			else
			{
				return false;
			}
		}
			
		
	}
	
	void RouteEnd()
	{
		if(patrolAfterEnd)
		{
			/*transform.eulerAngles = new Vector3(0f, 0f, 0f);
			EnemyScript e = GetComponent<EnemyScript>();
			e.enabled = true;
			e.lives = lives;
			this.enabled = false;
			*/
			
			pointsList.RemoveRange(0, pointsList.Count-2);
			cycleWalking = true;
			currentPoint = 0;
			back = false;
		}
		//Debug.Log("route end");
	}
			
	void OnCollisionEnter(Collision col) 
	{
		if(col.gameObject.CompareTag("Bullet"))
		{
			Destroy(col.gameObject);
			if(!onlyPowerBullet || (col.gameObject.GetComponent<BulletScript>().power))
				Shot();
			if(lives == 0)
				Death();
		}
		if(col.gameObject.CompareTag("Player1"))
		{
            col.gameObject.GetComponent<PlayerScript>().Cought();
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.CompareTag("Ladder"))
		{
			ladder = true;
			//Debug.Log("ladder");
		}
		if(col.gameObject.CompareTag("Stairs"))
		{
			stairs = true;
			stScript = col.gameObject.GetComponent<stairScript>();
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.CompareTag("Ladder"))
		{
			ladder = false;
			//Debug.Log("not ladder");
		}
		if(col.gameObject.CompareTag("Stairs"))
		{
			stairs = false;
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
	
	public void Death()
	{
		Destroy(this.gameObject);
	}
}