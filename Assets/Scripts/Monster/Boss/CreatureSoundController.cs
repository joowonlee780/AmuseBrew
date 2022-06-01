using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSoundController : MonoBehaviour
{

    private Animator c_animator;

    private AudioSource[] creatureAudioSource;
    private AudioSource creatureattackAudio;
    private AudioSource creaturehitAudio;
    private AudioSource creaturemoveAudio;
    private AudioSource creaturehowlAudio;
    private AudioSource creaturedeadAudio;

    // Start is called before the first frame update
    void Start()
    {
        c_animator = GetComponent<Animator>();
        creatureAudioSource = GetComponents<AudioSource>();

        creatureattackAudio = creatureAudioSource[0];
        creaturehitAudio = creatureAudioSource[1];
        creaturemoveAudio = creatureAudioSource[2];
        creaturehowlAudio = creatureAudioSource[3];
        creaturedeadAudio = creatureAudioSource[4];
    }

    // Update is called once per frame
    void Update()
    {
        if (c_animator.GetBool("hashMove"))
        {
            if (!creaturemoveAudio.isPlaying)
                creaturemoveAudio.Play();
        }
        else
        {
            creaturemoveAudio.Stop();
        }
    }

    void PlayCreatureAttackAudio()
    {
        creatureattackAudio.Play();
    }

    void PlayCreatureHitAudio()
    {
        creaturehitAudio.Play();
    }

    void PlayCreatureHowlAudio()
    {
        creaturehowlAudio.Play();
    }

    void PlayCreatureDeadAudio()
    {
        creaturedeadAudio.Play();
    }
}
