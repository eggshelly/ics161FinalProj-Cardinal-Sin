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

        public void BGFadeFirst()
    {
        DialogueManager.instance.bgPanel.GetComponent<Image>().sprite = DialogueManager.instance.currentBG;
        DialogueManager.instance.bgPanel2.GetComponent<Image>().sprite = DialogueManager.instance.currentBG;
        invisible = DialogueManager.instance.bgPanel2.GetComponent<Image>().color;
        invisible.a = 0f;
        DialogueManager.instance.bgPanel2.GetComponent<Image>().color = invisible;
        DialogueManager.instance.bgPanel.SetActive(true);
    }
            
    public void BGFadeSecond()
    {
        DialogueManager.instance.bgPanel2.GetComponent<Image>().sprite = DialogueManager.instance.currentBG;
        invisible = DialogueManager.instance.bgPanel2.GetComponent<Image>().color;
        invisible.a = 0f;
        DialogueManager.instance.bgPanel2.SetActive(true);    //must be invisible at this point
        if(DialogueManager.instance.bgPanel.GetComponent<Image>().sprite != DialogueManager.instance.bgPanel2.GetComponent<Image>().sprite && CR_Run == false)  
        {

            if(CR_Run == false)   //ensures that this is only called once so that if user clicks again during this routine, it will only register once and still work properly
            {
                StartCoroutine(bgFadeIn(1.15f));
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

    public void BGFadeToMap()
    {
        DialogueManager.instance.bgPanel2.GetComponent<Image>().sprite = null;
        StartCoroutine(bgFadeOut(2.0f));
        StartCoroutine(WaitFadeOut());
    }

    
    public IEnumerator bgFadeIn(float fadeTime)
    {
        CR_Run = true;
        Image tempImage = DialogueManager.instance.bgPanel2.GetComponent<Image>();
        float alphaVal = 0f;
        while(DialogueManager.instance.bgPanel2.GetComponent<Image>().color.a < 1f)
        {
            alphaVal += Time.deltaTime / fadeTime;
            DialogueManager.instance.bgPanel2.GetComponent<Image>().color = new Color(tempImage.color.r, tempImage.color.g, tempImage.color.b, alphaVal);
            yield return null;
        }
        CR_Run = false;
    }

    public IEnumerator bgFadeOut(float fadeTime)
    {
        CR_Run = true;
        Image tempImage = DialogueManager.instance.bgPanel.GetComponent<Image>();
        float alphaVal = 0f;
        while(DialogueManager.instance.bgPanel.GetComponent<Image>().color.a > 0f)
        {

            alphaVal -= Time.deltaTime / fadeTime;
            DialogueManager.instance.bgPanel.GetComponent<Image>().color = new Color(tempImage.color.r, tempImage.color.g, tempImage.color.b, alphaVal);
            yield return null;
        }
        CR_Run = false;
        

    }

    private IEnumerator WaitCR()
    {
        while (CR_Run)
            yield return null;
        DialogueManager.instance.bgPanel.SetActive(true);
        DialogueManager.instance.bgPanel2.GetComponent<Image>().color = invisible;
        DialogueManager.instance.bgPanel2.SetActive(false);
    }

    private IEnumerator WaitForCR()
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
        //DialogueManager.instance.HideBackground();
        //DialogueManager.instance.HidePanels();
        
    }
    public IEnumerator Wait()
    {
        while(CR_Run)
            yield return null;
    }
}