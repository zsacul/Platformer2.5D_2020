using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryEvent
{
    public string title;

    [TextArea(3,10)]
    public string[] texts;
}
