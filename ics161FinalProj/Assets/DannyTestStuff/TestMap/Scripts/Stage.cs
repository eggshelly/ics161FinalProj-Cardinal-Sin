using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class IntUnityEvent : UnityEvent<int> { }

public class Stage : MonoBehaviour
{
    [SerializeField] int pointValue;
    [SerializeField] float sizeMulti;
    [SerializeField] Color darkTint;

    public bool alreadyEntered;

    public IntUnityEvent FinishedStage;

    SpriteRenderer m_SpriteRenderer;
    Color origColor;
    GameObject player;

    private void Awake()
    {
        FinishedStage = new IntUnityEvent();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        alreadyEntered = false;
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
            if(!alreadyEntered)
            {
                transform.localScale = transform.localScale * sizeMulti;
                m_SpriteRenderer.color = Color.red;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (!alreadyEntered)
            {
                transform.localScale = transform.localScale / sizeMulti;
                m_SpriteRenderer.color = origColor;
            }
        }
    }

    public void EnterStage()
    {
        if (!alreadyEntered)
        {
            m_SpriteRenderer.color = Color.black;
            alreadyEntered = true;
            FinishedStage.Invoke(pointValue);
        }
    }

    public void CheckIfEntered()
    {
        if(alreadyEntered)
        {
            Debug.Log("entered!");
            transform.localScale = transform.localScale * sizeMulti;
            m_SpriteRenderer.color = Color.black;
        }
    }

    public void LoadEntered(bool entered)
    {
        alreadyEntered = entered;
        CheckIfEntered();
    }
}
