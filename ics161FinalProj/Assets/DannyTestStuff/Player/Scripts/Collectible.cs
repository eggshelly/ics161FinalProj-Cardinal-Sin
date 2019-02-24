using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//public class StringUnityEvent: UnityEvent<string> { }

public class Collectible : MonoBehaviour
{
    public IntUnityEvent Collected;

    int index;

    private void Awake()
    {
        Collected = new IntUnityEvent();
    }

    /*void Start()
    {
        LevelManager.instance.itemsLeft += 1;
    }*/
    // Start is called before the first frame update

    public void setIndex(int i)
    {
        index = i;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Collected.Invoke(index);
        Destroy(this.gameObject);
    }
}
