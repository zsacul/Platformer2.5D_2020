using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interacting : MonoBehaviour
{
    protected static Dictionary<int, GameObject> usableElements = new Dictionary<int, GameObject>();
    public enum ScriptOwnerType { Light, Boy, Soldier };
    //public ScriptOwnerType scriptOwnerType;
    public int scriptOwnerType;
    public KeyCode use;
    public KeyCode barricade;
    public PlayerScript playerScript;
    public FloatingHintBehaviour floatingHintBehaviour;
    bool buttonPressed = false;
    public UnityEvent useDoor;
    public UnityEvent cantUseDoor;

    public void AddUsableElement(int id, GameObject obj)
    {
        usableElements.Add(id, obj);
    }


    private void Update()
    {
        if (Input.GetKeyDown(use))
            buttonPressed = true;

        if (Input.GetKeyUp(use))
            buttonPressed = false;
    }

    private void DoorOpenClose(Animator anim)
    {
        if (anim.GetBool("Closing") == true)
            anim.SetBool("Closing", false);
        else
            anim.SetBool("Closing", true);
    }

    protected void OnTriggerStay(Collider other)
    {
        //Debug.Log(use);

        if (other.tag == "InteractionSurrounding")
        {
            switch ((ScriptOwnerType)scriptOwnerType)
            {
                case ScriptOwnerType.Light:
                    SwiatelkoScript swiatelkoScript = GetComponent<SwiatelkoScript>();
                    swiatelkoScript.inAction = true;
                    LightOnTrigger(other);
                    break;
                case ScriptOwnerType.Boy:
                    BoyOnTrigger(other);
                    break;
                case ScriptOwnerType.Soldier:
                    SoldierOnTrigger(other);
                    break;
            }
        }
        else if (other.tag == "Stairs")
        {
            switch ((ScriptOwnerType)scriptOwnerType)
            {
                case ScriptOwnerType.Light:
                    floatingHintBehaviour.setSprite(1);
                    break;
                case ScriptOwnerType.Boy:
                    floatingHintBehaviour.setSprite(1);
                    break;
                default:
                    break;
            }
        }
        else if (other.tag == "LineZone")
        {
            switch ((ScriptOwnerType)scriptOwnerType)
            {
                case ScriptOwnerType.Light:
                    floatingHintBehaviour.setSprite(3);
                    break;
                default:
                    break;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "InteractionSurrounding" || other.tag == "Stairs" || other.tag == "LineZone")
        {
            switch ((ScriptOwnerType)scriptOwnerType)
            {
                case ScriptOwnerType.Light:
                    SwiatelkoScript swiatelkoScript = GetComponent<SwiatelkoScript>();
                    swiatelkoScript.inAction = false;
                    floatingHintBehaviour.clearSprite();
                    break;
                case ScriptOwnerType.Boy:
                    floatingHintBehaviour.clearSprite();
                    break;
                default:
                    break;
            }
        }
    }
    public int LightOnTrigger(Collider other)
    {
        //Debug.Log("LightOnTrigger" + other.name);

        InteractionSurrounding.Type type = other.GetComponent<InteractionSurrounding>().SurroundingType;
        Animator anim = usableElements[other.GetComponent<InteractionSurrounding>().ParentID].GetComponent<Animator>();
        switch (type)
        {
            case InteractionSurrounding.Type.leftDoor:
                floatingHintBehaviour.setSprite(0);
                break;
            case InteractionSurrounding.Type.rightDoor:
                floatingHintBehaviour.setSprite(0);
                break;
            default:
                break;
        }
        if (buttonPressed)
        {
            buttonPressed = false;
            switch (type)
            {
                case InteractionSurrounding.Type.leftDoor:
                    {
                        useDoor.Invoke();
                        anim.SetTrigger("Player2UseLeft");
                        DoorOpenClose(anim);
                        break;
                    }
                case InteractionSurrounding.Type.rightDoor:
                    {
                        useDoor.Invoke();
                        anim.SetTrigger("Player2UseRight");
                        DoorOpenClose(anim);
                        break;
                    }
                default:
                    Debug.Log("Unknown surrounding type!!!");
                    break;
            }
        }
        /*   else if (Input.GetKeyDown(barricade))// && !buttonPressed)
           {
               //buttonPressed = true;
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
           }*/

        return 0;
    }
    public int BoyOnTrigger(Collider other)
    {
        if (playerScript.inStairs || playerScript.onLine)
            return 0;

        InteractionSurrounding.Type type = other.GetComponent<InteractionSurrounding>().SurroundingType;
        Animator anim = other.gameObject.GetComponentInParent<Animator>();//usableElements[other.GetComponent<InteractionSurrounding>().ParentID].GetComponent<Animator>();


        switch (type)
        {
            case InteractionSurrounding.Type.leftDoor:
                floatingHintBehaviour.setSprite(0);
                break;
            case InteractionSurrounding.Type.rightDoor:
                floatingHintBehaviour.setSprite(0);
                break;
            case InteractionSurrounding.Type.hidingSpotEntrance:
                floatingHintBehaviour.setSprite(3);
                break;
            case InteractionSurrounding.Type.hidingSpotInside:
                floatingHintBehaviour.setSprite(4);
                break;
            default:
                break;
        }


        if (buttonPressed)
        {
            buttonPressed = false;
            switch (type)
            {
                case InteractionSurrounding.Type.leftDoor:
                    {
                        useDoor.Invoke();
                        DoorOpenClose(anim);
                        break;
                    }
                case InteractionSurrounding.Type.rightDoor:
                    {
                        useDoor.Invoke();
                        DoorOpenClose(anim);
                        break;
                    }
                case InteractionSurrounding.Type.hidingSpotEntrance:
                    Debug.Log("Hiding player");
                    playerScript.hidePlayer();
                    anim.SetTrigger("Player1GetInside");
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
    public int SoldierOnTrigger(Collider other)
    {
        InteractionSurrounding.Type type = other.GetComponent<InteractionSurrounding>().SurroundingType;
        Animator anim = usableElements[other.GetComponent<InteractionSurrounding>().ParentID].GetComponent<Animator>();
        switch (type)
        {
            case InteractionSurrounding.Type.leftDoor:
                {
                    useDoor.Invoke();
                    if (anim.GetBool("Closing") == true)
                        anim.SetBool("Closing", false);
                    break;
                }
            case InteractionSurrounding.Type.rightDoor:
                {
                    useDoor.Invoke();
                    if (anim.GetBool("Closing") == true)
                        anim.SetBool("Closing", false);
                    break;
                }
            default:
                //Debug.Log("Unknown surrounding type!!!");
                break;
        }
        return 0;
    }


    public void Use()
    {
        Debug.Log("USING DOOR");
    }
}
