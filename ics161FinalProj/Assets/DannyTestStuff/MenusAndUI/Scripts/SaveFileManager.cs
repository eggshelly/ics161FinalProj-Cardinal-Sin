using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveFileManager : MonoBehaviour
{
    public static SaveFileManager instance = null;

    [SerializeField] GameObject OpenFile;

    PlayerData player;
    StageHubScript stageHub;

    OpenFileScript fileScript;

    string currentButton;

    bool loadData = false;

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
        SaveFileScript.SaveLevel(currentButton, player, stageHub);
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
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
            stageHub = GameObject.FindGameObjectWithTag("StageHub").GetComponent<StageHubScript>();

            if (loadData)
            {
                LevelData level = SaveFileScript.LoadLevel(currentButton);
                player.totalPoints = level.points;
                Vector3 loadedPos = new Vector3(level.position[0], level.position[1], level.position[2]);
                player.transform.position = loadedPos;

                stageHub.loadStages(level.stagesEntered);

            }
        }
    }



     
}
