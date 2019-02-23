using System.Collections;
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
        SceneManager.LoadScene("SaveFiles");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        SaveFileManager.instance.SetLoadDataFalse();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("TestMap");
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
}
