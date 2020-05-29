using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHintBehaviour : MonoBehaviour
{
    public Text hintText;

    string [] possibleTexts = {"", string.Format("Press M\nopen/close"), string.Format("Press F\nunlock & open/close"),
    string.Format("Press M\nhide"), string.Format("Press M\nleave")};

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
