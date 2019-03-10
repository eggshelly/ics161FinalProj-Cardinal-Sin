using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SaveFileManager : MonoBehaviour
{
    public static SaveFileManager instance = null; //ensures only one instance of the savefilemanager

    [SerializeField] GameObject OpenFile; //this tells the OpenFile gameobject in the SaveFiles scene to open up the panel that allows the player to press new game or load data

    StageHubScript stageHub; 

    GameObject player;

    OpenFileScript fileScript; //This is the script attached to the OpenFile gameobject above

    string currentButton; //this is the file that was loaded e.g. File 1, File 2, in order to keep track of what path to save to

    //FOR THE STAGE THAT THE PLAYER IS EITHER CURRENTLY IN OR ON IN THE TEST MAP
    int currentStage; //Each stage is given an index by StageHub and is stored in an array. This makes it easier to quickly locate the stage that was just finished to update collectibles
    public string currentStageName; //The Stage's name is also the name of the scene. This keeps track of what Stage/Scene was just finished 
    int currentStageLevel;
    bool[] currentStageCollectibles; //Keeps track of the list of collectibles from the finished stage (each index refers to one collectible, true if collected, false if not)
    LevelData data;

    Vector3 playerPos; //Keeps track of the position of the player prior to entering the stage. Once the stage is finished the player is moved to this location

    bool loadData = false; //Tells the SaveFileManager whether or not to load the data when the scene is loaded

    bool finishedAStage = false; //Keeps track of whether or not a state was just finished - aids in passing to the stage its list of collectibles
    public bool isStageCompleted { get; private set; }
    
    //Creates the instance of the SaveFileManager
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;      
    }

    //Called by LoadFile function in the ButtonFunctions script when the "Load Game" button is pressed in the SaveFiles scene. Makes it so that data is loaded once the TestMap scene is loaded
    public void LoadDataForLevel()
    {
        loadData = true;
    }

    //Called by BackToMenu function in ButtonFunctions Script. When a player goes back to the main menu, it sets loadData to false so if the player starts a new game it won't try to load data
    public void SetLoadDataFalse()
    {
        loadData = false;
    }

    //Saves the game
    public void SaveGame()
    {
        SaveFileScript.SaveLevel(currentButton, player.transform.position, stageHub, DialogueManager.instance, TimeManager.instance);
    }


    //This opens the panel when the player selects a file. If a path exists, the Load Game button is interactable. Otherwise, it is not. 
    public void ChooseSaveFile(string buttonName)
    {
        currentButton = buttonName;
        fileScript.LoadFile(buttonName);
        if (!SaveFileScript.CheckSaveFile(buttonName))
        {
            fileScript.CannotLoad();
        }
    }

    public void DeleteInstancesIfNotLoading()
    {
        if(!loadData)
        {
            if (DialogueManager.instance != null)
            {
                Destroy(DialogueManager.instance.gameObject);
                Destroy(TimeManager.instance.gameObject);
            }
        }
    }

    /*
     * Takes care the bulk of loading data and setting script variables:
     * 
     * If the scene is: 
     * 
     * SaveFiles: 
     *  -sets variables so it can open the panel
     *  
     * TestMap: 
     *  -If data should be loaded, loads the data and sets it appropriately. e.g. player position, collectibles to the respective stages
     *  
     * Add: 
     *     use the stored stage name (and eventually the level the stage is on) to load the appropriate dialogue using DialogueManager
     * 
     */
    void OnSceneLoaded(Scene loadedScene, LoadSceneMode sceneMode)
    {
        if(loadedScene == SceneManager.GetSceneByName("SaveFiles"))
        {
            OpenFile = GameObject.Find("OpenFile");
            fileScript = OpenFile.GetComponent<OpenFileScript>();
        }
        else if (loadedScene == SceneManager.GetSceneByName("TestMap"))
        {
            //StartCoroutine(TransitionManager.instance.FadeToLevel(2f));
            player = GameObject.FindGameObjectWithTag("Player");
            stageHub = GameObject.FindGameObjectWithTag("StageHub").GetComponent<StageHubScript>();
            if (loadData)
            {
                StartCoroutine(TransitionManager.instance.FadeToLevel(2f));
                LevelData level = SaveFileScript.LoadLevel(currentButton);
                Vector3 loadedPos = new Vector3(level.position[0], level.position[1], level.position[2]);
                player.transform.position = loadedPos;
                stageHub.loadStages(level.stageCollectibles, level.finishedLevels);
                DialogueManager.instance.hasDoneIntro = level.introCompleted;
                TimeManager.instance.SetDay(level.week, level.day);
                

            }
            if (finishedAStage)
            {
                StartCoroutine(TransitionManager.instance.FadeToLevel(2f));
                string path = string.Format("{0} ", currentStageName);      //create the string to be the name of the current stage followed by the stage number
                if(isStageCompleted)
                    path += "GOOD END";
                else
                    path += "BAD END";

                DialogueManager.instance.LoadDialogue(path);
                Vector3 loadedPos = new Vector3(data.position[0], data.position[1], data.position[2]);
                player.transform.position = loadedPos;
                stageHub.loadStages(data.stageCollectibles, data.finishedLevels);
                stageHub.UpdateFinishedStage(currentStage, currentStageLevel, currentStageCollectibles);
                TimeManager.instance.NextDay();
                finishedAStage = false;
            }
        }
    }

    public void SaveTempInfo()
    {
        data = new LevelData(Vector3.zero, stageHub, DialogueManager.instance, TimeManager.instance);
    }


    //Keeps track of the player's position right before they enter the stage
    public void SaveCurrentPosition()
    {
        playerPos = player.transform.position;
    }

    //Holds the list of collectibles from the stage that was just finished.
    public void AddCollectablesCountToCurrentState(bool[] collected)
    {
        currentStageCollectibles = collected;
        finishedAStage = true;
        isStageCompleted = !collected.Contains(false);

    }

    //Sets the name and index of the stage that was interacted with. (this is called if the player presses E on the stage and brings up the stage enter panel)
    public void SetCurrentStageNumber(string name, int index, int level)
    {
        currentStageName = name;
        currentStage = index;
        currentStageLevel = level;
    }
    
}
