using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHintBehaviour : MonoBehaviour
{
    public Text hintText;

    string [] possibleTexts = {"", string.Format("Press SPCAE\nopen/close"), string.Format("Press E\nunlock & open/close"),
    string.Format("Press SPCAE\nhide"), string.Format("Press SPCAE\nleave"), string.Format("Press SPCAE\ngo upstairs/downstairs"),
    string.Format("Press E\ngo upstairs/downstairs")};

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
