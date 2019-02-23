using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData

{
    //for the player
    public float[] position;
    public int points;


    //for the stage
    public bool[] stagesEntered;

    public LevelData(PlayerData playerData, StageHubScript stageHub)
    {
        points = playerData.totalPoints;
        position = new float[3];
        position[0] = playerData.transform.position.x;
        position[1] = playerData.transform.position.y;
        position[2] = playerData.transform.position.z;



        stagesEntered = new bool[stageHub.GetNumberOfStages()];
        Stage[] allStages = stageHub.getAllStages();
        for(int i = 0; i < allStages.Length; ++i)
        {
            stagesEntered[i] = allStages[i].alreadyEntered;
        }
    }
}
