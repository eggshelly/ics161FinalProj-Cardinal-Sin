using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Dialogue
{
    //public string key;
    public string named;
   // [TextArea(3,10)]
    //public string[] sentences;
    public string spriteType;
    public string text;

    public Dialogue(string named, string spriteType, string text)
    {
        this.named = named;
        this.spriteType = spriteType;
        this.text = text;
    }
}
