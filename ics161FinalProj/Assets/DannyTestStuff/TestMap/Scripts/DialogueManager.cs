﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public GameObject dialoguePanel;
    public GameObject stagePanel;
    public GameObject namePanel;

    private Queue<Dialogue> textOutput;
    private int initialText = 0;    //if initialText is 0, then there will be a sentence printed automatically
    private bool spaceDelay = false;    //locks the player from pressing spacebar while there is text being printed

    void Start()
    {
        textOutput = new Queue<Dialogue>();
        LoadDialogue("INTRODUCTION");
    }

    // Update is called once per frame
    void Update()
    {
        if (textOutput.Count >= 1)
        {
            dialoguePanel.SetActive(true);
            Time.timeScale = 0;

            if (initialText == 0)
            {
                initialText++;
                DisplayNextSentence();
            }

            if (Input.GetKey(KeyCode.Space) && !spaceDelay)
                DisplayNextSentence();
        }
        else if (textOutput.Count == 0)     //if we're on the last sentence, we want to wait for the player to close the dialogue box
        {
            if (Input.GetKey(KeyCode.Space) && !spaceDelay)
            {
                dialoguePanel.SetActive(false);
                namePanel.SetActive(false);
                Time.timeScale = 1;
                initialText = 0;
            }
        }
    }

    public Dictionary<int, Dialogue> BuildDialogueDictionary(TextAsset textFile) //converts CSV file to dictionary
    {
        Dictionary <int, Dialogue> GameDialogue = new Dictionary<int, Dialogue>();

        string[] data = textFile.text.Split(new char[] { '\n' });

        for (int i = 2; i <= data.Length - 1; i += 2)
        {
            string[] parsedData = data[i].Split(new char[] { ',' });

            Dialogue dialogueObj = new Dialogue(parsedData[0], parsedData[1].Replace("XYZ", ","), parsedData[2]);

            GameDialogue.Add(i / 2, dialogueObj);
        }

        return GameDialogue;
    }

    public void LoadDialogue(string fileName) //loads dictionary into queue
    {
        stagePanel.SetActive(false);

        int currentLine = 1;
        Dictionary<int, Dialogue> dialogueDict = BuildDialogueDictionary(Resources.Load<TextAsset>(fileName));

        while (currentLine <= dialogueDict.Count)
            textOutput.Enqueue(dialogueDict[currentLine++]);
    }

    void DisplayNextSentence()
    {
        spaceDelay = true;
        StartCoroutine(UpdateText(textOutput.Dequeue()));
    }

    IEnumerator UpdateText(Dialogue DialogueObj)
    {
        dialogueText.text = "";
        dialogueText.fontStyle = FontStyles.Normal;

        if (DialogueObj.speaker.Length == 1)
        {
            namePanel.SetActive(false);
            dialogueText.fontStyle = FontStyles.Italic;
        }
        else
        {
            namePanel.SetActive(true);
            nameText.text = DialogueObj.speaker;
        }

        foreach (char letter in DialogueObj.text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }

        spaceDelay = false;
    }
}