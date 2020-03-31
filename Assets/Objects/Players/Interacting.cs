using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacting : MonoBehaviour
{
    protected static Dictionary<int, GameObject> usableElements = new Dictionary<int, GameObject>();
    public KeyCode use;
    public KeyCode barricade;

    public void AddUsableElement(int id, GameObject obj)
    {
        usableElements.Add(id, obj);
        //Debug.Log ("Added element " + usableElements.Count.ToString() + " with id = " + id.ToString());
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.tag == "InteractionSurrounding" && Input.GetKeyDown(use))
        {
            InteractionSurrounding.Type type = other.GetComponent<InteractionSurrounding>().SurroundingType;
            Animator anim = usableElements[other.GetComponent<InteractionSurrounding>().ParentID].GetComponent<Animator>();
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
        if (other.tag == "InteractionSurrounding" && Input.GetKeyDown(barricade))
        {
            InteractionSurrounding.Type type = other.GetComponent<InteractionSurrounding>().SurroundingType;
            Animator anim = usableElements[other.GetComponent<InteractionSurrounding>().ParentID].GetComponent<Animator>();
            switch (type)
            {
                case InteractionSurrounding.Type.leftDoor:
                    anim.SetTrigger("Player2BarricadeLeft");
                    break;
                case InteractionSurrounding.Type.rightDoor:
                    anim.SetTrigger("Player2BarricadeRight");
                    break;
                default:
                    Debug.Log("Unknown surrounding type!!!");
                    break;
            }
        }
    }
}
