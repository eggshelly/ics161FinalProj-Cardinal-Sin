using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance = null;
    public Animator animator;
    public IEnumerator screenFadeIn;
    public IEnumerator screenFadeOut;
    private bool CR_Run = false;
    private Color invisible;
    private Color changingColor;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        screenFadeIn = FadeToLevel(2.1f);
        screenFadeOut = FadeToBlack(2.1f);
    }

    public IEnumerator FadeToBlack(float duration)
    {
        screenFadeIn = FadeToLevel(2.1f);
        StopCoroutine(screenFadeIn);
        animator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(duration);
    }

    public IEnumerator FadeToLevel(float duration)
    { 
        screenFadeOut = FadeToBlack(2.1f);    //resets variable to a fresh coroutine
        StopCoroutine(screenFadeOut);           //stops the opposite coroutine when this one starts
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(duration);
    }

        public void BGFadeFirst()   //when scene first opens up, background starts solid
    {
        DialogueManager.instance.bgPanel.GetComponent<Image>().sprite = DialogueManager.instance.currentBG;
        DialogueManager.instance.bgPanel2.GetComponent<Image>().sprite = DialogueManager.instance.currentBG;
        invisible = DialogueManager.instance.bgPanel2.GetComponent<Image>().color;
        invisible.a = 0f;
        DialogueManager.instance.bgPanel2.GetComponent<Image>().color = invisible;
        DialogueManager.instance.bgPanel.SetActive(true);
    }
            
    public void BGFadeSecond()  //for background transitions during dialogue
    {
        DialogueManager.instance.bgPanel2.GetComponent<Image>().sprite = DialogueManager.instance.currentBG;
        invisible = DialogueManager.instance.bgPanel2.GetComponent<Image>().color;
        invisible.a = 0f;
        DialogueManager.instance.bgPanel2.SetActive(true);    //must be invisible at this point
        if(DialogueManager.instance.bgPanel.GetComponent<Image>().sprite != DialogueManager.instance.bgPanel2.GetComponent<Image>().sprite && CR_Run == false)  
        {

            if(CR_Run == false)   //ensures that this is only called once so that if user clicks again during this routine, it will only register once and still work properly
            {
                StartCoroutine(bgFadeIn(0.75f));
                StartCoroutine(WaitForCR());
            }
            else     //addresses case where user clicks so fast that next coroutine tries to start before the last fade in effect has finished. this will force it to wait in theory
            {
                StartCoroutine(Wait());
                StartCoroutine(bgFadeIn(1.15f));
                StartCoroutine(WaitForCR());
            }
        }
    }

    public void BGFadeZero()   //for fading in the background when a new conversation is started mid scene
    {
        DialogueManager.instance.bgPanel.GetComponent<Image>().sprite = DialogueManager.instance.currentBG;
        DialogueManager.instance.bgPanel2.GetComponent<Image>().sprite = DialogueManager.instance.currentBG;
        invisible = DialogueManager.instance.bgPanel2.GetComponent<Image>().color;
        invisible.a = 0f;
        DialogueManager.instance.bgPanel2.GetComponent<Image>().color = invisible;
        DialogueManager.instance.bgPanel2.SetActive(true);        //bgpanel2 alpha value must be initialized as invisible
        StartCoroutine(bgFadeIn(0.75f));     //this will fade in panel2
        StartCoroutine(WaitCR());
    }

    public void BGFadeToMap()
    {
        //DialogueManager.instance.bgPanel2.GetComponent<Image>().sprite = null;
        StartCoroutine(bgFadeOut(0.75f));
        StartCoroutine(WaitFadeOut());
    }

    
    public IEnumerator bgFadeIn(float fadeTime)
    {
        CR_Run = true;
        changingColor = DialogueManager.instance.bgPanel2.GetComponent<Image>().color;
        while(DialogueManager.instance.bgPanel2.GetComponent<Image>().color.a < 1f)
        {
            changingColor.a += Time.deltaTime / fadeTime;
            DialogueManager.instance.bgPanel2.GetComponent<Image>().color = changingColor;
            yield return null;
        }
        CR_Run = false;
    }

    public IEnumerator bgFadeOut(float fadeTime)
    {
        CR_Run = true;
        changingColor = DialogueManager.instance.bgPanel.GetComponent<Image>().color;
        while(DialogueManager.instance.bgPanel.GetComponent<Image>().color.a > 0f)
        {
            changingColor.a -= Time.deltaTime / fadeTime;
            DialogueManager.instance.bgPanel.GetComponent<Image>().color = changingColor;
            yield return null;
        }
        CR_Run = false;
        

    }

    private IEnumerator WaitCR()  //coroutine used for the starting dialogue mid scene
    {
        while (CR_Run)
            yield return null;
        DialogueManager.instance.bgPanel.SetActive(true);
        DialogueManager.instance.bgPanel2.GetComponent<Image>().color = invisible;
        DialogueManager.instance.bgPanel2.SetActive(false);
    }

    private IEnumerator WaitForCR()  //coroutine for transitions within dialogue
    {
        while(CR_Run)
            yield return null;
        DialogueManager.instance.bgPanel.GetComponent<Image>().sprite = DialogueManager.instance.currentBG;
        DialogueManager.instance.bgPanel2.GetComponent<Image>().color = invisible;
        DialogueManager.instance.bgPanel2.SetActive(false);
    }

    public IEnumerator WaitFadeOut()
    {
        while(CR_Run)
        {
            yield return null;
        }
        //DialogueManager.instance.bgPanel2.GetComponent<Image>().sprite = null;
        DialogueManager.instance.bgPanel.SetActive(false);
        changingColor.a = 1f;
        DialogueManager.instance.bgPanel.GetComponent<Image>().color = changingColor;
        
    }
    public IEnumerator Wait()
    {
        while(CR_Run)
            yield return null;
    }
}