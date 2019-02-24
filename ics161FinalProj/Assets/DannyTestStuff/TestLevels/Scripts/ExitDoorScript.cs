using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void BackToMap()
    {
        manager.PassDataToSaveManager();
    }




}
