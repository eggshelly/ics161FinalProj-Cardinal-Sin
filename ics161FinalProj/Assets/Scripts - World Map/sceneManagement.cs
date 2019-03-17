using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class sceneManagement : MonoBehaviour
{
    private string scName;
    public static sceneManagement instance;
    public bool sceneFlag = false;
    public sceneMusic[] relations;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneFlag = true;   //for background transitions

        scName = scene.name;
        foreach(sceneMusic x in relations)
        {
            if(x.sceneName == scName)
            {
                FindObjectOfType<AudioManager>().Play(x.songName);
            }
        }

    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


}
