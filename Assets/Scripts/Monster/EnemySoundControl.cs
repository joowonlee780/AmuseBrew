using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySoundControl : MonoBehaviour
{
    // 피격, 사망, 공격 효과음 배열 선언
    public AudioClip[] damage, die, attack;

    // 오디오소스
    public AudioSource audioSource;
    
    // 효과음 랜덤 실행 시 필요한 변수, 실행 직전에 Random.Range를 이용함.
    private int soundNum;

    public enum EnemyState
    {
        ATTACK,
        DIE,
        DAMAGE,
        IDLE
    }
    public EnemyState activeState = EnemyState.IDLE;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {        
        switch (activeState)
            {
                case EnemyState.ATTACK:
                    //Debug.Log("공격"); // 공격 state 인지 확인하는 용도
                   enemyAttack();
                   break;
                case EnemyState.DIE:
                    //Debug.Log("사망");  // 사망 state 인지 확인하는 용도
                    enemyDie();
                    break;
                case EnemyState.DAMAGE:
                    //Debug.Log("피격"); // 피격 state 인지 확인하는 용도
                    enemyDamage();
                    break;
                default:
                    break;
            }
    }
    void enemyDamage()
    {
        // 피격 효과음 랜덤으로 재생하기 위해 랜덤 변수 사용
        soundNum = Random.Range(0, damage.Length);
        // 랜덤 피격 효과음 할당
        audioSource.clip = damage[soundNum];
        // 재생
        audioSource.Play();
    }

    void enemyDie()
    {
        // 사망 효과음 랜덤으로 재생하기 위해 랜덤 변수 사용
        soundNum = Random.Range(0, damage.Length);
        // 랜덤 사망 효과음 할당
        audioSource.clip = damage[soundNum];
        // 재생
        audioSource.Play();
    }
    
    void enemyAttack()
    {
        // 공격 효과음 랜덤으로 재생하기 위해 랜덤 변수 사용
        soundNum = Random.Range(0, damage.Length);
        // 랜덤 공격 효과음 할당
        audioSource.clip = damage[soundNum];
        // 재생
        audioSource.Play();
    }
}
