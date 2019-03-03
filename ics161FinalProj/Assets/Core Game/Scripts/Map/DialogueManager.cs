using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public GameObject dialoguePanel;
    public GameObject stagePanel;
    public GameObject namePanel;
    public GameObject spritePanel;

    private Queue<Dialogue> textOutput;
    public int initialText = 0;    //if initialText is -1, then a sentence can start as the dialoguePanel is set to true
    private bool spaceDelay = false;    //locks the player from pressing spacebar while there is text being printed
    private Dialogue nextLine;  //holds the next line on the queue, used to print letter by letter or replace the current text
    private Coroutine lastRoutine = null;   //used to hold pointer to coroutine call responsible for printing letter by letter

    public UnityEvent DoneWithDialogue;

    private void Awake()
    {
        DoneWithDialogue = new UnityEvent();
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

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

            if (Input.GetKeyDown(KeyCode.Space) && !spaceDelay)
                DisplayNextSentence();
            else if (Input.GetKeyDown(KeyCode.Space) && spaceDelay)
                DisplayFullSentence();
            else if (Input.GetKeyDown(KeyCode.P)) //press P to skip dialogue, FOR T E S T I N G P U R P O S E S
            {
                spaceDelay = false;
                initialText = 0;
                Time.timeScale = 1;

                HidePanels();

                textOutput.Clear();
                DoneWithDialogue.Invoke();
            }
        }
        else if (textOutput.Count == 0)     //if we're on the last sentence, we want to wait for the player to close the dialogue box
        {
            if (Input.GetKeyDown(KeyCode.Space) && !spaceDelay && Time.timeScale == 0)
            {
                HidePanels();
                Time.timeScale = 1;
                initialText = 0;

                DoneWithDialogue.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && spaceDelay && Time.timeScale == 0)
            {
                DisplayFullSentence();

                HidePanels();
                Time.timeScale = 1;
                initialText = 0;

                DoneWithDialogue.Invoke();
            }
        }
    }

    private void HidePanels()
    {
        dialoguePanel.SetActive(false);
        namePanel.SetActive(false);
        spritePanel.SetActive(false);
    }

    private Dictionary<int, Dialogue> BuildDialogueDictionary(TextAsset textFile) //converts CSV file to dictionary
    {
        Dictionary <int, Dialogue> GameDialogue = new Dictionary<int, Dialogue>();

        string[] data = textFile.text.Split(new char[] { '\n' });

        for (int i = 2; i <= data.Length - 1; i += 2) //even lines due to CSV sheet issues (prime lines are ,,)
        {
            string[] parsedData = data[i].Split(new char[] { ',' });

            Dialogue dialogueObj = new Dialogue(parsedData[0], parsedData[1].Replace("XYZ", ","), parsedData[2], parsedData[3]);

            GameDialogue.Add(i / 2, dialogueObj);
        }

        return GameDialogue;
    }

    public void LoadDialogue(string fileName) //loads dictionary into queue
    {
        if (stagePanel != null)
            stagePanel.SetActive(false);

        int currentLine = 1;

        Dictionary<int, Dialogue> dialogueDict = BuildDialogueDictionary(Resources.Load<TextAsset>(fileName));

        while (currentLine <= dialogueDict.Count)
            textOutput.Enqueue(dialogueDict[currentLine++]);
    }

    private void DisplayNextSentence()
    {
        nextLine = textOutput.Dequeue();
        spaceDelay = true;
        lastRoutine = StartCoroutine(UpdateText(nextLine));
    }

    private void DisplayFullSentence()
    {
        StopCoroutine(lastRoutine);
        dialogueText.text = nextLine.text;
        spaceDelay = false;
    }

    private IEnumerator UpdateText(Dialogue DialogueObj)
    {
        dialogueText.text = "";
        dialogueText.fontStyle = FontStyles.Normal;
        string filePath = "Art/Portraits/";
        string spriteName = DialogueObj.sprite.ToString().Trim();

        if (DialogueObj.speaker.Length == 1) //monologue: set namePanel to invisible and text to italic
        {
            namePanel.SetActive(false);

            if (spriteName == "none")
            {
                Debug.Log("NO SPRITE");
                spritePanel.SetActive(false);
            }

            else
                spritePanel.SetActive(true);

            dialogueText.fontStyle = FontStyles.Italic;
        }
        else if (DialogueObj.speaker.Length == 2) //for special cases
        {
            namePanel.SetActive(true);
            nameText.text = DialogueObj.speaker;

            if (spriteName == "none")
            {
                Debug.Log("NO SPRITE");
                spritePanel.SetActive(false);
            }

        }
        else //any other character: set namePanel to visible and text to normal
        {
            namePanel.SetActive(true);
            nameText.text = DialogueObj.speaker;
            filePath = string.Format("{0}{1}/{2}", filePath, DialogueObj.speaker.Trim(), spriteName);

            if (DialogueObj.speaker != "Haruka" && DialogueObj.speaker != "Touka" && DialogueObj.speaker != "Akiko" && DialogueObj.speaker != "Natsuki") //aka we don't have a sprite for it (MC, other charas, etc)    
            {
                Debug.Log(filePath);
                spritePanel.GetComponent<Image>().sprite = Resources.Load<Sprite>(filePath) as Sprite;
            }

            spritePanel.SetActive(true);
        }

        foreach (char letter in DialogueObj.text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;  
        }
        spaceDelay = false;
    }
}