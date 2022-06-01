using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAI : MonoBehaviour {
    //적 캐릭터의 상태를 표현하기 위한 열거형 변수 정의
    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    //public float hp = 100;
    public AudioClip patrol, trace, die, attack;
    public AudioSource audioSource;
    //상태를 저장할 변수
    public State state = State.PATROL;
    //주인공의 위치를 저장할 변수
    private Transform playerTr;
    //적 캐릭터의 위치를 저장할 변수
    private Transform enemyTr;
    private Animator animator;

    //플레이어를 봤을 때 바라보는 시간
    private float seeTime = 0.0f;
    //바라보고 쫓아오는 시간
    private float attackTime = 0.03f;
    
    //공격 사정거리
    public float attackDist = 6.0f;
    //추적 사정거리
    public float traceDist = 10.0f;
    //사망 여부를 판단할 변수
    public bool isDie = false;
    //코루틴에서 사용할 지연시간 변수
    private WaitForSeconds ws;
    private MoveAgent moveAgent;
    private EnemyFire enemyFire;

    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");

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
        moveAgent = GetComponent<MoveAgent>();
        enemyFire = GetComponent<EnemyFire>();
        seeTime = 0.0f;
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
            //주인공과 적 캐릭터 간의 거리를 계산
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);
            //공격 사정거리 이내의 경우
            if (dist <= attackDist)
            {
                state = State.ATTACK;
                enemyAttackSound();
            }//추적 사정거리 이내의 경우
            else if (dist <= traceDist)
            {
                state = State.TRACE;
                enemyTraceSound();
            }
            else
            {
                state = State.PATROL;
                //enemyPatrolSound();
            }
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
                case State.PATROL:
                    seeTime = 0.0f;
                    //enemyPatrolSound();
                    enemyFire.isFire = false;
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove,true);
                    break;
                case State.TRACE:
                    moveAgent.Stop();
                    animator.SetBool(hashMove,false);
                    this.transform.LookAt(playerTr);
                    seeTime += Time.deltaTime;
                    //enemyTraceSound();
                    if (seeTime > attackTime)
                    {
                        enemyFire.isFire = false;
                        moveAgent.traceTarget = playerTr.position;
                        animator.SetBool(hashMove, true);
                    }
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    //enemyAttackSound();
                    animator.SetBool(hashMove,false);
                    //GetComponent<EnemySoundControl>().activeState = EnemySoundControl.EnemyState.ATTACK; // 공격 효과음 재생
                    if (enemyFire.isFire == false)
                        enemyFire.isFire = true;
                    break;
                case State.DIE:
                    isDie = true;
                    enemyFire.isFire = false;
                    enemyDieSound();
                    moveAgent.Stop();
                    animator.SetInteger(hashDieIdx,UnityEngine.Random.Range(0,2));
                    animator.SetTrigger(hashDie);
                    //GetComponent<EnemySoundControl>().activeState = EnemySoundControl.EnemyState.DIE; // 사망 효과음 재생
                    GetComponent<SphereCollider>().enabled = false;
                    break;
            }
        }

    }
    private void Update()
    {
        animator.SetFloat(hashSpeed,moveAgent.speed);
    }
    
    void enemyDieSound()
    {
        if(audioSource.clip == attack && audioSource.isPlaying) audioSource.Stop();
        if(audioSource.clip == trace && audioSource.isPlaying) audioSource.Stop();
        if(audioSource.clip == patrol && audioSource.isPlaying) audioSource.Stop();
        
        if(audioSource.isPlaying) return;
        audioSource.clip = die;
        audioSource.Play();
    }
    
    void enemyAttackSound()
    {
        if(audioSource.clip == trace && audioSource.isPlaying) audioSource.Stop();
        if(audioSource.clip == patrol && audioSource.isPlaying) audioSource.Stop();
        if(audioSource.isPlaying) return;
        audioSource.clip = attack;
        audioSource.Play();
    }
    
    void enemyTraceSound()
    {
        if(audioSource.clip == attack && audioSource.isPlaying) audioSource.Stop();
        if(audioSource.clip == patrol && audioSource.isPlaying) audioSource.Stop();
        
        if(audioSource.isPlaying) return;
        audioSource.clip = trace;
        audioSource.Play();
    }
    void enemyPatrolSound()
    {
        if(audioSource.clip == attack && audioSource.isPlaying) audioSource.Stop();
        if(audioSource.clip == trace && audioSource.isPlaying) audioSource.Stop();
        
        if(audioSource.isPlaying) return;
        audioSource.clip = patrol;
        audioSource.Play();
    }
}
