using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    PlayerManager manager;
    ChaosMeterUI chaosMeter;
    HealthMeterUI healthMeter;

    //stats
    [SerializeField] bool isAlive;
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;

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
        chaosForm = false;
    }
    private void Start()
    {
        chaosMeter = UIManager.Instance.chaosMeter;
        chaosMeter.UpdateChaosMeter(currentChaosMeter, maxChaosMeter, false);
        healthMeter = UIManager.Instance.healthMeter;
        healthMeter.UpdateHealthMeter(currentHealth, maxHealth);
    }
    private void Update()
    {
        if (chaosForm)
        {
            if (currentChaosMeter > 0)
            {
                currentChaosMeter -= chaosMeterDepleteSpeed * Time.deltaTime;
                if (currentChaosMeter < 0) currentChaosMeter = 0;
                //updateUI
                chaosMeter.UpdateChaosMeter(currentChaosMeter, maxChaosMeter, chaosForm);
            } 
            else
            {
                //exitingChaosMode
                chaosForm = false;
                manager.anim.ChaosForm(chaosForm);
                manager.tailAnim.ChaosForm(chaosForm);
                //statChange
                currentBasicAttackCooldown = basicAttackCooldown;
                //updateUI
                chaosMeter.UpdateChaosMeter(currentChaosMeter, maxChaosMeter, chaosForm);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)) Hurt(1);
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
    public float GetMaxHealth() { return maxHealth; }
    public float GetCurrentHealth() { return currentHealth; }

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
                manager.tailAnim.ChaosForm(chaosForm);
                //statChange
                currentBasicAttackCooldown = basicAttackCooldownChaos;
            }
            //updateUI
            chaosMeter.UpdateChaosMeter(currentChaosMeter, maxChaosMeter,chaosForm);
        }
    }
    //Hurt
    public void Hurt(float damageReceived)
    {
        if (isAlive)
        {
            currentHealth -= damageReceived;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isAlive = false;
            }
            healthMeter.UpdateHealthMeter(currentHealth, maxHealth);
        }
        
    }
}
