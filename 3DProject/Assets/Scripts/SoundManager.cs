using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource source;

    private void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
    }

    //plays sound once 
    public void PlaySound(AudioClip _sound, float volume = 1f)
    {
        //source.volume = 0.5f;
        source.PlayOneShot(_sound, volume);
    }
}
