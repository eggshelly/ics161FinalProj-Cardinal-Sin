using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class IntUnityEvent : UnityEvent<int> { }

public class StringIntUnityEvent: UnityEvent<string, int, bool[]> { }

public class Stage : MonoBehaviour
{
    [SerializeField] float sizeMulti;
    [SerializeField] int totalNumCollectibles; //total number of collectibles the stage will contain. MUST SET IN THE INSPECTOR IN ORDER TO DISPLAY CORRECTLY ON THE PANEL

    public int currentLevel{ get; private set; }

    public UnityEvent StageLeft;

    public IntUnityEvent FinishedStage;

    public StringIntUnityEvent StageEntered;

    public int stageIndex { get; set; } 

    //--------------------
    //temporary
    SpriteRenderer m_SpriteRenderer;
    Color origColor;
    //--------------------
    GameObject player;

    bool[] collectibles; //collectibles for the stage

    private void Awake()
    {
        //Events to be called
        StageEntered = new StringIntUnityEvent();
        FinishedStage = new IntUnityEvent();
        StageLeft = new UnityEvent();



        CreateCollectiblesArray();
        currentLevel = 1;
        player = GameObject.FindGameObjectWithTag("Player");
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        origColor = m_SpriteRenderer.color;
    }

    //Initially sets its collectibles to all false - no collectibles have been obtained yet
    void CreateCollectiblesArray()
    {
        collectibles = new bool[totalNumCollectibles];
        for(int i = 0; i < totalNumCollectibles; ++i)
        {
            collectibles[i] = false;
        }
    }

    //Used by StageHubScript
    public int GetTotalCollectibles()
    {
        return totalNumCollectibles;
    }


    //Makes the stage increase in size and turn red when the player is over the stage - temporary
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            transform.localScale = transform.localScale * sizeMulti;
            m_SpriteRenderer.color = Color.red;
        }
    }

    //reverts the size and color when the player leaves
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            transform.localScale = transform.localScale / sizeMulti;
            m_SpriteRenderer.color = origColor;
            StageLeft.Invoke();
        }
    }

    //Called by PlayerMapInteraction - Passes this information to StageHubScript
    public void EnterStage()
    {
        StageEntered.Invoke(GetName(), stageIndex, collectibles);
    }


    //Called by StageHubScript to load in the collectibles 
    public void LoadCollectibles(bool[] collect)
    {
        
        if (collect.Length > 0)
        {
            collectibles = collect;
        }
    }

    string GetName()
    {
        return gameObject.name + " " + currentLevel;
    }

    


}
