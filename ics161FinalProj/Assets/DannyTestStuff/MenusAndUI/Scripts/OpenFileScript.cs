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
        //GetObjects();
    }


    public void CannotLoad()
    {
        loadButton.interactable = false;
    }

    public void LoadFile(string buttonName)
    {
        Panel.SetActive(true);
        loadButton.interactable = true;
        headerText.text = buttonName + " Loaded"; 
    }

    public void ChangeFile()
    {
        Panel.SetActive(false);
    }

    void GetObjects()
    {
        Panel = GameObject.Find("FilePanel");
        headerText = GameObject.Find("HeaderText").GetComponent<TextMeshProUGUI>();
        loadButton = GameObject.Find("Load Game").GetComponent<Button>();
        Panel.SetActive(false);
    }
    
}
