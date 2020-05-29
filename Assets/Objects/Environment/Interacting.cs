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
    bool buttonPressed = false;

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
                    SwiatelkoScript swiatelkoScript = GetComponent<SwiatelkoScript>();
                    swiatelkoScript.inAction = true;
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

    protected void OnTriggerExit(Collider other)
    {
        if (other.tag == "InteractionSurrounding")
        {
            switch ((ScriptOwnerType)scriptOwnerType)
            {
                case ScriptOwnerType.Light:
                    SwiatelkoScript swiatelkoScript = GetComponent<SwiatelkoScript>();
                    swiatelkoScript.inAction = false;
                    break;
            }
        }
    }
    public int LightOnTrigger (Collider other)
    {
        InteractionSurrounding.Type type = other.GetComponent<InteractionSurrounding>().SurroundingType;
        Animator anim = usableElements[other.GetComponent<InteractionSurrounding>().ParentID].GetComponent<Animator>();
        if (Input.GetKeyDown(use) && !buttonPressed)
        {
            buttonPressed = true;
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
        else if (Input.GetKeyDown(barricade) && !buttonPressed)
        {
            buttonPressed = true;
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
        else if (!Input.GetKeyDown(use) && !Input.GetKeyDown(barricade))
            buttonPressed = false;
        return 0;
    }
    public int BoyOnTrigger (Collider other)
    {
        if (playerScript.inStairs || playerScript.onLine)
            return 0;

        if (Input.GetKeyDown(use) && !buttonPressed)
        {
            buttonPressed = true;
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
        else if (!Input.GetKeyDown(use))
            buttonPressed = false;
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
