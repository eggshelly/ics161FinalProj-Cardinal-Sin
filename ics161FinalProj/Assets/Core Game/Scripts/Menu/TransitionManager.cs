using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;
    public Animator animator;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(instance);
    }

    public IEnumerator FadeToBlack(float duration)
    {
        animator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(duration);
    }

    public IEnumerator FadeToLevel(float duration)
    {
        animator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(duration);
    }
}