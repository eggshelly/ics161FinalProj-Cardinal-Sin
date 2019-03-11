using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOnMap : MonoBehaviour
{
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject StagePanel;

    GameObject player;
     
    static public bool mapPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Time.timeScale = 1;
    }

    //If escape is pressed: if the stage panel is activated, then deactive it. Otherwise the pause menu is brought up. If pause panel is already active, then deactivate it
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!StagePanel.activeInHierarchy)
            {
                if (!mapPaused)
                {
                    Pause();
                }
                else
                {
                    ResumedGame();
                }
            }
            else
            {
                StagePanel.SetActive(false);
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        mapPaused = true;
    }

    public void ResumedGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        mapPaused = false;
    }

    public void MainMenu()
    {
        player.GetComponent<PlayerMapInteraction>().CantInteract();
        player.GetComponent<PlayerMapMovement>().CantMove();
        DialogueManager.instance.ClearDialogue();
        PausePanel.SetActive(false);
        GetComponent<ButtonFunctions>().BackToMenu();
    }

    //Saves the game 
    public void SaveGame()
    {
        SaveFileManager.instance.SaveGame();
    }
}
