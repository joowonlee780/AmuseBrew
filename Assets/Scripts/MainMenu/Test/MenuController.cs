using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Text Mesh Pro

public class MenuController : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;             // 재설정 하기위한 변수 만들기

    /*
    [Header("Gameplay Settings")]
    [SerializeField] private Text controllerSenTextValue = null;
    //[SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    [SerializeField] private int defaultSen = 4;
    public int mainControllerSen = 4;                                       // 다른 스크립트에 쓸 수 있도로 public

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null;
    */

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1;

    private float _brightnessLevel;
    private bool _isFullScreen;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Resolution DropDowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        
    }

    [Header("Let's get to the game")]
    public string _newGame;
    // private string leveltoload;  // 로드 하기 위해 생성 변수
    // [SerializeField] private GameObject noSavedGameD = null; // 세이브 안할경우의 변수

    public void StartGameDialogYes()              // StartDialog 불러오기
    {
        SceneManager.LoadScene(_newGame);  // 씬 매니저로 _newgame 로드하기.
    }

    //public void LoadGameDialogYes()
    //{
    //    // PlayerPrefs는 유니티에서 제공하는 데이터 관리 클래스
    //    // HasKey 는 키 값이 존재하면 true 를 반환한다. 없으면 초기값을 키 값으로 설정한다.
    //    // if문은 true일 때 SavedLevel을 형성 그렇지 않을 때 noSavedGameD가 true
    //    if (PlayerPrefs.HasKey("SavedLevel"))
    //    {
    //        leveltoload = PlayerPrefs.GetString("Savedlevel");
    //        SceneManager.LoadScene(leveltoload);
    //    }
    //    else
    //    {
    //        noSavedGameD.SetActive(true);
    //    }
    //}

    public void ExitButton()
    {
        Application.Quit();
    }


    public void SetVolume(float volume) // 싱글 오디오 함수
    {
        // 오디오 리스너는 마이크와 같은 장치로 오디오 소스(Audio Source)로 부터 정보를 받아 사운드를 재생하는 역할을 한다.
        // 프로젝트 생성 시 Main Camera에 추가되어 있으며, 수정을 위한 옵션 설정을 제공하지 않는다.
        // 오디오 리스너를 제거하거나, 비활성 시에는 사운드를 재생하지 않는다.
        // 오디오 소스를 추가하여 사운드를 재생할 때, 오디오 리스너가 없다면 다음과 같은 메시지가 출력된다. 
        AudioListener.volume = volume;


        // 일반적인 text를 쓰면 상관 없지만
        // Text Mesh Pro를 쓰면 위에 네임스페이스를 TMPro 추가해주고 데이터 타입을 TMP_Text 로 바꾸어 준다
        volumeTextValue.text = volume.ToString("0.0"); // ToString 메서드는 데이터 형식을 문자열로 변환해 준다.
    }

    public void VolumeApply()
    {
        // PlayerPrefs는 유니티에서 제공하는 데이터 관리 클래스
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume); // AudioListener.volume을 저장한다.
        // Show Prompt
        StartCoroutine(Confirmationbox());
        //한 컴포넌트 내에서 Update 함수와 따로 일시적으로 돌아가는 서브 동작을 구현하거나,
        //어떤 다른 작업이 처리되는 것을 기다리는 기능을 구현하는데 쓰이는 것이 바로 코루틴이다.
    }

    //public void SetControllerSen(float sensitivity)  // 슬라이더를 우리가 업데이트 할때 정수형으로 나오니까 실수형을 정수형으로 변환하는 일이 필요..
    //{
    //    mainControllerSen = Mathf.RoundToInt(sensitivity);
    //    controllerSenTextValue.text = sensitivity.ToString("0"); // float형을 int형으로
    //}

    //public void GameplayApply()
    //{
    //    if(invertYToggle.isOn) // 토글이 On(true) 일때
    //    {
    //        PlayerPrefs.SetInt("masterInvertY", 1);     // 1 true  invertY
    //    }
    //    else
    //    {
    //        PlayerPrefs.SetInt("masterInvertY", 0);    // 0 false not InvertY
    //    }

    //    PlayerPrefs.SetFloat("masterSen", mainControllerSen);
    //    StartCoroutine(Confirmationbox());
    //}

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool isFullscreen)
    {
        _isFullScreen = isFullscreen;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);

        PlayerPrefs.SetInt("masterFullscreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;
        StartCoroutine(Confirmationbox());
    }

    public void ResetButton(string MenuType)
    {
        if(MenuType == "Graphics")
        {
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");

            Resolution currentResolutin = Screen.currentResolution;
            Screen.SetResolution(currentResolutin.width, currentResolutin.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }

        if(MenuType == "Audio")                             // 메뉴 타입이 Audio일 경우
        {
            AudioListener.volume = defaultVolume; // volume을 디폴트로 만들고
            volumeSlider.value = defaultVolume;     // 슬라이더도 디폴트 값으로 만들어준다.
            volumeTextValue.text = defaultVolume.ToString("0.0");       // 볼륨 텍스트도 0.0으로 만들어 준다.
            VolumeApply();                                     // VolumeApply() 함수 호출
        }

        //if(MenuType == "Gameplay")
        //{
        //    controllerSenTextValue.text = defaultSen.ToString("0");    // default 0 값으로 텍스트숫자를 변경해준다 -> 문자열을 숫자로 변환 시켜줌
        //    controllerSenSlider.value = defaultSen;                             // 슬라이더 값도 디폴트 값으로 만들어준다
        //    mainControllerSen = defaultSen;
        //    invertYToggle.isOn = false;                                             // 토글도 false로 만들어준다.
        //    GameplayApply();
        //}
    }
    

    public IEnumerator Confirmationbox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2); // 게임시간으로 2초 기다림
        confirmationPrompt.SetActive(false); // 2초 하고 SetActive를 false로 만들어 준다.
    }
}
