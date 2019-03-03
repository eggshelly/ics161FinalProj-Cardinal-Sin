using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;


public class AudioManager : MonoBehaviour
{
    public Sound s;
    public Sound[] sounds;
    public static AudioManager instance;

    // Start is called before the first frame update
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
        foreach(Sound track in sounds)
        {
            track.source = gameObject.AddComponent<AudioSource>();
            track.source.clip = track.clip;
            track.source.volume = track.volume; 
            track.source.pitch = track.pitch;
            track.source.loop = track.loop;
        }
    }
    void Start()
    {
        Play("Opening");
    }


    // Update is called once per frame
    void Update()
    {
        s.source.volume = s.volume;
        try{
            //if(!LevelManager.instance.isGameOver)
            //{
                if(!s.source.isPlaying)
                    Play("LoopTrack");
            //}
            //else
            //{
                if(s.name != "GameOverLoop")
                {
                     if(s.source.volume > 0)
                        s.volume -= Time.deltaTime / 2.5f;  //For a duration of fadeTime, volume gradually decreases till its 0
                    if(s.volume <= 0) 
                        s.source.Stop();
                }
                if(!s.source.isPlaying)
                {
                    s.source.volume = s.volume;
                    Play("GameOverLoop");
                    if(s.source.volume < 1)
                    {
                        StartCoroutine(FadeIn(1f));
                    }
                }
            //}
        }
        catch(Exception e)
        {
            if(!s.source.isPlaying)
            {
                Play("LoopTrack");
            }
        }

        
    }

    public void Play (string name)
    {
        s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.Log("ERROR: Sound not found");
            return;
        }
        s.source.Play();
    }

    public IEnumerator FadeOut(float fadeTime)
    {
        while(s.volume > 0)
        {
            s.volume -= Time.deltaTime / fadeTime;  //For a duration of fadeTime, volume gradually decreases till its 0
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator FadeIn(float fadeTime)
    {
        while (s.volume < 1)
        {
            s.volume += Time.deltaTime / fadeTime; //fades in over course of seconds fadeTime
            yield return new WaitForSeconds(0.1f);
        }
    }


    
}
