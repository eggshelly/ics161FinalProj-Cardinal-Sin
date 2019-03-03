using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlockScript : MonoBehaviour
{
    //This script does the actual detecting of collision and invokes the event in its parent gameObject

    KillPlayerScript kill;
    // Start is called before the first frame update
    void Start()
    {
        kill = GetComponentInParent<KillPlayerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            kill.GameOver.Invoke();
        }
    }
}
