using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    [SerializeField] int daysInTheWeek = 5;
    [SerializeField] TextMeshProUGUI timeText;
    public static TimeManager instance = null;

    int week = 1;
    int day = 1;

    Dictionary<int, string> daysOfTheWeek;

    string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        CreateDaysOfWeek();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    void CreateDaysOfWeek()
    {
        daysOfTheWeek = new Dictionary<int, string>();
        for(int i = day; i <= daysInTheWeek; ++i)
        {
            daysOfTheWeek[i] = days[i - 1];
        }
    }

    public void SetDay(int week, int day)
    {
        this.week = week;
        this.day = day;
        UpdateText();
    }

    public void NextDay()
    {
        GetText();
        day += 1; 
        if(day > daysInTheWeek)
        {
            day = 1;
            week += 1; 
        }
        UpdateText();
        SaveFileManager.instance.DoneWithEndDialogue();
    }

    void GetText()
    {
        GameObject text = GameObject.Find("Time Text");
        Debug.Log("Text");
        if (text != null)
        {
            timeText = text.GetComponent<TextMeshProUGUI>();
        }
    }

    public int GetCurrentDay()
    {
        return day;
    }

    public int GetCurrentWeek()
    {
        return week;
    }

    void UpdateText()
    {
        timeText.text = string.Format("{0}, Week {1}", daysOfTheWeek[day], week);
    }

    void OnSceneLoaded(Scene loadedScene, LoadSceneMode sceneMode)
    {
        if(loadedScene == SceneManager.GetSceneByName("TestMap"))
        {
            GetText();
            UpdateText();
        }
    }




}
