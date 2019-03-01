using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpenFileScript : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    [SerializeField] TextMeshProUGUI headerText;
    [SerializeField] Button loadButton;

    private void Awake()
    {
    }

    // Called by ChooseSaveFile function in SaveFileManager script. If no path exists this then Load Game button is not interactable
    public void CannotLoad()
    {
        loadButton.interactable = false;
    }

    //Sets Active the Panel when a file is selected
    public void LoadFile(string buttonName)
    {
        Panel.SetActive(true);
        loadButton.interactable = true;
        headerText.text = buttonName + " Loaded"; 
    }

    //Sets inactive the Panel when the player wants to select another file 
    public void ChangeFile()
    {
        Panel.SetActive(false);
    }
    
}
