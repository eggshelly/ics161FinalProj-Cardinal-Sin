using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStageManager : MonoBehaviour
{
    [SerializeField] GameObject spring;
    [SerializeField] GameObject summer;
    [SerializeField] GameObject autumn;
    [SerializeField] GameObject winter;
    [SerializeField] List<int> springweeks;
    [SerializeField] List<int> summerweeks;
    [SerializeField] List<int> autumnweeks;
    [SerializeField] List<int> winterweeks;


    // Start is called before the first frame update
    void Start()
    {
        TimeManager.instance.NextWeekEvent.AddListener(ActivateWeeks);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActivateWeeks()
    {
        int week = TimeManager.instance.GetCurrentWeek();
        if(springweeks.Contains(week))
        {
            spring.SetActive(true);
        }
        else
        {
            spring.SetActive(false);
        }
        if (summerweeks.Contains(week))
        {
            summer.SetActive(true);
        }
        else
        {
            summer.SetActive(false);
        }
        if (autumnweeks.Contains(week))
        {
            autumn.SetActive(true);
        }
        else
        {
            autumn.SetActive(false);
        }
        if (winterweeks.Contains(week))
        {
            winter.SetActive(true);
        }
        else
        {
            winter.SetActive(false);
        }
    }
}
