using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageHubScript : MonoBehaviour
{
    [SerializeField] int numStages;
    [SerializeField] GameObject StagePanel;
    [SerializeField] TextMeshProUGUI headerText;


    Stage[] allStages;
    // Start is called before the first frame update
    void Start()
    {
        allStages = GetComponentsInChildren<Stage>();
        SetStageListeners();
    }

    // Update is called once per frame
    public void loadStages(int[] stagesCollectibles)
    {
        Start();
        for (int i = 0; i < stagesCollectibles.Length; ++i)
        {
            allStages[i].LoadEntered(stagesCollectibles[i]);
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

    void StageEnteredListener(string stageName, int c_left, int totalC)
    {
        StagePanel.SetActive(true);
        headerText.text = stageName;
        StagePanel.GetComponent<CollectiblesScript>().CreateCollectibles(c_left, totalC);
    }

    public void BackToMap()
    {
        StagePanel.SetActive(false);
    }

    void SetStageListeners()
    {
        foreach(Stage s in allStages)
        {
            s.StageEntered.AddListener(StageEnteredListener);
        }
    }
}
