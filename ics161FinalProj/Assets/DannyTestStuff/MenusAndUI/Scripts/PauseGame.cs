using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject PausePanel;


    bool toggled = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!toggled)
            {
                Pause();
            }
            else
            {
                ResumedGame();
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        toggled = true;
    }

    public void ResumedGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        toggled = false;
    }

    public void SaveGame()
    {
        SaveFileManager.instance.SaveGame();
    }
}
