using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StoryEventManager : MonoBehaviour
{
    private Queue<string> statemnets;
    public UnityEvent talkEvent;

    public Text titleText;
    public Text storyText;

    public Animator animator;

    void Start()
    {
        statemnets = new Queue<string>();
    }

    public void StartStoryEvent(StoryEvent storyevent)
    {
        titleText.text = storyevent.title;

        animator.SetBool("isOpen", true);

        statemnets.Clear();

        foreach(string statement in storyevent.texts)
        {
            statemnets.Enqueue(statement);
        }

        Time.timeScale = 0;
        DisplayNextStoryEvent();
    }

    public void DisplayNextStoryEvent()
    {
        if (statemnets.Count == 0)
        {
            EndStoryEvent();
            return;
        }
        else if(talkEvent != null)
        {
            talkEvent.Invoke();
        }

        string statemnet = statemnets.Dequeue();

        StopAllCoroutines();
        storyText.text = "";

        foreach (char letter in statemnet.ToCharArray())
        {
            if (letter == ' ')
                storyText.text += " ";
            else
                storyText.text += letter;
        }
    }

    public void EndStoryEvent()
    {
        Time.timeScale = 1;
        animator.SetBool("isOpen", false);
    }
}
