using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEventTrigger : MonoBehaviour
{
    public StoryEvent storyEvent;
    private bool isShown;
    private StoryEventManager SEM;
    private static string activeStory;
    [SerializeField]
    public GameObject player;

    private void Start()
    {
        activeStory = "";
        isShown = false;
        SEM = FindObjectOfType<StoryEventManager>();
    }
    private void Update()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (isShown == false &&  dist < 1.0f)
        {
            activeStory = gameObject.name;
            isShown = true;
            TriggerStoryEvent();

        }
        if (Input.GetKeyDown(KeyCode.RightControl) && activeStory==gameObject.name)
            SEM.DisplayNextStoryEvent();
    }

    public void TriggerStoryEvent()
    {
        SEM.StartStoryEvent(storyEvent);
    }
}
