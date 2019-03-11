using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelDialogue : MonoBehaviour
{
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] List<string> levelDialogue;

    PlayerLevelMovement p_Movement;
    PlayerLevelInteraction p_Interaction;

    bool started = false;

    bool waitingToContinue = false;
    bool doneWithDialogue = false;
    bool entered = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject p = GameObject.Find("Player");
        p_Movement = p.GetComponent<PlayerLevelMovement>();
        p_Interaction = p.GetComponent<PlayerLevelInteraction>();
    }

    void Update()
    {
        if(started && Input.GetKeyDown(KeyCode.P))
        {
            waitingToContinue = false;
            UnfreezePlayer();
            dialoguePanel.SetActive(false);
        }
        if (waitingToContinue)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!doneWithDialogue)
                {
                    waitingToContinue = false;
                    StartCoroutine(ReadLine());
                }
                else
                {
                    waitingToContinue = false;
                    UnfreezePlayer();
                    dialoguePanel.SetActive(false);
                }
            }
        }
    }

    void NextLine()
    {
        StopCoroutine(ReadLine());
        levelDialogue.RemoveAt(0);
        if (levelDialogue.Count == 0)
        {
            doneWithDialogue = true;
        }
        waitingToContinue = true;
    }

    IEnumerator ReadLine()
    {
        dialogueText.text = "";
        string l = levelDialogue[0];
        for(int i = 0; i < l.Length; ++i )
        {
            dialogueText.text += l[i];
            yield return new WaitForSeconds(0.02f);
        }
        NextLine();
    }

    void FreezePlayer()
    {
        p_Movement.frozen = true;
        p_Interaction.interact = false;
    }
    void UnfreezePlayer()
    {
        p_Movement.frozen = false;
        p_Interaction.interact = true;
        p_Movement.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!entered)
        {
            if (collision.CompareTag("Player"))
            {
                started = true;
                entered = true;
                FreezePlayer();
                dialoguePanel.SetActive(true);
                StartCoroutine(ReadLine());
            }
        }
    }
}
