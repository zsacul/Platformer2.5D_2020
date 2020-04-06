using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteractionScript : MonoBehaviour
{
    protected static Dictionary<int, GameObject> usableElements = new Dictionary<int, GameObject>();
    public void AddUsableElement(int id, GameObject obj)
    {
        usableElements.Add(id, obj);
        //Debug.Log ("Added element " + usableElements.Count.ToString() + " with id = " + id.ToString());
    }

    protected void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "InteractionSurrounding")
        {
            InteractionSurrounding.Type type = other.gameObject.GetComponent<InteractionSurrounding>().SurroundingType;
            Animator anim = usableElements[other.gameObject.GetComponent<InteractionSurrounding>().ParentID].GetComponent<Animator>();
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
      
    }
}
