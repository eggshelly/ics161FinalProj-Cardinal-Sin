using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] int maxNumberOfWeeks;
    [SerializeField] string textBetweenWeeks = "One Week Later...";
    [SerializeField] GameObject timeText;
    [SerializeField] TextMeshProUGUI betweenWeekText;
    public static TimeManager instance = null;

    public UnityEvent NextWeekEvent;

    TextMeshProUGUI text;

    int week = 1;

    bool CR_started = false;
    //int day = 1;

    //Dictionary<int, string> daysOfTheWeek;

    //string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

    private void Awake()
    {
        NextWeekEvent = new UnityEvent();
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        //CreateDaysOfWeek();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
        DialogueManager.instance.EndOfWeek.AddListener(EndOfWeekListener);

    }

    /*void CreateDaysOfWeek()
    {
        daysOfTheWeek = new Dictionary<int, string>();
        for(int i = day; i <= daysInTheWeek; ++i)
        {
            daysOfTheWeek[i] = days[i - 1];
        }
    }*/

    public void SetDay(int week)
    {
        this.week = week;
        Debug.Log("Set day being called");
        UpdateText();
    }

    void NextWeek()
    {
        week += 1;
        UpdateText();
        NextWeekEvent.Invoke();
        SaveFileManager.instance.DoneWithEndDialogue();
    }

    public int GetCurrentWeek()
    {
        return week;
    }

    void UpdateText()
    {
        text.text= string.Format("Week {0}", week);
    }

    void EndOfWeekListener()
    {
        if (!CR_started)
        {
            CR_started = true;
            Debug.Log("Here");
            StartCoroutine(EndOfWeekFade());
        }
    }

    IEnumerator EndOfWeekFade()
    {
        yield return StartCoroutine(TransitionManager.instance.screenFadeOut);
        DialogueManager.instance.HideBackground();
        StartCoroutine(EndOfWeekText());        
    }

    IEnumerator EndOfWeekText()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = Vector3.zero;
        NextWeek();
        betweenWeekText.gameObject.SetActive(true);
        foreach (var c in textBetweenWeeks)
        {
            betweenWeekText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        betweenWeekText.text = "";
        yield return StartCoroutine(TransitionManager.instance.screenFadeIn);
        CR_started = false;
        betweenWeekText.gameObject.SetActive(false);
    }


    void OnSceneLoaded(Scene loadedScene, LoadSceneMode sceneMode)
    {
        if (loadedScene == SceneManager.GetSceneByName("TestMap"))
        {
            text = timeText.GetComponent<TextMeshProUGUI>();
            timeText.SetActive(true);
            UpdateText();
        }
        else
        {
            timeText.SetActive(false);
        }
    }






}
