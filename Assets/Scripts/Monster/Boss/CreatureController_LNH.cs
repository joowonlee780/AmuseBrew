using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureController_LNH : MonoBehaviour
{
    // Start is called before the first frame update
    public enum State
    {
        SUMMONED,
        TRACE,
        ATTACK,
        DIE
    }
    public State state = State.SUMMONED;
    private Animator c_animator;
    public int c_hp;
    private Transform player;
    private Vector3 center_player;
    private NavMeshAgent creatureAgent;
    private bool isDie;
    private bool isHowl;
    public float attackDist = 6.0f;
    private float distance = 100f;

    private Playercontroller2_donghee pd;
    private GameObject bloodEffect; // 혈흔 이펙트
    private BossController_LNH boss;
    private bool isBossDie;
    
    // Start is called before the first frame update
    void Start()
    {
        c_hp = 100;
        c_animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        pd = FindObjectOfType<Playercontroller2_donghee>();
        boss = FindObjectOfType<BossController_LNH>();

        creatureAgent = GetComponent<NavMeshAgent>();
        creatureAgent.destination = player.position;
        
        isDie = false;
        isHowl = false;
        isBossDie = false;

        //혈흔 효과 프리팹을 로드
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");

        

    }

    // Update is called once per frame
    void Update()
    {
        if (isDie) return;

        if (state != State.DIE)
        {
            center_player = player.localPosition;
            center_player.y = transform.localPosition.y;
            transform.LookAt(center_player);
        }

        if (pd.isDead)
        {
            StopAllCoroutines();
            creatureAgent.isStopped = true;
            c_animator.SetBool("hashMove", false);
            return;
        }

        

        creatureAgent.destination = player.position;
        distance = Vector3.Distance(player.position, this.transform.position);
        CreatureSpeed(distance); // 크리쳐 속도 조절용, 멀면 빨라지고 가까워지면 느려짐

        if (c_hp <= 0)
        {
            StopTrace();
        }
        
        if(boss.e_hp <= 0 && !isBossDie) // 보스 사망시 크리쳐도 사망
        {
            isBossDie = true;
            isDie = true;
            state = State.DIE;
            c_hp = 0;
            c_animator.SetTrigger("Dead");
            StopAllCoroutines();
            this.GetComponent<BoxCollider>().enabled = false;
            c_animator.SetBool("hashMove", false);
            

        }

    }
    private void OnEnable()
    {

        StartCoroutine(CreatureControll());
        StartCoroutine(Action());
    }

    private void StopTrace()
    {
        creatureAgent.isStopped = true;
        creatureAgent.velocity = Vector3.zero;
    }

    IEnumerator CreatureControll()
    {

        while (!isDie)
        {
            if (state == State.DIE) yield break;

            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }
            else
            {
                state = State.TRACE;
            }
            yield return new WaitForSecondsRealtime(0.3f);
        }
    }
    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return new WaitForSecondsRealtime(0.3f);
            int rnd = Random.Range(0, 3);
            switch (state)
            {

                case State.TRACE:
                    if (!isHowl)
                    {
                        c_animator.SetTrigger("Howl");
                        isHowl = true;
                    }
                    this.transform.LookAt(center_player);
                    creatureAgent.destination = player.position;
                    creatureAgent.isStopped = false;
                    c_animator.SetBool("hashMove", true);
                    break;
                case State.ATTACK:
                    StopTrace();
                    c_animator.SetBool("hashMove", false);
                    
                    switch (rnd)
                    {
                        case 0:
                            c_animator.SetTrigger("Attack1");
                            break;
                        case 1:
                            c_animator.SetTrigger("Attack2");
                            break;
                        case 2:
                            c_animator.SetTrigger("Attack3");
                            break;
                    }
                    
                    break;
            }
        }

    }

    private void CreatureSpeed(float distance)
    {
        creatureAgent.speed = distance + 8f;
    }

    public void EnemyGetHit()
    {
        c_animator.SetTrigger("GetHit");
        c_hp -= 30;

        if (c_hp <= 0)
        {
            isDie = true;
            state = State.DIE;
            c_hp = 0;
            c_animator.SetTrigger("Dead");
            StopAllCoroutines();
            this.GetComponent<BoxCollider>().enabled = false;
            c_animator.SetBool("hashMove", false);
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("BULLET"))
        {
            ShowBloodEffect(collision);
            Destroy(collision.gameObject);
            EnemyGetHit();
            
        }
    }

    void ShowBloodEffect(Collision coll)
    {
        //총알이 충돌한 지점 산출
        Vector3 pos = coll.contacts[0].point;
        //총알의 충돌했을 때의 법선 벡터
        Vector3 _normal = coll.contacts[0].normal;
        //총알의 충돌 시 방향 벡터의 회전값 계산
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        //혈흔 효과 생성
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
}
