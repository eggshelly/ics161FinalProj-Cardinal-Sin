using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private bool exitUnlocked = false;
    private bool levelOver = false;

    public int itemsLeft = 0;
    public int itemsCollected = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    void Update()
    {
        checkExit();
    }

    void checkExit()
    {
        if (itemsCollected > 0 && (itemsCollected == itemsLeft))
            exitUnlocked = true;
    }
}
