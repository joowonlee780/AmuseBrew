using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxControll : MonoBehaviour // 몬스터마다 가지고 있을 히트박스를 제어하는 스크립트
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine("AutoDisable"); // 이 객체가 활성화 되있을 때 코루틴 실행
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER")) // 트리거에 걸린 콜리더의 태그가 플레이어이면
        {
            print("hit"); // 피격 함수 추가부분
            PlayerHitManage pm = FindObjectOfType<PlayerHitManage>();
            pm.Hit();
        }
    }

    private IEnumerator AutoDisable() // 0.1초 간격으로 해당 객체를 disable시키는 코루틴
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}
