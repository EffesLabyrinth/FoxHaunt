using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    [HideInInspector] public PlayerController controller;
    [HideInInspector] public PlayerStat stat;
    [HideInInspector] public PlayerAnim anim;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        controller = GetComponent<PlayerController>();
        stat = GetComponent<PlayerStat>();
        anim = GetComponent<PlayerAnim>();
    }
}
