using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public class CrawlerBlood : MonoBehaviour
{
    // const string bulletTag = "BULLET";
    //생명 게이지
    //피격 시 사용할 혈흔 효과
    private GameObject bloodEffect;
    public float hp;
    void Start()
    {
        //혈흔 효과 프리팹을 로드
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");
        
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BULLET"))
        {
            //혈흔 효과를 생성하는 함수 호출
            ShowBloodEffect(coll);
            //총알 삭제
            Destroy(coll.gameObject);
            //생명 게이지 차감
            //hp -= coll.gameObject.GetComponent<BulletCtrl>().damage;
            hp = this.GetComponent<CrawlerManager>().crawlerHP;
            if (hp <= 0.0f)
            {
                //적 캐릭터의 상태를 DIE로 변경
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
            }
            else
            {
                //GetComponent<EnemySoundControl>().activeState = EnemySoundControl.EnemyState.DAMAGE;
            }
        }
    }

    


    //혈흔 효과를 생성하는 함수
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
