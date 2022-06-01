using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Image Panel;
    float time = 0f;
    float F_time = 4.0f;
    private PlayerHitManage pm;
    private bool isDead = false;
    private void Awake()
    {
        isDead = false;
        pm = GetComponent<PlayerHitManage>();
    }
    private void Update()
    {
        if(!isDead)
            if (pm.hp <= 0)
            {
                fadeOut();
                isDead = true;
                return;
            }
    }


    public void fadeOut()
    {
        StartCoroutine(FadeFlow());
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
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene(0);
    }
    
}
