using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMap : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
    
        //Vector3 playerLocation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerLocation = player.transform.position;
        this.transform.position = new Vector3(playerLocation.x, playerLocation.y, -20);
    }
}
