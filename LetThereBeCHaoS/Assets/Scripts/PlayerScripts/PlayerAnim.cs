using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    PlayerManager manager;
    Animator anim;
    Rigidbody rb;
    [SerializeField] SpriteRenderer spriteRend;
    [SerializeField] Transform tailPos;

    [SerializeField] GameObject dashAfterEffect;
    float startDashTime;
    Queue<GameObject> dashPool;

    //facingdirection
    public bool isFacingRight { private set; get; }
    public bool isFacingFront { private set; get; }
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
    }
    private void Update()
    {
        if (rb.velocity.x > 0) isFacingRight = true;
        else if (rb.velocity.x < 0) isFacingRight = false;
        if (rb.velocity.z > 0) isFacingFront = false;
        else if (rb.velocity.z < 0) isFacingFront = true;

        if (isFacingFront && isFacingRight) anim.Play("player_idle_front_right");
        else if (isFacingFront && !isFacingRight) anim.Play("player_idle_front_left");
        else if (!isFacingFront && isFacingRight) anim.Play("player_idle_back_right");
        else if (!isFacingFront && !isFacingRight) anim.Play("player_idle_back_left");

        manager.tailAnim.UpdateTailDirection();
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
}
