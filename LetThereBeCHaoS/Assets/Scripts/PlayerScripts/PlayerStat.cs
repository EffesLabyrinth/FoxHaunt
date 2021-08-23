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

    [SerializeField] float dashSpeed;
    [SerializeField] int maxDashCount;
    private void Awake()
    {
        manager = GetComponent<PlayerManager>();
    }

    //stats getter
    public float GetMoveSpeed() { return moveSpeed; }
    public float GetJumpPower() { return jumpPower; }
    public int GetMaxJumpCount() { return maxJumpCount; }
    public float GetDashSpeed() { return dashSpeed; }
    public int GetMaxDashCount() { return maxDashCount; }
}
