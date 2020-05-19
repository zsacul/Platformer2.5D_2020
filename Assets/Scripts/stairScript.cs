using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stairScript : MonoBehaviour
{

    public GameObject other;
    public Animator fadeAnim;

    IEnumerator fadeRoutine(Collider col)
    {
        fadeAnim.Play("screenFadeIn");
        yield return new WaitForSeconds(0.2f);
        Vector3 playerPos = col.gameObject.GetComponent<Transform>().position;
        col.gameObject.GetComponent<Transform>().position = new Vector3(other.transform.position.x, other.transform.position.y, playerPos.z);
    }

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Player1" && Input.GetKey(KeyCode.DownArrow))
        {
            StartCoroutine(fadeRoutine(col));
        }

        if (col.gameObject.tag == "Player2" && Input.GetKey(KeyCode.Q))
        {
            StartCoroutine(fadeRoutine(col));
        }
    }
}
