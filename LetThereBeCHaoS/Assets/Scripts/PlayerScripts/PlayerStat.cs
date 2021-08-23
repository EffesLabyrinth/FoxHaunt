using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    PlayerManager manager;

    //stats
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] int maxJumpCount;
    private void Awake()
    {
        manager = GetComponent<PlayerManager>();
    }

    //stats getter
    public float GetMoveSpeed() { return moveSpeed; }
    public float GetJumpPower() { return jumpPower; }
    public int GetMaxJumpCount() { return maxJumpCount; }
}
