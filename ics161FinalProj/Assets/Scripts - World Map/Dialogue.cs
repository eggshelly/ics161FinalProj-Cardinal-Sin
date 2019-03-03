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

    public Dialogue(string speaker, string text, string sprite, string audio)
    {
        this.speaker = speaker;
        this.text = text;
        this.sprite = sprite;
        this.audio = audio;
    }
}
