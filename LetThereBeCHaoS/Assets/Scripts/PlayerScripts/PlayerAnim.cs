using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    PlayerManager manager;
    Animator anim;
    Rigidbody rb;
    [SerializeField] SpriteRenderer spriteRend;

    //dash
    [SerializeField] GameObject dashAfterEffect;
    float startDashTime;
    Queue<GameObject> dashPool;

    //facingdirection
    public bool isFacingRight { set; get; }
    public bool isFacingFront { set; get; }

    //attack
    float startAttackingDuration;
    GameObject GetDashFromPool()
    {
        if (dashPool.Count > 0)
        {
            return dashPool.Dequeue();
        }
        else
        {
            return Instantiate(dashAfterEffect);
        }
    }
    private void Awake()
    {
        manager = GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        dashPool = new Queue<GameObject>();
        isFacingFront = true;
        isFacingRight = true;
    }
    private void Update()
    {
        TimerUpdate();

        if (startDashTime > 0)
        {
            CheckFacingDirection();
            RunAnimation();
        }
        else if (startAttackingDuration <= 0)
        {
            CheckFacingDirection();

            if (rb.velocity.y > 1f) JumpAnimation();
            else if (rb.velocity.y < -1f) FallAnimation();
            else if (manager.controller.GroundCheck()){
                if (CheckHorizontalInput()) RunAnimation();
                else IdleAnimation();
            }
            
        }

        manager.tailAnim.UpdateTailDirection();
    }
    bool CheckHorizontalInput()
    {
        return manager.controller.GetInputDirection().sqrMagnitude!=0;
    }
    void CheckFacingDirection()
    {
        if (rb.velocity.x > 0) isFacingRight = true;
        else if (rb.velocity.x < 0) isFacingRight = false;
        if (rb.velocity.z > 0) isFacingFront = false;
        else if (rb.velocity.z < 0) isFacingFront = true;
    }
    public void DashAfterEffect(float dashDuration)
    {
        startDashTime = dashDuration;
        StopCoroutine(StartDashEffect());
        StartCoroutine(StartDashEffect());
    }
    IEnumerator StartDashEffect()
    {
        float timeBtwEffect = 0.03f;
        float startTimeBtwEffect = 0f;
        do
        {
            startDashTime -= Time.deltaTime;
            startTimeBtwEffect -= Time.deltaTime;
            if (startTimeBtwEffect < 0)
            {
                startTimeBtwEffect = timeBtwEffect;
                GameObject temp = GetDashFromPool();
                temp.transform.position = transform.position;
                temp.GetComponent<ReturnToPool>().SetPool(dashPool);
                temp.GetComponent<ReturnToPool>().SetSprite(spriteRend.sprite);
                temp.SetActive(true);
            }
            if (startDashTime > 0) yield return null;
        } while (startDashTime > 0);
    }
    public void AttackAnimation(float attackingDuration)
    {
        startAttackingDuration = attackingDuration;

        if (isFacingFront && isFacingRight) anim.Play("player_attack_front_right",0,0);
        else if (isFacingFront && !isFacingRight) anim.Play("player_attack_front_left", 0, 0);
        else if (!isFacingFront && isFacingRight) anim.Play("player_attack_back_right", 0, 0);
        else if (!isFacingFront && !isFacingRight) anim.Play("player_attack_back_left", 0, 0);
    }
    void IdleAnimation()
    {
        if (isFacingFront && isFacingRight) anim.Play("player_idle_front_right");
        else if (isFacingFront && !isFacingRight) anim.Play("player_idle_front_left");
        else if (!isFacingFront && isFacingRight) anim.Play("player_idle_back_right");
        else if (!isFacingFront && !isFacingRight) anim.Play("player_idle_back_left");
    }
    void RunAnimation()
    {
        if (isFacingFront && isFacingRight) anim.Play("player_run_front_right");
        else if (isFacingFront && !isFacingRight) anim.Play("player_run_front_left");
        else if (!isFacingFront && isFacingRight) anim.Play("player_run_back_right");
        else if (!isFacingFront && !isFacingRight) anim.Play("player_run_back_left");
    }
    void JumpAnimation()
    {
        if (isFacingFront && isFacingRight) anim.Play("player_jump_front_right");
        else if (isFacingFront && !isFacingRight) anim.Play("player_jump_front_left");
        else if (!isFacingFront && isFacingRight) anim.Play("player_jump_back_right");
        else if (!isFacingFront && !isFacingRight) anim.Play("player_jump_back_left");
    }
    void FallAnimation()
    {
        if (isFacingFront && isFacingRight) anim.Play("player_fall_front_right");
        else if (isFacingFront && !isFacingRight) anim.Play("player_fall_front_left");
        else if (!isFacingFront && isFacingRight) anim.Play("player_fall_back_right");
        else if (!isFacingFront && !isFacingRight) anim.Play("player_fall_back_left");
    }
    void TimerUpdate()
    {
        if (startAttackingDuration > 0) startAttackingDuration -= Time.deltaTime;

    }
}
