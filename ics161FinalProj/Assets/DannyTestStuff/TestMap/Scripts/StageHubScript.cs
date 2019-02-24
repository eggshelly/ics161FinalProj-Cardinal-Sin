using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StageHubScript : MonoBehaviour
{

    [SerializeField] int numStages;
    [SerializeField] GameObject StagePanel;
    [SerializeField] TextMeshProUGUI headerText;


    Stage[] allStages;

    string currentStage;

    public List<bool[]> allStageCollectibles;

    private void Awake()
    {
        allStageCollectibles = new List<bool[]>();
        allStages = GetComponentsInChildren<Stage>();
        CreateArrays();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        SetStageListeners();
    }

    void CreateArrays()
    {
        allStageCollectibles.Clear();
        for (int i = 0; i < allStages.Length; ++i)
        {
            allStageCollectibles.Add(new bool[allStages[i].GetTotalCollectibles()]);
            allStages[i].stageIndex = i;
        }
    }

    // Update is called once per frame
    public void loadStages(bool[][] stagesCollectibles)
    {
        for (int i = 0; i < stagesCollectibles.Length; ++i)
        {
            allStageCollectibles[i] = stagesCollectibles[i];
        }
        /*int index= 0;
        int maxColl = allStages[index].GetTotalCollectibles();
        List<bool> temp = new List<bool>();
        for (int i = 0; i < stagesCollectibles.Length; ++i)
        {
            if(temp.Count < maxColl)
            {
                temp.Add(stagesCollectibles[i]);
            }
            else
            {
                allStageCollectibles[index] = temp.ToArray();
                temp.Clear();
                index += 1;
                maxColl = allStages[index].GetTotalCollectibles();
                temp.Add(stagesCollectibles[i]);
            }
            
        }*/
        UpdateCollectiblesOfStages();
    }

    void UpdateCollectiblesOfStages()
    {
        for (int i = 0; i < allStages.Length; ++i)
        {
            allStages[i].LoadCollectibles(allStageCollectibles[i]);
        }
    }


    public Stage[] getAllStages()
    {
        return allStages;
    }

    public int GetNumberOfStages()
    {
        return numStages;
    }

    void StageEnteredListener(string stageName, int index, bool[] collectibles)
    {
        StagePanel.SetActive(true);
        headerText.text = stageName;
        currentStage = stageName;
        SaveFileManager.instance.SetCurrentStageNumber(index, stageName);
        StagePanel.GetComponent<CollectiblesStageUI>().CreateCollectibles(collectibles);
    }

    void StageLeftListener()
    {
        BackToMap();
    }

    public void BackToMap()
    {
        StagePanel.SetActive(false);
    }

    void SetStageListeners()
    {
        for (int i = 0; i < allStages.Length; ++i)
        {
            allStages[i].StageEntered.AddListener(StageEnteredListener);
            allStages[i].StageLeft.AddListener(StageLeftListener);
        }
    }

    public void UpdateCollectiblesForStage(int index, bool[] stageColl)
    {
        allStageCollectibles[index] = stageColl;
        UpdateCollectiblesOfStages();
    }

    public void BeginCurrentLevel()
    {
        SaveFileManager.instance.SaveCurrentPosition();
        SceneManager.LoadScene(currentStage);
    }

}
