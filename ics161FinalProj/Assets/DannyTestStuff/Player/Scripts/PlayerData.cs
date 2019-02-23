using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData : MonoBehaviour
{
    public int totalPoints = 0;

    Stage OnThisStage;

    bool canEnterStage = false;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        SetStageListeners();
    }

    // Update is called once per frame
    void Update()
    {
        if(canEnterStage)
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                OnThisStage.EnterStage();
            }
        }
    }
    
    void SetStageListeners()
    {
        foreach(GameObject stage in GameObject.FindGameObjectsWithTag("Stage"))
        {
            stage.GetComponent<Stage>().FinishedStage.AddListener(FinishedStageListener);
        }
    }

    void FinishedStageListener(int pointVal)
    {
        totalPoints += pointVal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.CompareTag("Stage"))
        {
            canEnterStage = true;
            OnThisStage = collision.gameObject.GetComponent<Stage>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Stage"))
        {
            canEnterStage = false;
            OnThisStage = null;
        }
    }
}
