using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script is kind of unnecessary so I can merge this with the LevelManager script if you want
public class ExitDoorScript : MonoBehaviour
{
    LevelManager manager;


    public bool canExit { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<LevelManager>();
        manager.FinishedLevel.AddListener(FinishedLevelListener);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FinishedLevelListener()
    {
        canExit = true;
    }

    //Tells the LevelManager to pass the stage collectibles to the SaveFileManager before transitioning to TestMap 
    public void BackToMap()
    {
        manager.PassDataToSaveManager();
    }




}
