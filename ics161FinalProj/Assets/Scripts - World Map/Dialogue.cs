using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Dialogue
{
    public string speaker;
    public string text;
    public string sprite;

    public Dialogue(string speaker, string text, string sprite)
    {
        this.speaker = speaker;
        this.text = text;
        this.sprite = sprite;
    }
}
