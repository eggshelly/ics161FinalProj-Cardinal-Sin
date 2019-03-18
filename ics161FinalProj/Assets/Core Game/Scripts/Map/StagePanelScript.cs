using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StagePanelScript : MonoBehaviour
{
    [SerializeField] GameObject StagePanel;
    [SerializeField] TextMeshProUGUI headerText;
    [SerializeField] Button EnterStage;
    [SerializeField] Button prevLevel;
    [SerializeField] Button nextLevel;


    [SerializeField] GameObject Panel;
    [SerializeField] Sprite keySpriteFilled;
    [SerializeField] Sprite keySpriteEmpty;
    List<Image> collectibleImages;


    string currentStage = null;
    int currentStageIndex;
    int currentStageLevel;

    StageHubScript stage;
    // Start is called before the first frame update
    void Start()
    {
        collectibleImages = new List<Image>();
        stage = GameObject.FindGameObjectWithTag("StageHub").GetComponent<StageHubScript>();
        EnterStage.onClick.AddListener(delegate { DialogueManager.instance.LoadDialogue(currentStage); });
        EnterStage.onClick.AddListener(delegate { ClosePanel(); });
    }

    public void OpenPanel(string stageName, int sIndex, int sLevel, bool[] collectibles)
    {
        StagePanel.SetActive(true);
        headerText.text = stageName;
        currentStage = stageName;
        currentStageIndex = sIndex;
        currentStageLevel = sLevel;
        SetButtonInteract(sLevel);
        if (stage.hasFinishedLevel(sIndex, sLevel))
        {
            EnterStage.interactable = false;
        }
        else
        {
            EnterStage.interactable = true;
        }
        CreateCollectibles(collectibles); //Creates the collectibles sprite (filled and/or empty) on the panel
    }

    public void UpdatePanel(int sLevel, bool[] collectibles)
    {
        currentStageLevel = sLevel;
        currentStage = currentStage.Substring(0, headerText.text.Length - 1) + sLevel;
        headerText.text = currentStage;
        SetButtonInteract(sLevel);
        if (stage.hasFinishedLevel(currentStageIndex, currentStageLevel))
        {
            EnterStage.interactable = false;
        }
        else
        {
            EnterStage.interactable = true;
        }
        CreateCollectibles(collectibles);
    }
      

    void SetButtonInteract(int level)
    {
        if (level == 1)
        {
            if(prevLevel.interactable == true)
                prevLevel.interactable = false;
        }
        else if (prevLevel.interactable == false)
        {
            prevLevel.interactable = true;
        }
        if (level == stage.GetNumLevelsOfStage(currentStageIndex) || !stage.hasFinishedLevel(currentStageIndex, currentStageLevel))
        {
            if(nextLevel.interactable == true)
                nextLevel.interactable = false;
        }
        else if (nextLevel.interactable == false)
        {
            nextLevel.interactable = true;
        }
    }

    public void ClosePanel()
    {
        StagePanel.SetActive(false);
        currentStage = null;
    }

    //Called by StageHubScript when a stage is "entered". Creates the collectible icons on the StagePanel 
    public void CreateCollectibles(bool[] collectibles)
    {
        if (Panel.transform.childCount == 0 || collectibles.Length > Panel.transform.childCount)
        {
            DeleteCurrent();
            RectTransform r = Panel.GetComponent<RectTransform>();
            float width = r.rect.width;
            Vector3 leftBorder = new Vector3(r.rect.xMin, r.transform.position.y);
            float distanceBetween = width / (collectibles.Length * 2);
            for (int i = 0; i < collectibles.Length; i++)
            {
                GameObject newImage = new GameObject();
                newImage.transform.SetParent(Panel.transform, false);
                RectTransform rect = newImage.AddComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(leftBorder.x + distanceBetween + distanceBetween * 2 * i, newImage.transform.localPosition.y);
                Image image = newImage.AddComponent<Image>();
                if (collectibles[i])
                    image.sprite = keySpriteFilled;
                else
                    image.sprite = keySpriteEmpty;
                collectibleImages.Add(image);
            }
        }
        else
        {
            for(int i = 0; i < collectibles.Length; ++i)
            {
                collectibleImages[i].sprite = (collectibles[i] ? keySpriteFilled : keySpriteEmpty);
            }
        }
    }

    public void PrevLevel()
    {
        stage.PreviousLevel(currentStageIndex, currentStageLevel);
    }

    public void NextLevel()
    {
        Debug.Log("Current " + currentStageLevel);
        stage.NextLevel(currentStageIndex, currentStageLevel);
    }


    //Deletes all the existing collectible icons.
    void DeleteCurrent()
    {
        foreach (Transform t in Panel.transform)
        {
            Destroy(t.gameObject);
        }
        collectibleImages.Clear();
    }

}
