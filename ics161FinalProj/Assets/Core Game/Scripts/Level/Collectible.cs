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
