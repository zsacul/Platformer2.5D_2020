using System.Collections;
using UnityEngine;

public class UsableElement : EnvironmentElement
{
    public Transform interactionSurroundings;

    protected UsableElement ()//: base()
    {}

    protected virtual void Start ()
    {
         GameObject.FindWithTag("Player2").GetComponent<Interacting>().AddUsableElement(GetId(), transform.gameObject);
    }

    public virtual int Player1Use ()
    {
        return 0;
    }
    public virtual int Player2Use ()
    {
        return 0;
    }

}