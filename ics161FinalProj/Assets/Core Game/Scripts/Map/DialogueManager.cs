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
    public GameObject bgPanel;
    public GameObject bgPanel2;

    private Queue<Dialogue> textOutput;
    public int initialText = 0;    //if initialText is -1, then a sentence can start as the dialoguePanel is set to true
    private bool spaceDelay = false;    //locks the player from pressing spacebar while there is text being printed
    private Dialogue nextLine;  //holds the next line on the queue, used to print letter by letter or replace the current text
    private Coroutine lastRoutine = null;   //used to hold pointer to coroutine call responsible for printing letter by letter
    private bool introTransition = false;
    public bool dialogueAvailable = false;

    public UnityEvent DoneWithDialogue;
    public UnityEvent EndOfWeek;

    public bool hasDoneIntro = false;
    private Sprite currentSprite = null;
    private Sprite currentBG = null;
    private Color invisible;

    private bool CR_Run = false;

    private void Awake()
    {
        DoneWithDialogue = new UnityEvent();
        EndOfWeek = new UnityEvent();
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
        if (!hasDoneIntro)
        {
            LoadDialogue("INTRODUCTION");
            hasDoneIntro = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(PauseOnMap.mapPaused == false){
        if (textOutput.Count >= 1)
        {
            dialogueAvailable = true;
            dialoguePanel.SetActive(true);            
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
                dialogueAvailable = false;

                if (!introTransition)
                {
                    StartCoroutine(TransitionManager.instance.screenFadeIn);
                    introTransition = true;
                }

                initialText = 0;
                HidePanels();
                textOutput.Clear();
                currentSprite = null;
                bgPanel2.GetComponent<Image>().sprite = null;
                NextWeek();
                DoneWithDialogue.Invoke();
            }

        }
        else if (textOutput.Count == 0)     //if we're on the last sentence, we want to wait for the player to close the dialogue box
        {
            if(Input.GetKeyDown(KeyCode.Space) && !spaceDelay)
            {
                HidePanels();
                initialText = 0;
                dialogueAvailable = false;
                currentSprite = null;
                bgPanel2.GetComponent<Image>().sprite = null;
                    NextWeek();
                DoneWithDialogue.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && spaceDelay)
            {
                DisplayFullSentence();
                HidePanels();
                initialText = 0;
                dialogueAvailable = false;
                currentSprite = null;
                bgPanel2.GetComponent<Image>().sprite = null;
                NextWeek();
                DoneWithDialogue.Invoke();
            }
        }
        }
    }

    void NextWeek()
    {
        if (SaveFileManager.instance.finishedAStage)
        {
            EndOfWeek.Invoke();
        }
    }

    public void HidePanels()
    {
        dialoguePanel.SetActive(false);
        namePanel.SetActive(false);
        spritePanel.SetActive(false);
        bgPanel.SetActive(false);
        bgPanel2.SetActive(false);
    }

    public void ClearDialogue()
    {
        dialogueAvailable = false;
        currentSprite = null;
        HidePanels();
        textOutput.Clear();
        dialogueText.text = "";
        StopAllCoroutines();
    }

    private Dictionary<int, Dialogue> BuildDialogueDictionary(TextAsset textFile) //converts CSV file to dictionary
    {
        Dictionary <int, Dialogue> GameDialogue = new Dictionary<int, Dialogue>();

        string[] data = textFile.text.Split(new char[] { '\n' });

        for (int i = 2; i <= data.Length - 1; i += 2) //even lines due to CSV sheet issues (prime lines are ,,)
        {
            string[] parsedData = data[i].Split(new char[] { ',' });

            Dialogue dialogueObj = new Dialogue(parsedData[0], parsedData[1].Replace("XYZ", ","), parsedData[2], parsedData[3], parsedData[4]); 

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
        string backgroundName = DialogueObj.background.ToString().Trim();
        string bgFilepath = "Art/";


        if(AudioManager.instance.currentSong.name != DialogueObj.audio && DialogueObj.audio != null)
            FindObjectOfType<AudioManager>().DialogueTransitionSong(DialogueObj.audio);    //music changes during dialogue
        bgFilepath = BGStringBuilder(bgFilepath, backgroundName, DialogueObj);
        currentBG = Resources.Load<Sprite>(bgFilepath) as Sprite;
        if(currentBG != null)
        {
            if(bgPanel2.GetComponent<Image>().sprite == null)
            {
                bgPanel.GetComponent<Image>().sprite = currentBG;
                bgPanel2.GetComponent<Image>().sprite = currentBG;
                invisible = bgPanel2.GetComponent<Image>().color;
                invisible.a = 0f;
                bgPanel2.GetComponent<Image>().color = invisible;
                bgPanel2.SetActive(true);        //bgpanel2 alpha value must be initialized as invisible
                StartCoroutine(bgFadeIn(1.0f));     //this will fade in panel2
                StartCoroutine(WaitCR());
            }
            else       //this is run if the bgpanel image component is not null, which means its not the first time in the dialogue
            {
                bgPanel2.GetComponent<Image>().sprite = currentBG;
                invisible = bgPanel2.GetComponent<Image>().color;
                invisible.a = 0f;
                bgPanel2.SetActive(true);    //must be invisible at this point
                StartCoroutine(bgFadeIn(1.0f));
                StartCoroutine(WaitForCR());
            }

            //if last dialogue has passed, set the image sprite componenet of bgpanel2 to null so we know that at start of next dialogue,
            //it will start at the first part of the block instead of the second part.
        }
        
            


        if (DialogueObj.speaker.Length == 1) //monologue: set namePanel to invisible and text to italic
        {
            namePanel.SetActive(false);
            if(currentSprite == null)
                spritePanel.SetActive(false);
            dialogueText.fontStyle = FontStyles.Italic;
        }
        else if (DialogueObj.speaker.Length == 2) //for MC talking: name and sprite panel are both visible, showing MC name and girl he talks to
        {
            namePanel.SetActive(true);
            nameText.text = DialogueObj.speaker;
            if(currentSprite == null)
                spritePanel.SetActive(false);


        }
        else //any other character: set namePanel to visible and text to normal
        {
            namePanel.SetActive(true);
            nameText.text = DialogueObj.speaker;
            filePath = string.Format("{0}{1}/{2}", filePath, DialogueObj.speaker.Trim(), spriteName);

            if (DialogueObj.speaker == "Haruka" || DialogueObj.speaker == "Touka" || DialogueObj.speaker == "Akiko" || DialogueObj.speaker == "Natsuki") //aka only loads avail sprite    
            {
                if (DialogueObj.speaker == "Haruka" && !introTransition)
                {
                    StartCoroutine(TransitionManager.instance.screenFadeIn);
                    introTransition = true;
                }
                Debug.Log(filePath);
                currentSprite = Resources.Load<Sprite>(filePath) as Sprite;
                spritePanel.GetComponent<Image>().sprite = currentSprite;
            }

        }

        foreach (char letter in DialogueObj.text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;  
        }
        spaceDelay = false;
    }

    public string BGStringBuilder(string filePath, string backgroundName, Dialogue DO)
    {
        string[] splitString = backgroundName.Split(' ');
        if(splitString[0] == "BG")
        {
            if(DO.sprite != "NONE")
                spritePanel.SetActive(true);    //shows the sprite only if the background is BG and if the sprite name is not equal to NONE
            else
                spritePanel.SetActive(false);    //otherwise it hides the spritePanel
            return string.Format("{0}{1}/{2}", filePath, "BG", splitString[1]);
        }
        else{
            spritePanel.SetActive(false);
            return string.Format("{0}{1}/{2}/{3}", filePath, "CG", DO.speaker, splitString[1]);
        }
    }


    public IEnumerator bgFadeIn(float fadeTime)
    {
        CR_Run = true;
        Image tempImage = bgPanel2.GetComponent<Image>();
        float alphaVal = 0f;
        while(bgPanel2.GetComponent<Image>().color.a < 1f)
        {
            alphaVal += Time.deltaTime / fadeTime;
            bgPanel2.GetComponent<Image>().color = new Color(tempImage.color.r, tempImage.color.g, tempImage.color.b, alphaVal);
            yield return null;
        }
        CR_Run = false;
    }

    private IEnumerator WaitCR()
    {
        while (CR_Run)
        {
            yield return null;
        }
        bgPanel.SetActive(true);
        bgPanel2.GetComponent<Image>().color = invisible;
        bgPanel2.SetActive(false);
        
    }

    private IEnumerator WaitForCR()
    {
        while(CR_Run)
        {
            yield return null;
        }
        bgPanel.GetComponent<Image>().sprite = currentBG;
        bgPanel2.GetComponent<Image>().color = invisible;
        bgPanel2.SetActive(false);

    }
}