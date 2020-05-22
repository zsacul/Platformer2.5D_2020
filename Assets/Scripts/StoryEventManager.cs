using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryEventManager : MonoBehaviour
{
    private Queue<string> statemnets;

    public Text titleText;
    public Text storyText;

    public Animator animator;

    // Start is called before the first frame update
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

        DisplayNextStoryEvent();
    }

    public void DisplayNextStoryEvent()
    {
        Debug.Log("TUTAJ");
        if(statemnets.Count == 0)
        {
            EndStoryEvent();
            return;
        }

        string statemnet = statemnets.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeStatement(statemnet));
    }

    IEnumerator TypeStatement(string statement)
    {
        storyText.text = "";

        foreach(char letter in statement.ToCharArray())
        {
            storyText.text += letter;
            yield return null;
        }
    }

    public void EndStoryEvent()
    {
        animator.SetBool("isOpen", false);
    }
}
