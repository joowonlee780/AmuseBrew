using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeIn : MonoBehaviour
{
    public Image Panel;
    float time = 0f;
    float F_time = 3.0f;
    private void Awake()
    {
        fadeIn();
    }
    public void fadeIn()
    {
        StartCoroutine(FadeFlow());
    }
    IEnumerator FadeFlow()
    {
        
        Color alpha = Panel.color;
        while (alpha.a > 0f)
        {
            Panel.gameObject.SetActive(true);
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);
        time = 0;
        yield return null;
    }
    
    
}
