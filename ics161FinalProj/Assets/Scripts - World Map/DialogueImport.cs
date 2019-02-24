using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class DialogueImport : MonoBehaviour
{
    public TextAsset assetThing;

    public static Dictionary<int, Dialogue> GameDialogue;
    private int currentLine = 1;
    private int maxLine = 0;

    void Awake()
    {
        GameDialogue = new Dictionary<int, Dialogue>();

        string[] data = assetThing.text.Split(new char[] { '\n' });

        maxLine = (data.Length - 1) / 2;

        for (int i = 2; i <= data.Length - 1; i += 2)
        {
            string[] parsedData = data[i].Split(new char[] { ',' });

            Dialogue dialogueObj = new Dialogue(parsedData[0], parsedData[2], parsedData[1].Replace("XYZ", ","));

            GameDialogue.Add(i / 2, dialogueObj);
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentLine <= maxLine)
            Debug.Log(GameDialogue[currentLine++].text);         
    }
}
