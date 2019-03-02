﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMapInteraction : MonoBehaviour
{
    Stage OnThisStage;

    bool canEnterStage = false;

    
    void Update()
    {
        if(canEnterStage && Time.timeScale != 0)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                OnThisStage.EnterStage();
            }
        }
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