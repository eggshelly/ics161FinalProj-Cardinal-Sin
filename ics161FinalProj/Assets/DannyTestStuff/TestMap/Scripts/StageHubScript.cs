using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHubScript : MonoBehaviour
{
    [SerializeField] int numStages;


    Stage[] allStages;
    // Start is called before the first frame update
    void Start()
    {
        allStages = GetComponentsInChildren<Stage>();
    }

    // Update is called once per frame
    public void loadStages(bool[] stagesEntered)
    {
        Start();
        for(int i = 0; i < stagesEntered.Length; ++i)
        {
            allStages[i].LoadEntered(stagesEntered[i]);
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
}
