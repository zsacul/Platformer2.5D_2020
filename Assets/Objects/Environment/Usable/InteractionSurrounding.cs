using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSurrounding : MonoBehaviour
{
    public enum Type {normal, leftDoor, rightDoor};
    private int parentID; 
    public int ParentID
    {
        get {return parentID;}
        set {parentID = value;}
    }
    private Type surrType;
    public Type SurroundingType
    {
        get {return surrType;}
        set {surrType = value;}
    }
}
