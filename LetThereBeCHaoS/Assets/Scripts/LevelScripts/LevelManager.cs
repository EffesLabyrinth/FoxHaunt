using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] AudioClip bgm;
    void Start()
    {
        AudioManager.Instance.PlayMusic(bgm);
    }
}
