using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneMusicManager : MonoBehaviour
{
    private string scName;
    public static SceneMusicManager instance;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        scName = scene.name.Split(' ')[0];
        foreach(Sound x in AudioManager.instance.sounds)
        {
            if(x.name == scName)
            {
                FindObjectOfType<AudioManager>().Play(scName);
            }
        }

    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


}
