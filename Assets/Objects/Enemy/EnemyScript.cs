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
    public bool canOpenDoor;
    public bool canClimbingLadder;
    public bool onlyPowerBullet;
    public Vector3 chasingDist;

    public enum State { free, waiting, moving, chasing, climbing };
    public State state;
    int direction;
    float newX;
    float movingTime;
    float movingTimeMax;

    [HideInInspector]
    public bool ladder;

    GameObject boy;

    Renderer r;
    Rigidbody rb;


    protected static Dictionary<int, GameObject> usableElements = new Dictionary<int, GameObject>();
    public void AddUsableElement(int id, GameObject obj)
    {
        usableElements.Add(id, obj);
        //Debug.Log ("Added element " + usableElements.Count.ToString() + " with id = " + id.ToString());
    }


    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        SetRotation();
        state = State.free;
        r = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        boy = GameObject.FindWithTag("Player1");
        Physics.IgnoreCollision(GameObject.FindWithTag("Player2").GetComponent<Collider>(), GetComponent<Collider>());
        movingTimeMax = (routeEnd - routeStart) / speed;
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
                if(hit.collider.gameObject.tag == "Player1")
                    return true;
            }
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Player1"))) //promień w przód
        {
            if (hit.collider.gameObject.tag == "Player1")
                return true;
        }

        for (int i = 1; i <= 5; i++) // promienie w prawo
        {
            Vector3 currentRayDirection = new Vector3(rayDirection.x, rayDirection.y, rayDirection.z + rayMult * i);
            if (Physics.Raycast(transform.position, currentRayDirection, out hit, Mathf.Infinity, LayerMask.GetMask("Player1")))
            {
                if (hit.collider.gameObject.tag == "Player1")
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
        if (BoyIsSeen())
        {
            return true;
            if (distX <= chasingDist.x && distY <= chasingDist.y && distZ <= 0.2f)
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    bool StartClimbing()
    {
        float distX = Mathf.Abs(boy.transform.position.x - transform.position.x);
        float distY = Mathf.Abs(boy.transform.position.y - transform.position.y);
        float distZ = Mathf.Abs(boy.transform.position.z - transform.position.z);
        if (distX <= chasingDist.x && distY <= chasingDist.y && distZ <= chasingDist.z && canClimbingLadder && ladder && distY > 0.1f)
            return true;
        else
            return false;
    }


    void SetRotation()
    {
        transform.eulerAngles = new Vector3(0f, direction * 90f, 0f);
		//transform.position = new Vector3 (transform.position.x, transform.position.y, 0f);
	}

    void Update()
    {
        if (StartClimbing())
        {
            if (boy.transform.position.y > transform.position.y)
                direction = 1;
            if (boy.transform.position.y < transform.position.y)
                direction = -1;
            state = State.climbing;
            //Debug.Log(state);
        }
        else
        {
            if (StartChase())
            {
                state = State.chasing;
            }
            else
            {
                if (state == State.chasing)
                    state = State.free;
            }
        }

        if (state == State.climbing)
        {
            transform.Translate(new Vector3(0f, direction * speed * Time.deltaTime, 0f));
        }

        if (state == State.chasing)
        {
            if (boy.transform.position.x - transform.position.x > 0.1f)
                direction = 1;
            else
            {
                if (boy.transform.position.x - transform.position.x < -0.1f)
                    direction = -1;
                else
                    direction = 0;
            }
            transform.Translate(Vector3.forward * chasingSpeed * Time.deltaTime /*new Vector3(direction * chasingSpeed * Time.deltaTime, 0f, 0f)*/);
        }

        if (state == State.free)
        {
            int randAction = Random.Range(0, 4);
            if (randAction == 0)
            {
                state = State.waiting;
                float t = Random.Range(waitingTimeMin, waitingTimeMax);
                StartCoroutine(WaitInPoint(t));
            }
            else
            {
                state = State.moving;
                movingTime = Time.time;
                newX = Random.Range(routeStart, routeEnd);
                while (Mathf.Abs(newX - transform.position.x) < (routeEnd - routeStart) / 4f) // distance to walk must be minimum 1/4 of route length
                    newX = Random.Range(routeStart, routeEnd);
                if (newX - transform.position.x > 0)
                    direction = 1;
                else
                    direction = -1;
            }
        }

        if (state == State.moving)
        {
            if (Mathf.Abs(transform.position.x - newX) > 0.5f && Time.time - movingTime < movingTimeMax)
                transform.Translate(Vector3.forward * chasingSpeed * Time.deltaTime/*new Vector3(direction *speed * Time.deltaTime, 0f, 0f)*/);
            else
                state = State.free;
        }
        SetRotation();
    }

    IEnumerator WaitInPoint(float t)
    {
        yield return new WaitForSeconds(t);
        state = State.free;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            Destroy(col.gameObject);
            if (!onlyPowerBullet || (col.gameObject.GetComponent<BulletScript>().power))
                Shot();
            if (lives == 0)
                Death();
        }
        if (col.gameObject.CompareTag("Player1"))
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            col.gameObject.GetComponent<PlayerScript>().Cought();
            state = State.free;
        }
        if (col.gameObject.tag == "InteractionSurrounding" && canOpenDoor)
        {
            InteractionSurrounding.Type type = col.gameObject.GetComponent<InteractionSurrounding>().SurroundingType;
            Animator anim = usableElements[col.gameObject.GetComponent<InteractionSurrounding>().ParentID].GetComponent<Animator>();
            switch (type)
            {
                case InteractionSurrounding.Type.leftDoor:
                    anim.SetTrigger("Player2UseLeft");
                    break;
                case InteractionSurrounding.Type.rightDoor:
                    anim.SetTrigger("Player2UseRight");
                    break;
                default:
                    Debug.Log("Unknown surrounding type!!!");
                    break;
            }
            // Animator anim = usableElements[other.GetComponent<InteractionSurrounding>().ParentID].GetComponent<Animator>();
            // anim.SetTrigger ("Player2Use");
        }
        if (col.gameObject.CompareTag("Ladder"))
        {
            ladder = true;
            Debug.Log("ladder");
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Ladder"))
        {
            ladder = false;
            Debug.Log("not ladder");
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
