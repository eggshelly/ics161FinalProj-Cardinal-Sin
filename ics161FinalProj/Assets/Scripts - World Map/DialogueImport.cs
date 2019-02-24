using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class DialogueImport : MonoBehaviour
{
    //public TextAsset csvFile = Resources.Load("INTRODUCTION.csv") as TextAsset;
    public TextAsset csvFile;
    private char lineSeparator = '\n';
    private char fieldSeparator = ',';
    private Dictionary<int, Dialogue> GameDialogue;

    // Start is called before the first frame update
    void Awake()
    {
        GameDialogue = new Dictionary<int, Dialogue>();
    }
    void Start()
    {
        readData();
        foreach(KeyValuePair<int, Dialogue> i in GameDialogue)
        {
            Debug.Log(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void readData()
    {
        string name = "";
        string text = "";
        string sprite = "";
        int i = 0;
        string[] parsedFields = new string[0];
        string[] parsedLines = csvFile.text.Split(new char[] { '\n' });
        Debug.Log(parsedLines.Length);
        for (int j = 0; j < parsedLines.Length - 1; j++)
        {
            string[] row = parsedLines[j].Split(new char[] { ','});
            Debug.Log(row);
        }

        foreach(string line in parsedLines)
        {
            parsedFields = line.Split(fieldSeparator);
        } 
        foreach(string field in parsedFields)
        {
            if(i%3 == 0)
            {
                name = parsedFields[i];
            }
            else if(i%3 == 1)
            {
                text = parsedFields[i];
            }
            else if(i%3 == 2)
            {
                sprite = parsedFields[i];
                GameDialogue.Add((i-2), new Dialogue(name, text, sprite));
            }
            ++i;
        }
    }
}
