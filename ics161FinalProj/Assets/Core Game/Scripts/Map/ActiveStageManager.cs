using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStageManager : MonoBehaviour
{
    [SerializeField] List<int> springweeks;
    [SerializeField] List<int> summerweeks;
    [SerializeField] List<int> autumnweeks;
    [SerializeField] List<int> winterweeks;

    GameObject[] allStages;
    Dictionary<GameObject, List<int>> stageWeeks;


    // Start is called before the first frame update
    void Awake()
    {
        GetStages();
     }

    private void Start()
    {
        TimeManager.instance.NextWeekEvent.AddListener(ActivateWeeks);
        ActivateWeeks();
    }

    void GetStages()
    {

        Stage[] s = GetComponent<StageHubScript>().GetStages();
        stageWeeks = new Dictionary<GameObject, List<int>>();
        allStages = new GameObject[s.Length];
        for(int i = 0; i < s.Length; ++i)
        {
            allStages[i] = s[i].gameObject;
            string n = s[i].gameObject.name;
            switch (n)
            {
                case "SPRING":
                    stageWeeks[allStages[i]] = springweeks;
                    break;
                case "WINTER":
                    stageWeeks[allStages[i]] = winterweeks;
                    break;
                case "SUMMER":
                    stageWeeks[allStages[i]] = summerweeks;
                    break;
                case "AUTUMN":
                    stageWeeks[allStages[i]] = autumnweeks;
                    break;
                

            }

        }
    }

    void AllInactive()
    {
        foreach(GameObject g in allStages)
        {
            g.SetActive(false);
        }
    }
    void ActivateWeeks()
    {
        int week = TimeManager.instance.GetCurrentWeek();
        foreach(GameObject g in allStages)
        {
            if(stageWeeks[g].Contains(week))
            {
                g.SetActive(true);
            }
            else
            {
                g.SetActive(false);
            }
        }
    }
}
