﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void CheckSaveFiles()
    {
        StartCoroutine(LoadMenu());
    }
    public void BackToMenu()
    {
        Time.timeScale = 1;
        FindObjectOfType<AudioManager>().Stop(AudioManager.instance.currentSong.name);
        StartCoroutine(LoadMainScreen());
    }

    public void StartGame()
    {
        FindObjectOfType<AudioManager>().Stop(AudioManager.instance.currentSong.name);
        StartCoroutine(StartGameCR());
    }

    public void Credits()
    {
        //SceneManager.LoadScene("Credits");
    }

    public void ChooseFile(string buttonName)
    {
        SaveFileManager.instance.ChooseSaveFile(buttonName);
    }

    public void LoadFile()
    {
        SaveFileManager.instance.LoadDataForLevel();
        StartGame();
    }

    public IEnumerator StartGameCR()
    {
        yield return StartCoroutine(TransitionManager.instance.screenFadeOut);
        SaveFileManager.instance.DeleteInstancesIfNotLoading();
        SceneManager.LoadScene("TestMap");

    }

    public IEnumerator LoadMenu()
    {
        yield return StartCoroutine(TransitionManager.instance.screenFadeOut);
        SceneManager.LoadScene("SaveFiles");
        yield return StartCoroutine(TransitionManager.instance.screenFadeIn);
    }

    public IEnumerator LoadMainScreen()
    {
        SaveFileManager.instance.SetLoadDataFalse();
        yield return StartCoroutine(TransitionManager.instance.screenFadeOut);
        SceneManager.LoadScene("MainMenu");
        yield return StartCoroutine(TransitionManager.instance.screenFadeIn);
    }
}
