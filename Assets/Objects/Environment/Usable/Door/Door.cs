using UnityEngine;

public class Door : UsableElement
{
    public Transform rightInteractionSurrounding; // interactionSurrounding inherited from UsableElement will be treated as left interaction surrounding
    public int startingState = 0; // 0 = closed, 1 = open, 2 = barricaded left, 3 = barricaded right

    public Door ()
    {}

    protected override void Start ()
    {
        base.Start();
        interactionSurroundings.GetComponent<InteractionSurrounding>().ParentID = GetId();
        interactionSurroundings.GetComponent<InteractionSurrounding>().SurroundingType = InteractionSurrounding.Type.leftDoor;
        rightInteractionSurrounding.GetComponent<InteractionSurrounding>().ParentID = GetId();
        rightInteractionSurrounding.GetComponent<InteractionSurrounding>().SurroundingType = InteractionSurrounding.Type.rightDoor;
        switch (startingState)
        {
            case 0:
                break;
            case 1:
                GetComponent<Animator>().SetTrigger("Player2UseLeft");
                break;
            case 2:
                GetComponent<Animator>().SetTrigger("Player2BarricadeLeft");
                break;
            case 3:
                GetComponent<Animator>().SetTrigger("Player2BarricadeRight");
                break;
            default:
                Debug.Log ("Unknown door starting state!");
                break;
        }
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
