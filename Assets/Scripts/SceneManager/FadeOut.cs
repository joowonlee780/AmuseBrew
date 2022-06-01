using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeOut : MonoBehaviour
{
    public Image Panel;
    float time = 0f;
    float F_time = 3.0f;
    AudioSource audioClip;

    private void Awake()
    {
        audioClip = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            fadeOut();
        }
    }


    public void fadeOut()
    {
        StartCoroutine(FadeFlow());
        audioClip.Play();
    }
    IEnumerator FadeFlow()
    {
        Panel.gameObject.SetActive(true);
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            Panel.gameObject.SetActive(true);
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        time = 0;
        yield return null;
    }
    
}
