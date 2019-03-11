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


    string currentStage = null; //Keeps track of which stage the player is currently on 


    Stage[] allStages; //array that stores all the stages
    public Dictionary<Stage, List<bool[]>> allStageCollectibles; //A list of bool arrays where the bool arrays are the list of collectibles of each stage
    int[] stageLevels;
    GameObject player;

    StagePanelScript panel;

    private void Awake()
    {

        allStageCollectibles = new Dictionary<Stage, List<bool[]>>();
        allStages = GetComponentsInChildren<Stage>();
        CreateArrays();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        panel = StagePanel.GetComponent<StagePanelScript>();
        DialogueManager.instance.DoneWithDialogue.AddListener(DoneWithDialogueListener);
        SetStageListeners();
    }

    //Retrieves the list of collectibles from each stage and puts it into the list allStageCollectibles
    void CreateArrays()
    {
        allStageCollectibles.Clear();
        Stage s;
        for (int i = 0; i < allStages.Length; ++i)
        {
            s = allStages[i];
            allStageCollectibles[s] = new List<bool[]>();
            for (int j = 0; j < 4; ++j)
            {
                allStageCollectibles[s].Add(new bool[s.GetTotalCollectibles(j)]);
            }
            s.stageIndex = i;
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
    public void loadStages(bool[][][] stagesCollectibles, bool[][] levelStatus)
    { 
        Stage s;
        for (int i = 0; i < stagesCollectibles.Length; ++i)
        {
            s = allStages[i];
            s.SetLevelStatus(levelStatus[i]);
            for(int j = 0; j < stagesCollectibles[i].Length; ++j)
                allStageCollectibles[allStages[i]][j] = stagesCollectibles[i][j];  
            
        }
        UpdateAllStages();
    }

    //Updates the collectibles of each individual stage to the array stored by this class. Used when loading in data
    void UpdateAllStages()
    {
        Stage s;
        for (int i = 0; i < allStages.Length; ++i)
        {
            s = allStages[i];
            s.LoadCollectibles(allStageCollectibles[s]);
        }
    }

    //Updates the list of collectibles for an individual stage. Used when a stage is completed
    public void UpdateFinishedStage(int index, int stageLevel, bool[] stageColl)
    {
        Stage s = allStages[index];
        allStageCollectibles[s][stageLevel-1] = stageColl;
        s.NextLevel(stageLevel);
        s.LoadCollectibles(allStageCollectibles[s]);
    }

    //When a player presses E on a stage, the StageEntered Event is Invoked. Passes the stage name and stage index to SaveFileManager and activates the Panel. 
    void StageEnteredListener(string stageName, int stageIndex, int stageLevel, bool[] collectibles)
    {
        currentStage = stageName;
        panel.OpenPanel(stageName, stageIndex, stageLevel, collectibles);
        SaveFileManager.instance.SetCurrentStageNumber(stageName, stageIndex, stageLevel);
    }
    //Deactivates the Panel
    void StageLeftListener()
    {
        currentStage = null;
        panel.ClosePanel();
    }

    //Listener that is triggered when the DialogueManager is done with the current sequence. Starts the level 
    void DoneWithDialogueListener()
    {
        if (currentStage != null)
        {
            panel.ClosePanel();
            player.GetComponent<PlayerMapMovement>().enabled = false;
            player.GetComponent<PlayerMapInteraction>().enabled = false;
            FindObjectOfType<AudioManager>().Stop(AudioManager.instance.currentSong.name);
            StartCoroutine(DoneDialogueCR());
            StartCoroutine(TransitionManager.instance.screenFadeOut);
        }
    }

    public void PreviousLevel(int index, int level)
    {
        int cLevel = allStages[index].PrevLevel(level);
        currentStage = currentStage.Substring(0, currentStage.Length - 1) + cLevel;
        panel.UpdatePanel(cLevel, allStages[index].GetLevelCollectibles(cLevel)); 
    }

    public void NextLevel(int index, int level)
    {
        int cLevel = allStages[index].NextLevel(level);
        currentStage = currentStage.Substring(0, currentStage.Length - 1) + cLevel;
        panel.UpdatePanel(cLevel, allStages[index].GetLevelCollectibles(cLevel)); 
    }

    public bool hasFinishedLevel(int index, int level)
    {
        return allStages[index].levelComplete(level);
    }

    public List<bool[]> GetAllStageLevelStatus()
    {
        List<bool[]> l = new List<bool[]>();
        foreach(Stage s in allStages)
        {
            l.Add(s.GetLevelStatus());
        }
        return l;
    }

    public void BackToMap()
    {
        panel.ClosePanel();
    }

    public IEnumerator DoneDialogueCR()
    {
        while(AudioManager.instance.CR_running)
        {
            yield return null;
        }
        player.GetComponent<PlayerMapMovement>().enabled = true;
        player.GetComponent<PlayerMapInteraction>().enabled = true;
        //SaveFileManager.instance.SaveCurrentPosition();
        SaveFileManager.instance.SaveTempInfo();
        SceneManager.LoadScene(currentStage);

    }
}