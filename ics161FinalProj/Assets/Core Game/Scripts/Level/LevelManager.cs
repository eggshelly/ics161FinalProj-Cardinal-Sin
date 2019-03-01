using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject collectiblesPanel;
    [SerializeField] Sprite keySpriteFilled;
    [SerializeField] Sprite keySpriteEmpty;
    [SerializeField] float distanceBetween;

    //public static LevelManager instance;
    GameObject[] collectibles;
    Image[] collectiblesSprites;
    public bool[] collected;

    public UnityEvent FinishedLevel;
    //private bool levelOver = false;

    int totalCollectibles;
    int itemsCollected = 0;

    void Awake()
    {
        FinishedLevel = new UnityEvent();
    }

    private void Start()
    {
        collectibles = GameObject.FindGameObjectsWithTag("Collectible");
        collectiblesSprites = new Image[collectibles.Length];
        collected = new bool[collectibles.Length];
        totalCollectibles = collectibles.Length;
        for(int i = 0; i < collectibles.Length; ++i)
        {
            Collectible collComponent = collectibles[i].GetComponent<Collectible>();
            collComponent.setIndex(i);
            collComponent.Collected.AddListener(CollectedListener);
        }
        CreateCollectiblesPanel();
    }

    void Update()
    {
        //checkExit();
    }

    void checkExit()
    {
        if (itemsCollected == totalCollectibles)
            FinishedLevel.Invoke();
                //exitUnlocked = true;
    }


    void CollectedListener(int index)
    {
        collected[index] = true;
        UpdateCollectiblesPanel(index);
        itemsCollected += 1;
        checkExit();
    }

    public void PassDataToSaveManager()
    {
        SaveFileManager.instance.AddCollectablesCountToCurrentState(collected);
        SceneManager.LoadScene("TestMap");
    }


    void CreateCollectiblesPanel()
    {
        RectTransform r = collectiblesPanel.GetComponent<RectTransform>();
        Vector3 leftBorder = new Vector3(r.rect.xMin, r.transform.position.y);
        for (int i = 0; i < totalCollectibles; i++)
        {
            GameObject newImage = new GameObject();
            newImage.transform.SetParent(collectiblesPanel.transform, false);
            newImage.transform.localScale = newImage.transform.localScale * 0.7f;
            RectTransform rect = newImage.AddComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(leftBorder.x + distanceBetween/2 + distanceBetween * i, newImage.transform.localPosition.y);

            Image sr = newImage.AddComponent<Image>();
            sr.sprite = keySpriteEmpty;
            collectiblesSprites[i] = sr;
        }
    }

    void UpdateCollectiblesPanel(int index)
    {
        collectiblesSprites[index].sprite = keySpriteFilled;
    }
}
