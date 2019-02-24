using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //public static LevelManager instance;
    GameObject[] collectibles;
    public bool[] collected;

    public UnityEvent FinishedLevel;
    //private bool levelOver = false;

    int totalCollectibles;
    int itemsCollected = 0;

    void Awake()
    {
        FinishedLevel = new UnityEvent();
    }

    private void Start()
    {
        collectibles = GameObject.FindGameObjectsWithTag("Collectible");
        collected = new bool[collectibles.Length];
        totalCollectibles = collectibles.Length;
        for(int i = 0; i < collectibles.Length; ++i)
        {
            Collectible collComponent = collectibles[i].GetComponent<Collectible>();
            collComponent.setIndex(i);
            collComponent.Collected.AddListener(CollectedListener);
        }
    }

    void Update()
    {
        //checkExit();
    }

    void checkExit()
    {
        if (itemsCollected == totalCollectibles)
            FinishedLevel.Invoke();
                //exitUnlocked = true;
    }


    void CollectedListener(int index)
    {
        collected[index] = true;
        itemsCollected += 1;
        checkExit();
    }

    public void PassDataToSaveManager()
    {
        SaveFileManager.instance.AddCollectablesCountToCurrentState(collected);
        SceneManager.LoadScene("TestMap");
    }
}
