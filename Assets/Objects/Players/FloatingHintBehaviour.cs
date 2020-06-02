using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHintBehaviour : MonoBehaviour
{
    public Text hintText;

    string [] possibleTexts = {"", string.Format("Press ENTER\nopen/close"), string.Format("Press SPACE\nunlock & open/close"),
    string.Format("Press ENTER\nhide"), string.Format("Press ENTER\nleave"), string.Format("Press ENTER\ngo upstairs/downstairs"),
    string.Format("Press SPACE\ngo upstairs/downstairs")};

    void Start ()
    {
        setText(0);
    }

    public void setText (int textId)
    {
        hintText.text = possibleTexts[textId];
    }

    void Update()
    {
        Vector3 hintPosition = Camera.main.WorldToScreenPoint(transform.position);
        hintText.transform.position = hintPosition;
    }
}
