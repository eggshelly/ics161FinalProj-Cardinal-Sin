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

    //Constructs the LevelData object which stores all the information that should be saved and later retrieved
    public LevelData(Vector3 playerPos, StageHubScript stageHub)
    {
        position = new float[3];
        position[0] = playerPos.x;
        position[1] = playerPos.y;
        position[2] = playerPos.z;

        stageCollectibles = stageHub.allStageCollectibles.ToArray();
        
    }


    List<bool> ConvertListToArray(List<bool[]> collect)
    {
        List<bool> temp = new List<bool>();
        for(int i = 0; i < collect.Count; ++i)
        {
            for(int j = 0; j < collect[i].Length; ++j)
            {
                temp.Add(collect[i][j]);
            }
        }
        return temp;
    }
}
