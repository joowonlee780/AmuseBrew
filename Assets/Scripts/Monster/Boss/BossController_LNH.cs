using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController_LNH : MonoBehaviour
{

    public ActionController security;
    // Start is called before the first frame update
    private Animator e_animator;
    
    
    public int e_hp;
    public Animator anim = null;
    
    private bool CanSummon;
    private Transform player;
    private Vector3 center_player;
    private bool isAttackCoroutineOn;

    Playercontroller2_donghee pd;
    private GameObject bloodEffect; // 혈흔 이펙트

    private CapsuleCollider boss_cp;

    
    // Start is called before the first frame update
    void Start()
    {
        isAttackCoroutineOn = false;
        e_hp = 100;
        e_animator = GetComponent<Animator>();
        CanSummon = true;
        player = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        pd = FindObjectOfType<Playercontroller2_donghee>();
        
        
        //혈흔 효과 프리팹을 로드
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");


        boss_cp = this.GetComponent<CapsuleCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        if (pd.isDead)
        {
            StopAllCoroutines();
            return;
        }

        if (this.e_hp <= 0) return;

        center_player = player.localPosition;
        center_player.y = transform.localPosition.y;
        transform.LookAt(center_player);

        
        
        
        float distance = Vector3.Distance(player.position, this.transform.position); // 7가까이 오면 길게 휘둘러치기에 맞고 3정도면 짧은 휘둘러치기에 맞을거 같음
        EnemyControll(distance);
    }

    
    void EnemyControll(float distance)
    {

        if (!isAttackCoroutineOn)
            StartCoroutine(AttackSimulation(distance));
        if (e_hp <= 50 && CanSummon)
        {
            CanSummon = !CanSummon;
            boss_cp.enabled = false;
            e_animator.SetTrigger("Howling");
            Invoke("SwitchSummon", 30f);
            boss_cp.enabled = true;
        }
    }

    public void EnemyGetHit()
    {
        e_animator.SetTrigger("GetHit");
        e_hp -= 3;

        if (e_hp <= 0)
        {
            e_hp = 0;
            e_animator.ResetTrigger("Howling");
            e_animator.SetTrigger("Dead");
            StopAllCoroutines();
            CancelInvoke();
            this.GetComponent<CapsuleCollider>().enabled = false;
            anim.SetTrigger("IsOpen");
            Destroy(this,1.0f);
            security.SecurityLevel = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("CHANDELIER"))
        {
            e_animator.ResetTrigger("Howling");
            e_animator.SetTrigger("Dead");

            e_hp = 0;
            StopAllCoroutines();
            CancelInvoke();
            this.GetComponent<CapsuleCollider>().enabled = false;
            Destroy(collision.gameObject, 3f);
            anim.SetTrigger("IsOpen");
            security.SecurityLevel = true;
            Destroy(this,1.0f);
            

        }

        if (collision.collider.CompareTag("BULLET"))
        {
            ShowBloodEffect(collision);
            Destroy(collision.gameObject);
            EnemyGetHit();

        }
    }

    private void SwitchSummon()
    {
        CanSummon = !CanSummon;
    }

    IEnumerator AttackSimulation(float distance)
    {
        isAttackCoroutineOn = true;

        // 시즈 모드 후 돌던지기만 하기
        int rnd = Random.Range(0, 3); // 돌을 던지는 3가지 방법 중 하나를 선택하기 위한 난수

        if (distance > 0f)
        {
            switch (rnd)
            {
                case 0:
                    e_animator.SetTrigger("ThrowStone_L");
                    break;
                case 1:
                    e_animator.SetTrigger("ThrowStone_R");
                    break;
                case 2:
                    e_animator.SetTrigger("ThrowStone_LR");
                    break;
            }
        }
        yield return new WaitForSecondsRealtime(1f);
        isAttackCoroutineOn = false;
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
