using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSoundController : MonoBehaviour
{

    private AudioSource[] bossAudioSource;
    private AudioSource bossAttackSound;
    private AudioSource bossHitSound;
    private AudioSource bossShoutSound;
    private AudioSource bossDeadSound;

    // Start is called before the first frame update
    void Start()
    {
        this.bossAudioSource = GetComponents<AudioSource>();
        bossAttackSound = bossAudioSource[0];
        bossHitSound = bossAudioSource[1];
        bossShoutSound = bossAudioSource[2];
        bossDeadSound = bossAudioSource[3];
    }

    void PlayBossAttackAudio()
    {
        bossAttackSound.Play(); // 공격
    }

    void PlayBossHitAudio()
    {
        bossHitSound.Play(); // 피격
    }

    void PlayBossShoutAudio()
    {
        bossShoutSound.Play(); // 소리 지르기
    }

    void PlayBossDeadAudio()
    {
        bossDeadSound.Play(); // 죽음
    }
}
