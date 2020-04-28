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

    bool BoyIsSeen()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Player1")))
        {
            return true;
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
		transform.position = new Vector3 (transform.position.x, transform.position.y, 0f);
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
