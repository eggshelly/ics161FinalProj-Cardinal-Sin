using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance = null;
    public Animator animator;
    public IEnumerator screenFadeIn;
    public IEnumerator screenFadeOut;

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
        Debug.Log("Fading out.");

        yield return new WaitForSeconds(duration);
    }

    public IEnumerator FadeToLevel(float duration)
    {
        screenFadeOut = FadeToBlack(2.1f);    //resets variable to a fresh coroutine
        StopCoroutine(screenFadeOut);           //stops the opposite coroutine when this one starts
        animator.SetTrigger("FadeIn");
        Debug.Log("Fading in.");
        yield return new WaitForSeconds(duration);
    }
}