using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData

{
    //for keeping track of what day and week it is
    public int day;
    public int week;


    //for the player
    public float[] position;


    //keeps track of all stage collectibles. If a collectible has been obtained, then it is "true" in the array. otherwise, false
    public bool[][][] stageCollectibles;


    //To keep track of if the intro has been done already
    public bool introCompleted;


    //keeps track of which levels have been finished
    public bool[][] finishedLevels;

    //Constructs the LevelData object which stores all the information that should be saved and later retrieved
    public LevelData(Vector3 playerPos, StageHubScript stageHub, DialogueManager manager, TimeManager time)
    {
        position = new float[3];
        position[0] = playerPos.x;
        position[1] = playerPos.y;
        position[2] = playerPos.z;

        List<bool[][]> coll = new List<bool[][]>();
        foreach(List<bool[]> c in stageHub.allStageCollectibles.Values)
        {
            coll.Add(c.ToArray());
        }

        stageCollectibles = coll.ToArray();

        finishedLevels = stageHub.GetAllStageLevelStatus().ToArray();

        introCompleted = manager.hasDoneIntro;

        day = time.GetCurrentDay();
        week = time.GetCurrentWeek();
        
    }
}
