using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    void Start()
    {
        LevelManager.instance.itemsLeft += 1;
    }
    // Start is called before the first frame update

    void OnTriggerEnter2D(Collider2D collision)
    {
        LevelManager.instance.itemsCollected += 1;
        Destroy(this.gameObject);
    }
}
