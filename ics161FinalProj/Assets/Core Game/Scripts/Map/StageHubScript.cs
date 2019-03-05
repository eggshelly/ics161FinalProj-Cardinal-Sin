using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StageHubScript : MonoBehaviour
{

    [SerializeField] int numStages;
    [SerializeField] GameObject StagePanel; //Panels that displays the number of collected/missing collectibles and allows the player to enter the level
    [SerializeField] TextMeshProUGUI headerText; //Name at the top of the panel that displays the stage name
    [SerializeField] Button EnterStage;

    string currentStage = null; //Keeps track of which stage the player is currently on 


    Stage[] allStages; //array that stores all the stages
    public List<bool[]> allStageCollectibles; //A list of bool arrays where the bool arrays are the list of collectibles of each stage
    int[] stageLevels;

    private void Awake()
    {
        
        allStageCollectibles = new List<bool[]>();
        allStages = GetComponentsInChildren<Stage>();
        CreateArrays();
    }

    // Start is called before the first frame update
    void Start()
    {
        EnterStage.onClick.AddListener(delegate { DialogueManager.instance.LoadDialogue(currentStage); });
        DialogueManager.instance.DoneWithDialogue.AddListener(DoneWithDialogueListener);
        SetStageListeners();
    }

    //Retrieves the list of collectibles from each stage and puts it into the list allStageCollectibles
    void CreateArrays()
    {
        allStageCollectibles.Clear();
        for (int i = 0; i < allStages.Length; ++i)
        {
            allStageCollectibles.Add(new bool[allStages[i].GetTotalCollectibles()]);
            allStages[i].stageIndex = i;
        }
    }

    //Adds event listeners to the Events in the Stage script
    void SetStageListeners()
    {
        for (int i = 0; i < allStages.Length; ++i)
        {
            allStages[i].StageEntered.AddListener(StageEnteredListener);
            allStages[i].StageLeft.AddListener(StageLeftListener);
        }
    }


    //Called by SaveFileManager when data is loaded. Passes in the stored lists of collectibles and stores it.
    public void loadStages(bool[][] stagesCollectibles, int[] stageL)
    {
        for (int i = 0; i < stagesCollectibles.Length; ++i)
        {
            allStageCollectibles[i] = stagesCollectibles[i];   
        }
        UpdateAllStages(stageL);
    }

    //Updates the collectibles of each individual stage to the array stored by this class. Used when loading in data
    void UpdateAllStages(int[] stageL)
    {
        for (int i = 0; i < allStages.Length; ++i)
        {
            allStages[i].LoadCollectibles(allStageCollectibles[i]);
            allStages[i].currentLevel = stageL[i];
        }
    }

    //Updates the list of collectibles for an individual stage. Used when a stage is completed
    public void UpdateFinishedStage(int index, int stageLevel, bool[] stageColl)
    {
        allStageCollectibles[index] = stageColl;
        allStages[index].NextLevel(stageLevel);
        allStages[index].LoadCollectibles(stageColl);
    }

    public List<int> GetStageLevels()
    {
        List<int> levels = new List<int>();
        foreach(Stage s in allStages)
        {
            levels.Add(s.currentLevel);
        }
        return levels;
    }

    //When a player presses E on a stage, the StageEntered Event is Invoked. Passes the stage name and stage index to SaveFileManager and activates the Panel. 
    void StageEnteredListener(string stageName, int stageIndex, int stageLevel, bool[] collectibles)
    {
        StagePanel.SetActive(true);
        headerText.text = stageName;
        currentStage = stageName;
        SaveFileManager.instance.SetCurrentStageNumber(stageName, stageIndex, stageLevel);
        StagePanel.GetComponent<CollectiblesStageUI>().CreateCollectibles(collectibles); //Creates the collectibles sprite (filled and/or empty) on the panel
    }

    //Deactivates the Panel
    void StageLeftListener()
    {
        currentStage = null;
        StagePanel.SetActive(false);
    }

    //Listener that is triggered when the DialogueManager is done with the current sequence. Starts the level 
    void DoneWithDialogueListener()
    {
        if (currentStage != null)
        {
            FindObjectOfType<AudioManager>().Stop("TestMap");
            StartCoroutine(DoneDialogueCR());
        }
    }

    public void BackToMap()
    {
        StagePanel.SetActive(false);
    }

    public IEnumerator DoneDialogueCR()
    {

        while(AudioManager.instance.CR_running)
        {
            yield return null;
        }
        SaveFileManager.instance.SaveCurrentPosition();
        SceneManager.LoadScene(currentStage);

    }
}