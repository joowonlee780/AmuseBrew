using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBoxOn : MonoBehaviour // 애니메이션 이벤트 함수를 위한 스크립트, 간단하게 히트박스를 active 시키는 용도
{
    // Start is called before the first frame update

    public GameObject attackCollision;

    public void OnAttackCollision()
    {
        attackCollision.SetActive(true);
    }
}
