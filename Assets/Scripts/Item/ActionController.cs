using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Text를 쓰기 위해서
using UnityEngine.SceneManagement; // F4층으로 왔을 때를 위함
public class ActionController : MonoBehaviour
{
    private Animator Door;
    [SerializeField]
    private float range; // 습득 가능 사정 거리

    private bool pickupActivated = false; // 습득 가능할시 true 하기 위해

    [Header("책 오디오")] public AudioClip BookAudioClip;
    [Header("키 오디오")] public AudioClip KeyaudioClip;
    [Header("문 오디오")] public AudioClip DooraudioClip;
    [Header("키 생성 오디오")] public AudioClip KeyActivateaudioClip;


    [Header("손전등 오디오")] public AudioClip FlashlightaudioClip;
    [Header("총 오디오")] public AudioClip PistolaudioClip;
    [Header("배터리 오디오")] public AudioClip BatteryaudioClip;
    [Header("총알 오디오")] public AudioClip BulletaudioClip;
    [Header("잠금문 열림 오디오")] public AudioClip UnlockDooraudioClip;

    private AudioSource playerAudioSource;
    private RaycastHit hitInfo; // 충돌체 정보 저장, 레이저가 물체에 다으면 충돌체의 정보를 담아오기 위해 Hit

    [SerializeField]
    private LayerMask layerMask; // 아이템에 레이어에 대해서만 반응하기 위해 레이어 마스크를 설정

    [SerializeField]
    private FlashLight flashLight;

    [SerializeField]
    private GunController gunController;

    [SerializeField]
    private GameObject pistol;

    [SerializeField]
    private GameObject hud;

    [SerializeField]
    private GameObject hud_battery;

    [SerializeField] // 필요한 컴포넌트
    private Text actionText; // 행동을 보여줄 텍스트
    
    [Header("컴퓨터 오디오")]public AudioSource computerAudio;

    public bool SecurityLevel = false;
    private int BookCount = 0;
    [Header("필요한 책 개수")] public int BookFinish = 0;
    [Header("다모으면 켜질 컴퓨터")] public BoxCollider computer = null;
    [Header("컴퓨터 동작시 나올 키")] public GameObject key = null;
    [Header("단서를 다 모았을 때 표시될 키 위치 이미지")] public GameObject keyImg;
    [Header("단서 진척도 표시를 위한 이미지")] public Image findingBookImg;
    

    private Gun playerGun;
    private FlashLight playerflash;
    private bool fundamentalOn;
    private PlayerScemeManage ps;
    public bool TrueEndingOpen;

    void Update()
    {
        if (PauseMenu.GameisPause) return;
        CheckItem(); // 아이템이 있는지 없는지 확인하기 위해서
        TryAction(); // 물건 가져오기 실행
        CollectBook();
        CheckFundamentalItem(); // 4층에서 총과 손전등을 획득 했는지 확인
    }

    private void CheckFundamentalItem()
    {
        if (SceneManager.GetActiveScene().name == "F4")
        {
            if (flashLight.enabled && pistol.activeSelf && !fundamentalOn)
            {               
                fundamentalOn = true;
                GameObject invisibleWall = GameObject.FindGameObjectWithTag("InvisibleWall");
                if (invisibleWall.activeSelf) { 
                    invisibleWall.SetActive(false); 
                }
            }
        }
    }
    private void CollectBook()
    {
        if (BookCount == BookFinish && key != null && !keyImg.activeSelf && SceneManager.GetActiveScene().name != "F2")
        {
            key.SetActive(true);
            keyImg.SetActive(true);
            playerAudioSource.clip = KeyActivateaudioClip;
            playerAudioSource.Play();
        }
    }
    
    private void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();
        computer.enabled = false;
        BookCount = 0;
        //computer.SetActive(false);
        key.SetActive(false);
        fundamentalOn = false;
        TrueEndingOpen = false;
        //if(SceneManager.GetActiveScene().name == "F4") // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            ps = FindObjectOfType<PlayerScemeManage>(); 
        //clueCnt = 0;

