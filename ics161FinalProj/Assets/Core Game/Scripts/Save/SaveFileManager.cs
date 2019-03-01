using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveFileManager : MonoBehaviour
{
    public static SaveFileManager instance = null;

    [SerializeField] GameObject OpenFile;

    StageHubScript stageHub;

    GameObject player;

    OpenFileScript fileScript;

    string currentButton;

    int currentStage;
    string currentStageName;
    bool[] currentStageCollectibles;

    Vector3 playerPos;

    bool loadData = false;

    bool finishedAStage = false;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
    }

    public void LoadDataForLevel()
    {
        loadData = true;
    }

    public void SetLoadDataFalse()
    {
        loadData = false;
    }

    public void SaveGame()
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("TestMap"))
            SaveFileScript.SaveLevel(currentButton, playerPos, stageHub);
        else
        {
            SaveFileScript.SaveLevel(currentButton, player.transform.position, stageHub);
        }
        LoadDataForLevel();
    }

    public void ChooseSaveFile(string buttonName)
    {
        currentButton = buttonName;
        fileScript.LoadFile(buttonName);
        if (!SaveFileScript.CheckSaveFile(buttonName))
        {
            fileScript.CannotLoad();
        }
    }

    void OnSceneLoaded(Scene loadedScene, LoadSceneMode sceneMode)
    {
        if(loadedScene == SceneManager.GetSceneByName("SaveFiles"))
        {
            OpenFile = GameObject.Find("OpenFile");
            fileScript = OpenFile.GetComponent<OpenFileScript>();
        }
        else if (loadedScene == SceneManager.GetSceneByName("TestMap"))
        {
            player = GameObject.FindGameObjectWithTag("Player");
            stageHub = GameObject.FindGameObjectWithTag("StageHub").GetComponent<StageHubScript>();
            if (loadData)
            {
                LevelData level = SaveFileScript.LoadLevel(currentButton);
                Vector3 loadedPos = new Vector3(level.position[0], level.position[1], level.position[2]);
                player.transform.position = loadedPos;
                stageHub.loadStages(level.stageCollectibles);

            }
            if (finishedAStage)
            {
                stageHub.UpdateCollectiblesForStage(currentStage, currentStageCollectibles);
                player.transform.position = playerPos;
                finishedAStage = false;
            }
        }
        else if(loadedScene.name == currentStageName)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

    }

    public void SaveCurrentPosition()
    {
        playerPos = player.transform.position;
    }

    public void AddCollectablesCountToCurrentState(bool[] collected)
    {
        currentStageCollectibles = collected;
        finishedAStage = true;
        //loadData = true;
    }

    public void SetCurrentStageNumber(int index, string name)
    {
        currentStageName = name;
        currentStage = index;
    }

   void printListOfBools(bool[] list)
    {
        for(int i = 0; i < list.Length; ++i)
        {
            Debug.Log(list[i]);
        }
    }



     
}
