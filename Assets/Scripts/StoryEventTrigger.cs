using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEventTrigger : MonoBehaviour
{
    public StoryEvent storyEvent;
    private bool isShown;

    [SerializeField]
    public GameObject player;

    private void Start()
    {
        isShown = false;
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 1.0f)
        {
            TriggerStoryEvent();
            isShown = true;
        }
    }

    public void TriggerStoryEvent()
    {
        FindObjectOfType<StoryEventManager>().StartStoryEvent(storyEvent);
    }
}
