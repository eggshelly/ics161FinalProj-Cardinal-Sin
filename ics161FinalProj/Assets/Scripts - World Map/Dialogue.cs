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
    public string audio;
    public string dialogue;

    public Dialogue(string speaker, string text, string sprite, string audio, string dialogue)
    {
        this.speaker = speaker;
        this.text = text;
        this.sprite = sprite;
        this.audio = audio;
        this.background = background;
    }
}
