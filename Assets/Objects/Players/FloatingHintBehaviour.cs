using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHintBehaviour : MonoBehaviour
{
    public Image hint;

    public Sprite [] possibleSprites;
    // public Text hintText;

    // string [] possibleTexts = {"", string.Format("Press ENTER\nopen/close"), string.Format("Press SPACE\nunlock & open/close"),
    // string.Format("Press ENTER\nhide"), string.Format("Press ENTER\nleave"), string.Format("Press ENTER\ngo upstairs/downstairs"),
    // string.Format("Press SPACE\ngo upstairs/downstairs")};

    void Start ()
    {
        // setText(0);
        clearSprite ();
    }

    public void clearSprite ()
    {
        Color c = hint.color;
        c.a = 0.0f;
        hint.color = c;
        //hint.sprite = null;
    }

    public void setSprite (int spriteId)
    {
        Color c = hint.color;
        c.a = 1.0f;
        hint.color = c;
        hint.sprite = possibleSprites[spriteId];
        // hintText.text = possibleTexts[textId];
    }

    void Update()
    {
        Vector3 hintPos = Camera.main.WorldToScreenPoint(transform.position);
        hint.transform.position = hintPos;
    }
}
