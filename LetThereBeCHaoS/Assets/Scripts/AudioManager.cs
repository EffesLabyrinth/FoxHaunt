using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    public static AudioManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource.clip != musicClip || !musicSource.isPlaying)
        {
            musicSource.clip = musicClip;
            musicSource.Play();
        }
    }
}
