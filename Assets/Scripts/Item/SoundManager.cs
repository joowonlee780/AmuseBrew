using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource soundSource;
    private void Start()
    {
        soundSource = this.GetComponent<AudioSource>();
    }

    public void PickUpSound()
    {
        soundSource.Play();
    }
    
}
