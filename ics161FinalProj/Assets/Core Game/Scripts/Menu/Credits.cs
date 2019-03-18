using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roleText;
    [SerializeField] TextMeshProUGUI namesText;
    [SerializeField] float fadeTime;
    [SerializeField] float displayTime;

    Dictionary<string, string[]> roles;

    bool CR_running = false;

    // Start is called before the first frame update
    void Start()
    {
        roles = new Dictionary<string, string[]>();
        roles["Designer"] = new string[]{ "Timothy Quach"};
        roles["Artist"] = new string[]{ "Michelle Wang"};
        roles["Writer"] = new string[]{ "Michaela Gonzales"};
        roles["Programmers"] = new string[]{ "Josh Lebow", "Johnny Vong", "Danny Qi"};
        namesText.alpha = 0;
        roleText.alpha = 0;
        StartCoroutine(CycleThroughRoles());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CycleThroughRoles()
    {
        foreach (string role in roles.Keys)
        { 
            while(CR_running)
            {
                yield return null;
            }
            string names = "";
            foreach(string name in roles[role])
            {
                names += name + '\n';
            }
            names = names.Substring(0, names.Length - 1);
            roleText.text = role;
            namesText.text = names;
            StartCoroutine(TextFadeIn());
        }
        StartCoroutine(CycleThroughRoles());
    }

    IEnumerator TextFadeIn()
    {
        CR_running = true;
        while(roleText.alpha < 1f)
        {
            roleText.color = new Color(roleText.color.r, roleText.color.g, roleText.color.b, roleText.alpha + (Time.deltaTime / fadeTime));
            namesText.color = new Color(namesText.color.r, namesText.color.g, namesText.color.b, namesText.alpha + (Time.deltaTime / fadeTime));
            yield return null;
        }
        yield return new WaitForSeconds(displayTime);
        StartCoroutine(TextFadeOut());
    }

    IEnumerator TextFadeOut()
    {
        while (roleText.alpha > 0f)
        {
            roleText.color = new Color(roleText.color.r, roleText.color.g, roleText.color.b, roleText.alpha - (Time.deltaTime / fadeTime));
            namesText.color = new Color(namesText.color.r, namesText.color.g, namesText.color.b, namesText.alpha - (Time.deltaTime / fadeTime));
            yield return null;
        }
        CR_running = false;
    }

    public void stopCoroutine()
    {
        StopCoroutine(CycleThroughRoles());
        StopCoroutine(TextFadeIn());
        StopCoroutine(TextFadeOut());
    }


}
