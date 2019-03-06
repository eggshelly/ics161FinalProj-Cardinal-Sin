using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }
    

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
        StartCoroutine(LoadMainScreen());
    }

    public void StartGame()
    {
        FindObjectOfType<AudioManager>().Stop("Opening");
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
        yield return StartCoroutine(TransitionManager.instance.FadeToBlack(2.1f));
        SaveFileManager.instance.DeleteInstancesIfNotLoading();
        SceneManager.LoadScene("TestMap");

    }

    public IEnumerator LoadMenu()
    {
        yield return StartCoroutine(TransitionManager.instance.FadeToBlack(1.5f));
        SceneManager.LoadScene("SaveFiles");
        yield return StartCoroutine(TransitionManager.instance.FadeToLevel(1.5f));
    }

    public IEnumerator LoadMainScreen()
    {
        player.GetComponent<PlayerMapMovement>().enabled = false;
        player.GetComponent<PlayerMapInteraction>().enabled = false;
        SaveFileManager.instance.SetLoadDataFalse();
        yield return StartCoroutine(TransitionManager.instance.FadeToBlack(1.5f));
        SceneManager.LoadScene("MainMenu");
        yield return StartCoroutine(TransitionManager.instance.FadeToLevel(1.5f));
    }
}
