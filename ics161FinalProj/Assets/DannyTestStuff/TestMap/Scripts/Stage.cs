using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class IntUnityEvent : UnityEvent<int> { }

public class StringIntUnityEvent: UnityEvent<string, int, bool[]> { }

public class Stage : MonoBehaviour
{
    [SerializeField] float sizeMulti;
    [SerializeField] int totalNumCollectibles;

    public UnityEvent StageLeft;

    public IntUnityEvent FinishedStage;

    public StringIntUnityEvent StageEntered;

    public int stageIndex { get; set; } 

    SpriteRenderer m_SpriteRenderer;
    Color origColor;
    GameObject player;

    bool[] collectibles;

    public int collectiblesLeft = -1;

    private void Awake()
    {
        CreateCollectiblesArray();
        StageEntered = new StringIntUnityEvent();
        FinishedStage = new IntUnityEvent();
        StageLeft = new UnityEvent();
        collectiblesLeft = (collectiblesLeft < 0 ? totalNumCollectibles: collectiblesLeft);
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        origColor = m_SpriteRenderer.color;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateCollectiblesArray()
    {
        collectibles = new bool[totalNumCollectibles];
        for(int i = 0; i < totalNumCollectibles; ++i)
        {
            collectibles[i] = false;
        }
    }

    public int GetTotalCollectibles()
    {
        return totalNumCollectibles;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
                transform.localScale = transform.localScale * sizeMulti;
                m_SpriteRenderer.color = Color.red;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
                transform.localScale = transform.localScale / sizeMulti;
                m_SpriteRenderer.color = origColor;
                 StageLeft.Invoke();
        }
    }

    public void EnterStage()
    {
            StageEntered.Invoke(gameObject.name, stageIndex, collectibles);
            //FinishedStage.Invoke(pointValue);
    }

    public void CheckIfEntered()
    {
    }

    public void LoadCollectibles(bool[] collect)
    {
        
        if (collect.Length > 0)
        {
            collectibles = collect;
        }
    }

    


}
