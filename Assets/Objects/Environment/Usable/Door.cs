using UnityEngine;

public class Door : UsableElement
{
    public Transform rightInteractionSurrounding; 
    // interactionSurrounding inherited from UsableElement will be treated as left interaction surrounding
    
    public override int Player1Use()
    {
        return 0;
    }

    public override int Player2Use()
    {
        return 0;
    }
}
