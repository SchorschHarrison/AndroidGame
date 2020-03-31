using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIText : MonoBehaviour
{

    Text text;

    public void SetText(string text)
    {
        if(this.text == null)
        {
            this.text = GetComponent<Text>();
        }
        this.text.text = text;
    }
     
}
