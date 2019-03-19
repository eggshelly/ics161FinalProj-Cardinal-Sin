using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] int week = 1;
    [SerializeField] string[] textBetweenWeeks;
    [SerializeField] GameObject timeText;
    [SerializeField] TextMeshProUGUI betweenWeekText;
    public static TimeManager instance = null;

    public UnityEvent NextWeekEvent;

    TextMeshProUGUI text;

    bool CR_started = false;

    int maxNumberOfWeeks;


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
        
    }

    // Start is called before the first frame update
    void Start()
    {
        maxNumberOfWeeks = GameObject.Find("StageHub").GetComponent<StageHubScript>().GetTotalNumLevels();
        DialogueManager.instance.AllWeeksFinished.AddListener(AllWeeksFinishedListener);
        DialogueManager.instance.EndOfWeek.AddListener(EndOfWeekListener);

    }

    public void SetDay(int week)
    {
        this.week = week;
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
        NextWeek();
        StartCoroutine(EndOfWeekText(textBetweenWeeks[(int)(Random.value * textBetweenWeeks.Length)]));
        
    }

    IEnumerator EndOfWeekText(string text)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = Vector3.zero;
        betweenWeekText.gameObject.SetActive(true);
        foreach (var c in text)
        {
            betweenWeekText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        betweenWeekText.text = "";
        CR_started = false;
        betweenWeekText.gameObject.SetActive(false);
        if (week > maxNumberOfWeeks)
        {
            DialogueManager.instance.doneWithGame = true;
            DialogueManager.instance.LoadDialogue("CONCLUSION");
        }
        else
        {
            FindObjectOfType<AudioManager>().DialogueTransitionSong("INTRODUCTION");
            player.GetComponent<PlayerMapInteraction>().CanInteract();
            player.GetComponent<PlayerMapMovement>().CanMove();
            yield return StartCoroutine(TransitionManager.instance.screenFadeIn);
        }
        
            
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

    void AllWeeksFinishedListener()
    {
        StartCoroutine(EndingText());

    }

    IEnumerator EndingText()
    {
        yield return StartCoroutine(EndOfWeekText("More levels coming soon!\nThank you for playing!")); //temporary
        SaveFileManager.instance.SaveGame();
        SceneManager.LoadScene("Credits");
        StartCoroutine(TransitionManager.instance.screenFadeIn);
    }






}
