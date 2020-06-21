using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHelicopterSpeed : MonoBehaviour
{
    public float speed;
    public GameObject reflector;
    public float zDif;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            reflector.SetActive(true);
            reflector.GetComponent<ReflectorScript>().speed = speed;
            reflector.GetComponent<Transform>().Translate(new Vector3(0f, 0f, zDif));
        }
    }
}
