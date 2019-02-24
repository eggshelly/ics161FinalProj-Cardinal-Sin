using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class IntUnityEvent : UnityEvent<int> { }

public class StringIntUnityEvent: UnityEvent<string, int, int> { }

public class Stage : MonoBehaviour
{
    [SerializeField] int pointValue;
    [SerializeField] float sizeMulti;
    [SerializeField] int totalNumCollectibles;

    public IntUnityEvent FinishedStage;

    public StringIntUnityEvent StageEntered;

    SpriteRenderer m_SpriteRenderer;
    Color origColor;
    GameObject player;

    public int collectiblesLeft = -1;

    private void Awake()
    {
        StageEntered = new StringIntUnityEvent();
        FinishedStage = new IntUnityEvent();
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
        }
    }

    public void EnterStage()
    {
            StageEntered.Invoke(gameObject.name, collectiblesLeft, totalNumCollectibles);
            FinishedStage.Invoke(pointValue);
    }

    public void CheckIfEntered()
    {
    }

    public void LoadEntered(int collectibles)
    {
        collectiblesLeft = collectibles;
    }


}
