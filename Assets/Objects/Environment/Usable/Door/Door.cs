using UnityEngine;

public class Door : UsableElement
{
    public Transform rightInteractionSurrounding; // interactionSurrounding inherited from UsableElement will be treated as left interaction surrounding

    public Door ()
    {}

    protected override void Start ()
    {
        base.Start();
        interactionSurroundings.GetComponent<InteractionSurrounding>().ParentID = GetId();
        interactionSurroundings.GetComponent<InteractionSurrounding>().SurroundingType = InteractionSurrounding.Type.leftDoor;
        rightInteractionSurrounding.GetComponent<InteractionSurrounding>().ParentID = GetId();
        rightInteractionSurrounding.GetComponent<InteractionSurrounding>().SurroundingType = InteractionSurrounding.Type.rightDoor;
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
