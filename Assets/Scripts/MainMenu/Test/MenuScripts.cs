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

    //    // 버튼 불러와서 실행하게 하기
    //    public void OnClickNewGame()
    //    {
    //        Debug.Log("새 게임");
    //    }

    //    public void OnClickOption()
    //    {
    //        Debug.Log("옵션");
    //    }

    //    public void OnClickQuit()
    //    {
    //        // 에디터에서는 실행 되지 않기 때문에 #if 전처리기를 사용
    //#if UNITY_EDITOR // 유니티 에디터상에서는
    //        UnityEditor.EditorApplication.isPlaying = false;
    //#else
    //        Application.Quit();
    //#endif


    //    }

    public GameObject mainMenuHolder;
    public GameObject optionMenuHolder;


    public void Play()
    {
        //SceneManager.LoadScene ("Game");  // Game 로드
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Subject()
    {
        mainMenuHolder.SetActive(true); // option에서 true
        optionMenuHolder.SetActive(false); // option에서 false
    }

    public void OptionMenu()
    {
        mainMenuHolder.SetActive (false); // option에서 false
        optionMenuHolder.SetActive (true); // option에서 true
    }

    public void MainMenu()
    {
        mainMenuHolder.SetActive(true);  // main에서는 true
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
