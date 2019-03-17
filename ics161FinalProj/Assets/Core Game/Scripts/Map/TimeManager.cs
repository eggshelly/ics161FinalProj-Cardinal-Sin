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
    [SerializeField] string[] textBetweenWeeks;
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

        text = timeText.GetComponent<TextMeshProUGUI>();
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
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerMapInteraction>().CantInteract();
            player.GetComponent<PlayerMapMovement>().CantMove();
            CR_started = true;
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = Vector3.zero;
        NextWeek();
        betweenWeekText.gameObject.SetActive(true);
        foreach (var c in textBetweenWeeks[(int)(Random.value*textBetweenWeeks.Length)])
        {
            betweenWeekText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        betweenWeekText.text = "";
        FindObjectOfType<AudioManager>().DialogueTransitionSong("INTRODUCTION");
        yield return StartCoroutine(TransitionManager.instance.screenFadeIn);
        player.GetComponent<PlayerMapInteraction>().CanInteract();
        player.GetComponent<PlayerMapMovement>().CanMove();
        AllWeeksFinished();
        CR_started = false;
        betweenWeekText.gameObject.SetActive(false);
    }


    void OnSceneLoaded(Scene loadedScene, LoadSceneMode sceneMode)
    {
        if (loadedScene == SceneManager.GetSceneByName("TestMap"))
        {
                UpdateText();
        }
        else
        {
            text.text = "";
        }
    }

    void AllWeeksFinished()
    {
        if(week >= maxNumberOfWeeks)
        {
            //do smth
        }
    }






}
