using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool GameisPause = false;

    [SerializeField] public GameObject Panel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameisPause)
            {
                //Cursor.visible = true;
                //Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Resume();
            }
            else
            {
                //Cursor.visible = false;   
                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Pause();
            }
            
        }
    }

    public void Resume()
    {
        Panel.SetActive(false);
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameisPause = false;
        Time.timeScale = 1f;
    }

    void Pause()
    {
        Panel.SetActive(true);
        GameisPause = true;
        Time.timeScale = 0f;
    }

    
    public void ClickHome()
    {
        
        SceneManager.LoadScene("Main");
    }
    public void ClickExit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

}
