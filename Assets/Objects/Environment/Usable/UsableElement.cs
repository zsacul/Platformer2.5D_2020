using System.Collections;
using UnityEngine;

public class UsableElement : EnvironmentElement
{
    public Transform interactionSurroundings;

    public virtual int Player1Use()
    {
        return 0;
    }

    public virtual int Player2Use()
    {
        return 0;
    }
}