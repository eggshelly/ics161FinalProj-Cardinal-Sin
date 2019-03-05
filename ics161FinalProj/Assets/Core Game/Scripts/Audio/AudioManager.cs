﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;


public class AudioManager : MonoBehaviour
{
    //[HideInInspector]
    //public Sound s;
    public float fadeTime;  //affects how long it takes to fade audio
    public Sound[] sounds;
    public static AudioManager instance;
    IEnumerator fadeIn;
    IEnumerator fadeOut;

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
        foreach(Sound track in sounds)  //sets all initial values of audio source to be whats in inspector when those sounds are played
        {
            track.source = gameObject.AddComponent<AudioSource>();
            track.source.clip = track.clip;
            track.source.volume = track.volume; //sets the initial volume to what is in inspector
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
    }

    public void Play (string name)  //s.source.volume will adjust actual volume. s.volume will adjust initial value which has no meaning here
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        fadeIn = FadeIn(s);     //we assign coroutines only when we start the song. These same references are used when we stop the song
        fadeOut = FadeOut(s);
        if(s == null)
        {
            Debug.Log("ERROR: Sound not found");
            return;
        }
        //s.volume = 0f;
        //s.source.volume = 0.0f;
        StopCoroutine(fadeOut);    //ensures that fading out does not occur simultaneously to fading in
        StartCoroutine(fadeIn);

    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.Log("ERROR: Sound not found to stop");
            return;
        }
        StopCoroutine(fadeIn);  
        StartCoroutine(fadeOut);
        
    }

    public void StraightPlay(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.Log("ERROR: Sound not found");
            return;
        }
        s.source.Play();
        while(s.source.volume < 1.0f)
        {
            s.source.volume += Time.deltaTime / fadeTime;
        }
    }

    public void StraightStop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.Log("ERROR: Sound not found");
            return;
        }
        while(s.source.volume < 1.0f)
        {
            Debug.Log("here");
            s.source.volume -= Time.deltaTime / fadeTime;
        }
        s.source.volume = 0f;
        s.source.Stop();
    }
    public IEnumerator FadeOut(Sound s)
    {
        while(s.source.volume > 0.01f)
        {
            s.source.volume -= Time.deltaTime / 1.0f;  //For a duration of fadeTime, volume gradually decreases till its 0
            //yield return new WaitForSeconds(0.5f);
            yield return null;
        }
        s.source.volume = 0f;
        s.source.Stop();
    }

    public IEnumerator FadeIn(Sound s)
    {
        s.source.Play();
        while (s.source.volume < 1.0f)
        {
            s.source.volume += Time.deltaTime / fadeTime; //fades in over course of seconds fadeTime
            yield return null;
        }
    }


    
}
