using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneManage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("F3");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene("F2");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("F4");
        }
    }
}
