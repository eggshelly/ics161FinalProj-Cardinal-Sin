using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneMusicManager : MonoBehaviour
{
    private string scName;
    public static SceneMusicManager instance;
    // Start is called before the first frame update
/*
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
    */
    // Update is called once per frame
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
        //Debug.Log(scene.name);
        //Sound s = Array.Find(AudioManager.instance.sounds, sound => sound.name == name);
        //Debug.Log(s);
        //if(s == null)
            //Debug.Log("its not here");
        //else
            //Debug.Log("its here");
        //if(AudioManager.instance.sounds.name.Contains(scName))
           // Debug.Log("its here");
        //else
            //Debug.Log("its not here");

        //FindObjectOfType<AudioManager>().Play(scName);
        //Debug.Log(scene.name);
        //scName = SceneMusicManager.GetActiveScene().name;
        //Debug.Log("OnSceneLoaded: " + scene.name);
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    //void OnSceneLoaded()
    //{
        //Debug.Log("here");
        //m_scene = SceneManager.GetActiveScene();
        //scName = m_scene.name;
        //FindObjectOfType<AudioManager>().Play(scName);

   // }


}
