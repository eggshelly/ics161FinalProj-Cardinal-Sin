using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    //Used a Brackey's tutorial for reference, though basically the same thing 

    [SerializeField] Transform[] backgrounds;
    [SerializeField] float smoothing;
    [SerializeField] float intensity = 1f;

    float[] scale;

    Transform cam;

    Vector3 prevCamPos; 

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        prevCamPos = cam.position;
        scale = new float[backgrounds.Length];

        for( int i = 0; i < backgrounds.Length; ++i)
        {
            scale[i] = backgrounds[i].position.z * -1 * intensity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = cam.position;
        if(currentPos != prevCamPos)
        {
            for(int i = 0; i < backgrounds.Length; ++i)
            {
                float p = (prevCamPos.x - currentPos.x) * scale[i];
                Vector3 targetPos = new Vector3(backgrounds[i].position.x + p, backgrounds[i].position.y, backgrounds[i].position.z);
                backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, targetPos, smoothing * Time.deltaTime);
            }
            prevCamPos = currentPos;
        }
        
    }
}
