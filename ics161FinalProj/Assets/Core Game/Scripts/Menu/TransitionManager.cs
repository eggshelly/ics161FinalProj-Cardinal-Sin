using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance = null;
    public Animator animator;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public IEnumerator FadeToBlack(float duration)
    {
        animator.SetTrigger("FadeOut");
        Debug.Log("Fading out.");

        yield return new WaitForSeconds(duration);
    }

    public IEnumerator FadeToLevel(float duration)
    {
        animator.SetTrigger("FadeIn");
        Debug.Log("Fading in.");
        yield return new WaitForSeconds(duration);
    }
}