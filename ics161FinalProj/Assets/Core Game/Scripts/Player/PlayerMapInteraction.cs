using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMapInteraction : MonoBehaviour
{
    Stage OnThisStage;

    bool canEnterStage = false;

    bool canInteract = true;
    void Update()
    {
        if (canInteract)
        {
            
            if (canEnterStage && Time.timeScale != 0)
            {
                if (OnThisStage != null && !OnThisStage.gameObject.activeInHierarchy)
                {
                    OnThisStage = null;
                    canEnterStage = false;
                }
                else if (Input.GetAxisRaw("Interact") == 1 && !DialogueManager.instance.dialogueAvailable)
                {
                    OnThisStage.EnterStage();
                }

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

    public void CantInteract()
    {
        canInteract = false;
    }

    public void CanInteract()
    {
        canInteract = true;
    }
}
