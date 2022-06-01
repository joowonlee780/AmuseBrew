using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScemeManage : MonoBehaviour
{
    private float start_hp;
    private int start_carryBulletCount; // 현재 소지하고 있는 총알 개수
    private int start_currentBulletCount; // 현재 탄알집에 남아 있는 총알 개수
    private float start_battery;

    public float onGoing_hp;
    public int onGoing_carryBulletCount;
    public int onGoing_currentBulletCount;
    public float onGoing_battery;

    private PlayerHitManage playHp;
    private Gun playerGun;
    private FlashLight playerflashlight;
    private ActionController TrueEnd;
    public bool TrueEndOn;
    public int clueCnt;

    // Start is called before the first frame update
    private void Awake()
    {
        start_hp = 100f;
        start_carryBulletCount = 0;
        start_currentBulletCount = 0;
        start_battery = 50f;
        TrueEndOn = false;
        clueCnt = 0;
        // 씬으로 넘어갈 때 삭제되지 않는 정보들을 담기 위해 선언함
        
        // 게임을 처음 시작할 때 최초 hp와 잔탄으로 onGoing을 설정함
        onGoing_hp = start_hp;
        onGoing_carryBulletCount = start_carryBulletCount;
        onGoing_currentBulletCount = start_currentBulletCount;
        onGoing_battery = start_battery;

        var obj = FindObjectsOfType<PlayerScemeManage>(); 
        if (obj.Length == 1) { 
            DontDestroyOnLoad(gameObject); 
        } else {
            Destroy(gameObject); 
        }

        Time.timeScale = 1f;
        PauseMenu.GameisPause = false;
        
    }

    private void Start()
    {
        
        
        TrueEnd = FindObjectOfType<ActionController>();
        //playerGun = FindObjectOfType<Gun>();

        
    }

    private void Update()
    {
        playHp = FindObjectOfType<PlayerHitManage>();
        playerGun = FindObjectOfType<Gun>();
        playerflashlight = FindObjectOfType<FlashLight>();
        // onGoing은 플레이 중 누적되는 hp, 잔탄의 변화를 갱신함
        if (playHp != null)
        {
            onGoing_hp = playHp.hp;
        }
        if (playerGun != null)
        {
            onGoing_currentBulletCount = playerGun.currentBulletCount;
            onGoing_carryBulletCount = playerGun.carryBulletCount;
        }

        if(playerflashlight != null)
        {
            onGoing_battery = playerflashlight.battery_limit;
        }

        
        
    }
}
