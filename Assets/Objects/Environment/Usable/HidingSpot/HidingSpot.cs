using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : UsableElement
{
    public Transform insideInteractionSurrounding;

    protected override void Start ()
    {
        base.Start();
        interactionSurroundings.GetComponent<InteractionSurrounding>().ParentID = GetId();
        interactionSurroundings.GetComponent<InteractionSurrounding>().SurroundingType = InteractionSurrounding.Type.hidingSpotEntrance;
        insideInteractionSurrounding.GetComponent<InteractionSurrounding>().ParentID = GetId();
        insideInteractionSurrounding.GetComponent<InteractionSurrounding>().SurroundingType = InteractionSurrounding.Type.hidingSpotInside;
    }

    public override int Player1Use ()
    {
        return 0;
    }
    public override int Player2Use ()
    {
        return 0;
    }
}