        if(SceneManager.GetActiveScene().name == "F4")
        {
            flashLight.enabled = false;
            hud_battery.SetActive(false);
            pistol.SetActive(false);
            hud.SetActive(false);
            gunController.enabled = false;
            if(ps != null)
            {
                ps.clueCnt = 0;
                ps.TrueEndOn = false;
            }
        }
    }
    private void TryAction() // 물건 가져오기 실행
    {
        if (Input.GetKeyDown(KeyCode.E)) // E키를 누르면 실행
        {
            CheckItem(); // 아이템이 있는지 확인
            CanPickUp(); // 가능하면 줍는다.
            
        }
    }
   
    private void CanPickUp() // 가능하면 줍기
    {
        if (pickupActivated) // pickupActivated가 true일 때 실행
        {
            if(hitInfo.transform != null) // hitInfo가 null이 아닐 경우 실행
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득했습니다");
                if (hitInfo.transform.CompareTag("Book"))
                {
                    BookCount++;
                    playerAudioSource.clip = BookAudioClip;
                    playerAudioSource.Play();
                    findingBookImg.fillAmount += 0.2f;
                   Destroy(hitInfo.transform.gameObject); // hitInfo 파괴
                   InfoDisappear();                       // 사라지게
                }
                if (hitInfo.transform.CompareTag("Computer"))
                {
                    SecurityLevel = true;
                    computer.enabled = false;
                    playerAudioSource.clip = UnlockDooraudioClip;
                    playerAudioSource.Play();
                    return;
                }
                if (hitInfo.transform.CompareTag("Key"))
                {
                    playerAudioSource.clip = KeyaudioClip;
                    playerAudioSource.Play();
                    computer.enabled = true;
                    computerAudio.enabled = true;
                    Destroy(hitInfo.transform.gameObject); // hitInfo 파괴
                    InfoDisappear();                       // 사라지게
                }
                if (hitInfo.transform.CompareTag("Flashlight"))
                {
                    playerAudioSource.clip = FlashlightaudioClip;
                    playerAudioSource.Play();
                    flashLight.enabled = true; // 플래시 획득 확인
                    hud_battery.SetActive(true);
                    Destroy(hitInfo.transform.gameObject); // hitInfo 파괴
                    InfoDisappear();                       // 사라지게
                }
                if (hitInfo.transform.CompareTag("Pistol"))
                {
                    playerAudioSource.clip = PistolaudioClip;
                    playerAudioSource.Play();
                    pistol.SetActive(true); // 총 획득 확인
                    hud.SetActive(true);
                    gunController.enabled = true;
                    Destroy(hitInfo.transform.gameObject); // hitInfo 파괴
                    InfoDisappear();                       // 사라지게
                }
                if (hitInfo.transform.CompareTag("Door"))
                {
                    
                    Door = hitInfo.transform.GetComponent<Animator>();
                    bool doorStatusOpen = hitInfo.transform.GetComponent<DoorStatus>().DoorOpen;
                    bool doorStatusClose = hitInfo.transform.GetComponent<DoorStatus>().DoorClose;
                    
                    playerAudioSource.clip = DooraudioClip;
                    playerAudioSource.Play();
                    
                    if (doorStatusClose)
                    {
                        Door.SetTrigger("IsOpen");
                        hitInfo.transform.GetComponent<DoorStatus>().DoorClose = false;
                        hitInfo.transform.GetComponent<DoorStatus>().DoorOpen = true;
                        
                    }
                    else if(doorStatusOpen)
                    {
                        Door.SetTrigger("IsClose");
                        hitInfo.transform.GetComponent<DoorStatus>().DoorOpen = false;
                        hitInfo.transform.GetComponent<DoorStatus>().DoorClose = true;
                        
                    }
                }

                if (hitInfo.transform.CompareTag("EndDoor"))
                {
                    if (SecurityLevel)
                    {
                        Door = hitInfo.transform.GetComponent<Animator>();
                        playerAudioSource.clip = DooraudioClip;
                        Door.SetTrigger("IsOpen");
                        playerAudioSource.Play();
                    }
                }

                if (hitInfo.transform.CompareTag("TrueEnding"))
                {

                    //  if (SceneManager.GetActiveScene().name == "F4") // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  { // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    if (ps != null)
                    {
                        ps.clueCnt++;
                        if (ps.clueCnt == 3) ps.TrueEndOn = true;
                    }
                 //   } // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //    else // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //    { // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAA"); // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //        TrueEndingOpen = true; // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        
                 //   } // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    hitInfo.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
                    InfoDisappear();
                }

                if(hitInfo.transform.CompareTag("Item"))
                {
                    if(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName == "Bullet")
                    {
                        playerGun = FindObjectOfType<Gun>();
                        if (playerGun != null)
                        {
                            playerAudioSource.clip = BulletaudioClip;
                            playerAudioSource.Play();
                            playerGun.carryBulletCount += 7;
                            Destroy(hitInfo.transform.gameObject); // hitInfo 파괴
                            InfoDisappear();                       // 사라지게
                        }
                    }
                    if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemName == "Battery")
                    {
                        playerflash = FindObjectOfType<FlashLight>();
                        if (playerflash != null)
                        {
                            playerAudioSource.clip = BatteryaudioClip;
                            playerAudioSource.Play();
                            playerflash.battery_limit += 10f;
                            playerflash.battery.fillAmount += 0.1f;
                            Destroy(hitInfo.transform.gameObject); // hitInfo 파괴
                            InfoDisappear();                       // 사라지게
                        }
                    }
                }
            }
        }
    }

    
   
    
    
    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask)) // 플레이어의 현재 위치에서 바라보는 정면에 광선을 쏜다, 충동체의 정보를 저장, 광선의 사정거리, layerMask로 아이템과 충돌했을 때만 반응하도록
        {
            if (hitInfo.transform.tag == "Item") // 히트 정보가 Item일 경우에 실행
            {
                ItemInfoAppear();
            }
            
            if (hitInfo.transform.tag == "Book") // 히트 정보가 Book일 경우에 실행
            {
                ItemInfoAppear();
            }
            if (hitInfo.transform.tag == "Computer") // 히트 정보가 Computer일 경우에 실행
            {
                ItemInfoAppear();
            }
            if (hitInfo.transform.tag == "Key") // 히트 정보가 Key일 경우에 실행
            {
                ItemInfoAppear();
            }
            if (hitInfo.transform.tag == "Door" )
            {
                DoorActivate();
            }
            if (hitInfo.transform.tag == "EndDoor")
            {
                EndDoorCheck();
            }
            if(hitInfo.transform.tag == "Flashlight")
            {
                FlashlightActive();
            }
            if(hitInfo.transform.tag == "Pistol")
            {
                PistolActive();
            }
            if(hitInfo.transform.tag == "TrueEnding")
            {
                TrueEndClue();
            }
            
        }
        else
            InfoDisappear();
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true; // 습득 할 수 있도록 true로 바꿔준다.
        actionText.gameObject.SetActive(true); // SetActive를 true로
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>"; // 
    }

    private void EndDoorCheck()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        if(SecurityLevel)
            actionText.text =" 문 상호작용 " + "<color=yellow>" + "(E)" + "</color>";
        else
            actionText.text ="<color=red>" +" 보안레벨 낮음 " + "</color>";
            
    }
    private void DoorActivate()
    {
        pickupActivated = true;                // 습득 할 수 있도록 true로 바꿔준다.
        actionText.gameObject.SetActive(true); // SetActive를 true로
        actionText.text =" 문 상호작용 " + "<color=yellow>" + "(E)" + "</color>"; // 
        
    }
    private void InfoDisappear()
    {
        pickupActivated = false; // 주울수 없게 하기 위해서
        actionText.gameObject.SetActive(false); // 사라지게 하기 위해서
    }

    private void FlashlightActive()
    {
        pickupActivated = true; // 습득 할 수 있도록 true로 바꿔준다.
        actionText.gameObject.SetActive(true); // SetActive를 true로
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
        

    }

    private void PistolActive()
    {
        pickupActivated = true; // 습득 할 수 있도록 true로 바꿔준다.
        actionText.gameObject.SetActive(true); // SetActive를 true로
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
        
    }

    private void TrueEndClue()
    {
        pickupActivated = true; // 습득 할 수 있도록 true로 바꿔준다.
        actionText.gameObject.SetActive(true); // SetActive를 true로
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
        
    }
}



