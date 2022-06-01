using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerFire : MonoBehaviour
{
    //AudioSource 컴포넌트를 저장할 변수
    private AudioSource _audioSource;
    //Animator 컴포넌트를 저장할 변수
    private Animator animator;
    //주인공 캐릭터의 Transform 컴포넌트
    private Transform playerTr;
    //적 캐릭터의 Transform 컴포넌트
    private Transform enemyTr;
    //애니메이터 컨트롤러에 정의한 파라미터의 해시값을 미리 추출
    private readonly int hashFire = Animator.StringToHash("Fire");

    //다음 발사할 시간 계산용 변수
    private float nextFire = 0.0f;
    //총알 발사 간격
    private readonly float fireRate = 1.5f;
    //주인공을 향해 회전할 속도 계수
    private readonly float damping = 10.0f;

    
    
    //총 발사 여부를 판단할 변수
    public bool isFire = false;
    //총 발사 사운드를 저장할 변수
    //public AudioClip fireSfx;
    public GameObject crawler = null;

    private PlayerHitManage pm;
    void Start()
    {
        //컴포넌트 추출 및 변수 저장
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        //_audioSource = GetComponent<AudioSource>();

        pm = FindObjectOfType<PlayerHitManage>();

    }
	
    void Update()
    {
        if (pm.hp <= 0)
        {
            animator.ResetTrigger(hashFire);
            return;
        }
        if (isFire)
        {
            //현재 시간이 다음 발사 시간보다 큰지를 확인
            if (Time.time >= nextFire)
            {
                Fire();
                //다음 발사 시간 계산
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.5f);
                pm.Hit();
            }
            //주인공이 있는 위치까지의 회전 각도 계산
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            //보간함수를 사용해 점진적으로 회전시킴
            crawler.transform.rotation = Quaternion.Slerp(crawler.transform.rotation, rot, Time.deltaTime * damping);        
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        isFire = false;
    }
    void Fire()
    {
        animator.SetTrigger(hashFire);
        //_audioSource.PlayOneShot(fireSfx, 1.0f);
    }    
}
