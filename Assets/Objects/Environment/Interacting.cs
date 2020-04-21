using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacting : MonoBehaviour
{
    protected static Dictionary<int, GameObject> usableElements = new Dictionary<int, GameObject>();
    public enum ScriptOwnerType {Light, Boy, Soldier};
    //public ScriptOwnerType scriptOwnerType;
    public int scriptOwnerType;
    public KeyCode use;
    public KeyCode barricade;
    public PlayerScript playerScript;

    public void AddUsableElement(int id, GameObject obj)
    {
        usableElements.Add(id, obj);
        Debug.Log ("Added element " + usableElements.Count.ToString() + " with id = " + id.ToString());
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.tag == "InteractionSurrounding")
        {
            switch ((ScriptOwnerType)scriptOwnerType)
            {
                case ScriptOwnerType.Light:
                    LightOnTrigger (other);
                    break;
                case ScriptOwnerType.Boy:
                    BoyOnTrigger (other);
                    break;
                case ScriptOwnerType.Soldier:
                    SoldierOnTrigger (other);
                    break;
            }
        }
    }
    public int LightOnTrigger (Collider other)
    {
        InteractionSurrounding.Type type = other.GetComponent<InteractionSurrounding>().SurroundingType;
        Animator anim = usableElements[other.GetComponent<InteractionSurrounding>().ParentID].GetComponent<Animator>();
        if (Input.GetKeyDown(use))
        {
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
        }
        if (Input.GetKeyDown(barricade))
        {
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
        return 0;
    }
    public int BoyOnTrigger (Collider other)
    {
        if (Input.GetKeyDown(use))
        {
            InteractionSurrounding.Type type = other.GetComponent<InteractionSurrounding>().SurroundingType;
            Animator anim = usableElements[other.GetComponent<InteractionSurrounding>().ParentID].GetComponent<Animator>();
            switch (type)
            {
                case InteractionSurrounding.Type.leftDoor:
                    anim.SetTrigger("Player1UseLeft");
                    break;
                case InteractionSurrounding.Type.rightDoor:
                    anim.SetTrigger("Player1UseRight");
                    break;
                case InteractionSurrounding.Type.hidingSpotEntrance:
                    if (!playerScript.isSeen)
                        {
                            Debug.Log("Hiding player");
                            playerScript.hidePlayer();
                            anim.SetTrigger("Player1GetInside");
                        }
                    break;
                case InteractionSurrounding.Type.hidingSpotInside:
                    Debug.Log("Unhiding player");
                    playerScript.unhidePlayer();
                    anim.SetTrigger("Player1GetOut");
                    break;
                default:
                    Debug.Log("Unknown surrounding type!!!");
                    break;
            }
        }
        return 0;
    }
    public int SoldierOnTrigger (Collider other)
    {
        InteractionSurrounding.Type type = other.GetComponent<InteractionSurrounding>().SurroundingType;
        Animator anim = usableElements[other.GetComponent<InteractionSurrounding>().ParentID].GetComponent<Animator>();
        switch (type)
        {
            case InteractionSurrounding.Type.leftDoor:
                anim.SetTrigger("EnemyUseLeft");
                break;
            case InteractionSurrounding.Type.rightDoor:
                anim.SetTrigger("EnemyUseRight");
                break;
            default:
                Debug.Log("Unknown surrounding type!!!");
                break;
        }
        return 0;
    }
}
