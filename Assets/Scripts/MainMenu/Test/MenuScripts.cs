using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScripts : MonoBehaviour
{
    //    // Start is called before the first frame update
    //    void Start()
    //    {

    //    }

    //    // Update is called once per frame
    //    void Update()
    //    {

    //    }

    //    // ��ư �ҷ��ͼ� �����ϰ� �ϱ�
    //    public void OnClickNewGame()
    //    {
    //        Debug.Log("�� ����");
    //    }

    //    public void OnClickOption()
    //    {
    //        Debug.Log("�ɼ�");
    //    }

    //    public void OnClickQuit()
    //    {
    //        // �����Ϳ����� ���� ���� �ʱ� ������ #if ��ó���⸦ ���
    //#if UNITY_EDITOR // ����Ƽ �����ͻ󿡼���
    //        UnityEditor.EditorApplication.isPlaying = false;
    //#else
    //        Application.Quit();
    //#endif


    //    }

    public GameObject mainMenuHolder;
    public GameObject optionMenuHolder;


    public void Play()
    {
        //SceneManager.LoadScene ("Game");  // Game �ε�
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Subject()
    {
        mainMenuHolder.SetActive(true); // option���� true
        optionMenuHolder.SetActive(false); // option���� false
    }

    public void OptionMenu()
    {
        mainMenuHolder.SetActive (false); // option���� false
        optionMenuHolder.SetActive (true); // option���� true
    }

    public void MainMenu()
    {
        mainMenuHolder.SetActive(true);  // main������ true
        optionMenuHolder.SetActive(false); // option false
    }

    public void Set_1208x720_Resolution(int i)
    {

    }

    public void Set_3840x2160_Resolution(int i)
    {

    }

    public void SetFullScreen(bool isFullscreen)
    {

    }
    
    public void SetMusicVolume(float value)
    {

    }

    public void SetEffectVolume(float value)
    {

    }
}
