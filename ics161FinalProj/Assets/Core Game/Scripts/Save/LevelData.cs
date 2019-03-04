using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData

{
    //for the player
    public float[] position;


    //keeps track of all stage collectibles. If a collectible has been obtained, then it is "true" in the array. otherwise, false
    public bool[][] stageCollectibles;

    //keeps track of stage levels
    public int[] stageLevels;

    //To keep track of if the intro has been done already
    public bool introCompleted;

    //Constructs the LevelData object which stores all the information that should be saved and later retrieved
    public LevelData(Vector3 playerPos, StageHubScript stageHub, DialogueManager manager)
    {
        position = new float[3];
        position[0] = playerPos.x;
        position[1] = playerPos.y;
        position[2] = playerPos.z;

        stageCollectibles = stageHub.allStageCollectibles.ToArray();
        stageLevels = stageHub.GetStageLevels().ToArray();

        introCompleted = manager.hasDoneIntro;
        
    }
}
