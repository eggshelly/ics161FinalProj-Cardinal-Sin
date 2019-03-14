using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class IntUnityEvent : UnityEvent<int> { }

public class StringIntUnityEvent: UnityEvent<string, int, int, bool[]> { }

public class Stage : MonoBehaviour
{
    [SerializeField] Sprite WavingSprite;
    [SerializeField] float sizeMulti;
    [SerializeField] int[] totalNumCollectibles; //total number of collectibles the stage will contain. MUST SET IN THE INSPECTOR IN ORDER TO DISPLAY CORRECTLY ON THE PANEL

    public int currentLevel{ get; private set; }
    public int stageIndex { get; set; }

    public UnityEvent StageLeft;

    public IntUnityEvent FinishedStage;

    public StringIntUnityEvent StageEntered;

    //--------------------
    //temporary
    SpriteRenderer m_SpriteRenderer;
    Sprite origSprite;
    //--------------------
    GameObject player;

    Dictionary<int, bool[]> allCollectibles;

    bool[] collectibles; //collectibles for the stage

    Dictionary<int, bool> hasFinishedLevel;

    private void Awake()
    {
        //Events to be called
        StageEntered = new StringIntUnityEvent();
        FinishedStage = new IntUnityEvent();
        StageLeft = new UnityEvent();



        allCollectibles = new Dictionary<int, bool[]>();
        hasFinishedLevel = new Dictionary<int, bool>();
        SetLevelBools();
        ResetCollectibles();
        currentLevel = 1;
        player = GameObject.FindGameObjectWithTag("Player");
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        origSprite = m_SpriteRenderer.sprite;
    }

    public bool[] GetLevelCollectibles(int level)
    {
        return allCollectibles[level];
    }

    void SetLevelBools()
    {
        for(int i = 1; i < 5; ++i)
        {
            hasFinishedLevel[i] = false;
        }
    }


    //Initially sets its collectibles to all false - no collectibles have been obtained yet
    void ResetCollectibles()
    { 
        for(int i = 1; i < 5; ++i)
        {
            allCollectibles[i] = new bool[totalNumCollectibles[i - 1]];
            for(int j = 0; j < totalNumCollectibles[i-1]; ++j)
            {
                allCollectibles[i][j] = false;
            }
        }
    }

    //Used by StageHubScript
    public int GetTotalCollectibles(int index)
    {
        return totalNumCollectibles[index];
    }


    //Makes the stage increase in size and turn red when the player is over the stage - temporary
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            m_SpriteRenderer.sprite = WavingSprite;
        }
    }

    //reverts the size and color when the player leaves
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            m_SpriteRenderer.sprite = origSprite;
            StageLeft.Invoke();
        }
    }

    //Called by PlayerMapInteraction - Passes this information to StageHubScript
    public void EnterStage()
    {
        
        StageEntered.Invoke(GetName(), stageIndex, currentLevel, allCollectibles[currentLevel]);
    }


    //Called by StageHubScript to load in the collectibles 
    public void LoadCollectibles(List<bool[]> collect)
    {
       for(int i = 0; i < collect.Count; ++i)
        {
            allCollectibles[i + 1] = collect[i];
        }
    }

    public int NextLevel(int level)
    {
        hasFinishedLevel[level] = true;
        currentLevel = level + 1;
        return currentLevel;
    }

    public int PrevLevel(int level)
    {
        currentLevel = level - 1;
        return currentLevel;
    }

    public string GetName()
    {
        return gameObject.name + " " + currentLevel;
    }

    public bool levelComplete(int level)
    {
        return hasFinishedLevel[level];
    }

    public bool[] GetLevelStatus()
    {
        List<bool> l = new List<bool>();
        foreach(bool b in hasFinishedLevel.Values)
        {
            l.Add(b);
        }
        return l.ToArray();
    }

    public void SetLevelStatus(bool[] status)
    {
        for(int i = 0; i < status.Length; ++i)
        {
            hasFinishedLevel[i + 1] = status[i];
        }
    }
    


}
