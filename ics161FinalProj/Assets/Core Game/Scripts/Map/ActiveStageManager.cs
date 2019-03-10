using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStageManager : MonoBehaviour
{
    [SerializeField] GameObject spring;
    [SerializeField] GameObject summer;
    [SerializeField] GameObject autumn;
    [SerializeField] GameObject winter;
    [SerializeField] List<int> springDays;
    [SerializeField] List<int> summerDays;
    [SerializeField] List<int> autumnDays;
    [SerializeField] List<int> winterDays;


    // Start is called before the first frame update
    void Start()
    {
        ActivateDays();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActivateDays()
    {
        int day = TimeManager.instance.GetCurrentDay();
        if(springDays.Contains(day))
        {
            spring.SetActive(true);
        }
        else
        {
            spring.SetActive(false);
        }
        if (summerDays.Contains(day))
        {
            summer.SetActive(true);
        }
        else
        {
            summer.SetActive(false);
        }
        if (autumnDays.Contains(day))
        {
            autumn.SetActive(true);
        }
        else
        {
            autumn.SetActive(false);
        }
        if (winterDays.Contains(day))
        {
            winter.SetActive(true);
        }
        else
        {
            winter.SetActive(false);
        }
    }
}
