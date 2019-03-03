using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillPlayerScript : MonoBehaviour
{
    //This class is literally only used for this event. The LevelManager adds a listener to this event. This script is attached to the parent object of all death-causing blocks
    //the children of this gameObject will invoke this function to minimize the amount of listeners the level manager needs to add
    //kind of keeps it simple?
    //In order to have kill blocks, just attach this script to a gameobject and set the tag to DeathBlock. Then add any kill blocks as a child to this gameobject and attach the DeathBlock
    //Script to the children and everything should work.
    public UnityEvent GameOver;
   
    private void Awake()
    {
        GameOver = new UnityEvent();
    }
}
