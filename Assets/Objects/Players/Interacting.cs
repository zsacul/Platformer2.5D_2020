using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacting : MonoBehaviour
{
    protected static Dictionary<int, GameObject> usableElements = new Dictionary<int, GameObject>();
    public KeyCode use;

    public void AddUsableElement (int id, GameObject obj)
    {
        usableElements.Add(id, obj);
        //Debug.Log ("Added element " + usableElements.Count.ToString() + " with id = " + id.ToString());
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.tag == "InteractionSurrounding" && Input.GetKeyDown(use))
        {
            Animator anim = usableElements[other.GetComponent<InteractionSurrounding>().ParentID].GetComponent<Animator>();
            anim.SetTrigger ("TryOpening");
        }
    }    
}
