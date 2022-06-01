using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Targetting : MonoBehaviour
{
    public float viewRadius; // 시야 범위
    [Range(0, 360)]
    public float viewAngle; // 시야각

    public LayerMask targetMask; // 타겟 설정을 위한 layer마스크
    public LayerMask obstacleMask; // 장애물 설정을 위한 layer마스크
    //있어야함, 왜냐면 타겟 사이에 다른 오브젝트가 있는데 그 오브젝트를 투과해서 뒤의 타겟오브젝트를 볼 수 있음 
    public bool m_TargetOn; // 타겟팅 설정 확인 변수

    private Vector3 pos;

    void Start()
    {
        m_TargetOn = false; // 시작할 땐 타겟팅 설정이 안되어있으므로 false
        //플레이 시 TargettingDelay 코루틴을 실행. 0.5초 간격 
        StartCoroutine("TargettingDelay", 0.5f);
    }

    void Update()
    {
        pos = transform.position;
    }

    IEnumerator TargettingDelay(float delay)
    {
        while (true) // 루틴 실행 받은 delay값 만큼 간격을 띄우고 실행
        {
            yield return new WaitForSeconds(delay);
            FindTarget();
        }
    }

    void FindTarget()
    {
        Collider[] targetInViewRadius = Physics.OverlapSphere(pos, viewRadius, targetMask);
        // 현재 좀비의 위치로부터 시야안에 target이 있는지 확인

        for (int i = 0; i < targetInViewRadius.Length; i++)
        //ViewRadius 안에 있는 타겟의 개수 = 배열의 개수 보다 i가 작을 때 for 실행 
        {
            Transform target = targetInViewRadius[i].transform; //타겟의 위치 
            Vector3 dirToTarget = (target.position - (pos + transform.up)).normalized;
            //vector3타입의 타겟의 방향 변수 선언 = 타겟의 방향벡터, (타겟의 position - 이 게임오브젝트의 position) normalized 
            //  = 벡터 크기 정규화 = 단위벡터화
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            // 전방 벡터와 타겟방향벡터의 크기가 시야각의 1/2이면 = 시야각 안에 타겟 존재 
            {
                float dstToTarget = Vector3.Distance(pos, target.position); //타겟과의 거리를 계산 
                if (!Physics.Raycast(pos + transform.up, dirToTarget, dstToTarget, obstacleMask))
                // 레이캐스트의 시작 위치, 타겟의 방향벡터, 타겟과의 거리, 장애물 마스크 검사
                //레이캐스트를 쐈는데 obstacleMask가 아닐 때 참이고 아래를 실행함 
                {
                    m_TargetOn = true;
                    print("raycast hit!"); // 타겟이 잡힌걸 확인
                    Debug.DrawRay(pos + transform.up, dirToTarget * 10f, Color.red, 5f);
                    // 타겟의 위치를 따라가는걸 확인하기 위해 레이저를 그림
                }
                else// 레이캐스트를 쐈는데 obstacle이면 TargetOff, 
                {
                    m_TargetOn = false;
                }
            }
            else // 시야안에 타겟이 존재하지 않으면 TargetOff
            {
                m_TargetOn = false;
            }

        }


    }
}
