using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerManager : MonoBehaviour
{
    public enum State
    {
        IDLE,
        MOVE,
        ATTACK,
        DIE
    }
    public AudioClip die, attack;
    public AudioSource audioSource;
    public GameObject CrawlerParent = null;
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashMove = Animator.StringToHash("IsMove");

    public float crawlerHP = 100.0f;
    
    //상태를 저장할 변수
    public State state = State.IDLE;
    //주인공의 위치를 저장할 변수
    private Transform playerTr;
    //적 캐릭터의 위치를 저장할 변수
    private Transform enemyTr;
    private Animator animator;
    
    private WaitForSeconds ws;
    private CrawlerFire enemyFire;
    
    //private float attackTime = 0.03f;
    public bool isDie = false;
    public bool isAttack = false;
    private void OnTriggerEnter(Collider other)
    {
        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PLAYER"))
        {
            isAttack = true;
        }
        if (collision.collider.CompareTag("BULLET"))
        {
            checkHP();
            crawlerHP -= 30.0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        state = State.IDLE;
        isAttack = false;
    }

    private bool checkHP()
    {
        if (crawlerHP < 20)
        {
            Debug.Log("die");
            state = State.DIE;
            animator.SetTrigger(hashDie);
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            return true;
        }
        return false;
    }

    void Awake()
    {
        //주인공 게임오브젝트 추출
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        
        //주인공의 Transform 컴포넌트 추출
        if (player != null)
            playerTr = player.GetComponent<Transform>();
        //적 캐릭터의 Tranform 컴포넌트 추출
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        enemyFire = GetComponent<CrawlerFire>();
        //코루틴의 지연시간 생성
        ws = new WaitForSeconds(0.5f);
    }

    void OnEnable()
    {
        //CheckState 코루틴 함수 실행
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }
    
    
    
    //적 캐릭터의 상태를 검사하는 코루틴 함수
    IEnumerator CheckState()
    {
        //적 캐릭터가 사망하기 전까지 도는 무한루프
        while (!isDie)
        {
            //상태가 사망이면 코루틴 함수를 종료시킴
            if (state == State.DIE) yield break;
            //공격 사정거리 이내의 경우
            if (isAttack)
            {
                state = State.ATTACK;
            }
            // if (Input.GetKey(KeyCode.E))
            // {
            //     state = State.MOVE;
            // }
            //0.3초 동안 대기하는 동안 제어권을 양보
            yield return ws;
        }
    }
    
    

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.MOVE:
                    animator.SetBool(hashMove, false);
                    this.transform.LookAt(playerTr);
                    break;
                case State.ATTACK:
                    animator.SetBool(hashMove, false);
                    enemyAttackSound();
                    if (enemyFire.isFire == false)
                        enemyFire.isFire = true;
                    break;
                case State.DIE:
                        isDie = true;
                        enemyDieSound();
                        enemyFire.isFire = false;
                        GetComponent<CapsuleCollider>().enabled = false;
                        enemyDieSound();
                        animator.SetTrigger(hashDie);
                        break;
            }
        }
    }
    private void Update()
    {
        if (state == State.DIE)
        {
            enemyDieSound();
            Destroy(CrawlerParent, 4.0f);
        }
    }
    void enemyDieSound()
    {
        if(audioSource.clip == attack && audioSource.isPlaying) audioSource.Stop();
        if(audioSource.isPlaying) return;
        audioSource.clip = die;
        audioSource.Play();
    }
    
    void enemyAttackSound()
    {
        if(audioSource.clip == die && audioSource.isPlaying) audioSource.Stop();
        
        if(audioSource.isPlaying) return;
        audioSource.clip = attack;
        audioSource.Play();
    }
}
