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

    [SerializeField] float strength;
    [SerializeField] float basicAttackCooldown;
    [SerializeField] float basicAttackCooldownChaos;
    float currentBasicAttackCooldown;

    [SerializeField] bool chaosForm;
    [SerializeField] float maxChaosMeter;
    [SerializeField] float currentChaosMeter;
    [SerializeField] float chaosMeterReplenishSpeed;
    [SerializeField] float chaosMeterDepleteSpeed;
    private void Awake()
    {
        manager = GetComponent<PlayerManager>();
        currentBasicAttackCooldown = basicAttackCooldown;
    }
    private void Update()
    {
        if (chaosForm)
        {
            if (currentChaosMeter > 0) currentChaosMeter -= chaosMeterDepleteSpeed * Time.deltaTime;
            else
            {
                //exitingChaosMode
                chaosForm = false;
                currentChaosMeter = 0;
                manager.anim.ChaosForm(chaosForm);
                //statChange
                currentBasicAttackCooldown = basicAttackCooldown;
            }
        }
    }

    //stats getter
    public float GetMoveSpeed() { return moveSpeed; }
    public float GetJumpPower() { return jumpPower; }
    public int GetMaxJumpCount() { return maxJumpCount; }
    public float GetDashSpeed() { return dashSpeed; }
    public int GetMaxDashCount() { return maxDashCount; }
    public float GetStrength() { return strength; }
    public float GetBasicAttackCooldown() { return currentBasicAttackCooldown; }
    public bool GetChaosForm() { return chaosForm; }

    //Chaos
    public void GainChaos()
    {
        if (!chaosForm)
        {
            currentChaosMeter += chaosMeterReplenishSpeed;
            if (currentChaosMeter >= maxChaosMeter)
            {
                //enteringChaosMode
                currentChaosMeter = maxChaosMeter;
                chaosForm = true;
                manager.anim.ChaosForm(chaosForm);
                //statChange
                currentBasicAttackCooldown = basicAttackCooldownChaos;
            }
        }
    }
}
