using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class EndingStage :  MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            StartCoroutine(LoadScene());
        }
    }
    IEnumerator LoadScene()
    {
        var obj = FindObjectOfType<PlayerScemeManage>();
        yield return new WaitForSeconds(3.0f);

        if (obj != null)
        {
            obj.clueCnt = 0;
            if (obj.TrueEndOn)
            {
                obj.TrueEndOn = false;               
                SceneManager.LoadScene("True_Ending");
            }
            else
            {
                SceneManager.LoadScene("Normal_Ending");
            }

            
        }
    }

    private void Update()
    {
        
    }
}
