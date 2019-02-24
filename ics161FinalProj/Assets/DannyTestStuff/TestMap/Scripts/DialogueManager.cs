using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    private Queue<string> textOutput;
    private int initialText = 0;
    private bool spaceDelay = false;

    void Start()
    {
        textOutput = new Queue<string>();
        LoadDialogue("INTRODUCTION");
    }

    // Update is called once per frame
    void Update()
    {
        if (textOutput.Count != 0)
        {
            dialoguePanel.SetActive(true);
            Time.timeScale = 0;

            if (initialText++ == 0)
                DisplayNextSentence();

            if (Input.GetKey(KeyCode.Space) && !spaceDelay)
                DisplayNextSentence();
        }
    }

    public Dictionary<int, Dialogue> BuildDialogueDictionary(TextAsset textFile)
    {
        Dictionary <int, Dialogue> GameDialogue = new Dictionary<int, Dialogue>();

        string[] data = textFile.text.Split(new char[] { '\n' });

        for (int i = 2; i <= data.Length - 1; i += 2)
        {
            string[] parsedData = data[i].Split(new char[] { ',' });

            Dialogue dialogueObj = new Dialogue(parsedData[0], parsedData[2], parsedData[1].Replace("XYZ", ","));

            GameDialogue.Add(i / 2, dialogueObj);
        }

        return GameDialogue;
    }

    void LoadDialogue(string fileName)
    {
        int currentLine = 1;
        Dictionary<int, Dialogue> dialogueDict = BuildDialogueDictionary(Resources.Load<TextAsset>(fileName));

        while (currentLine <= dialogueDict.Count)
            textOutput.Enqueue(dialogueDict[currentLine++].text);
    }

    void DisplayNextSentence()
    {
        spaceDelay = true;
        StartCoroutine(UpdateText(textOutput.Dequeue()));
    }

    IEnumerator UpdateText(string text)
    {
        dialogueText.text = "";

        foreach(char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }

        if (textOutput.Count == 0)
        {
            dialoguePanel.SetActive(false);
            Time.timeScale = 1;
            initialText = 0;
        }

        spaceDelay = false;
    }
}

//Function to load a specific dictionary
//Have keyboard input to progress text in Update()