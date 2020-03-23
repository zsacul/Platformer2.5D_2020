using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEventTrigger : MonoBehaviour
{
    public StoryEvent storyEvent;

    public void TriggerStoryEvent()
    {
        FindObjectOfType<StoryEventManager>().StartStoryEvent(storyEvent);
    }
}
