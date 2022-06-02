using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FlashLight : MonoBehaviour
{
    [SerializeField]
    GameObject FlashLightLight;
    public bool flashlightActive;
    public Light _light;
    public Image battery;

    public AudioSource[] playerAudioSources;
    public AudioClip FlashlightToggleSound;
    public float battery_limit;

    private PlayerScemeManage playerSceneManger;
    private Playercontroller2_donghee playerController;
    private void Awake()
    {
        playerSceneManger = FindObjectOfType<PlayerScemeManage>();
    }

    void Start()
    {
        playerController = FindObjectOfType<Playercontroller2_donghee>();
        playerAudioSources = GetComponents<AudioSource>();
        playerAudioSources[1].clip = FlashlightToggleSound;
        _light.intensity = 0.0f;
        FlashLightLight.gameObject.SetActive(false);


        if (SceneManager.GetActiveScene().name == "F4" || playerSceneManger == null) 
            battery_limit = 50f;
        else battery_limit = playerSceneManger.onGoing_battery;

        battery.fillAmount = battery_limit / 100f;
    }

    void Update()
    {
        if (playerController != null && playerController.isDead) return;
        if (battery_limit <= 0f)
        {
            FlashLightLight.gameObject.SetActive(false);
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            playerAudioSources[1].Play();
            if (flashlightActive == false)
            {
                _light.intensity = 4.5f;
                FlashLightLight.gameObject.SetActive(true);
                flashlightActive = true;
                InvokeRepeating("UsingFlashlight", 0f, 1f);
            }
            else
            {
                _light.intensity = 0.0f;
                FlashLightLight.gameObject.SetActive(false);
                flashlightActive = false;
                CancelInvoke("UsingFlashlight");
            }
        }
    } 

    

    void UsingFlashlight()
    {
        battery_limit -= 1f;
        battery.fillAmount = battery_limit / 100f;
        if (battery_limit <= 0f) battery_limit = 0f;
    }
}
