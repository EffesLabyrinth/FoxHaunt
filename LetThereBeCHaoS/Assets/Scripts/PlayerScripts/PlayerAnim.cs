using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    PlayerManager manager;
    Animator anim;
    Rigidbody rb;
    [SerializeField] SpriteRenderer spriteRend;
    [SerializeField] Transform sprites;
    [SerializeField] Transform tailPos;

    [SerializeField] GameObject dashAfterEffect;
    float startDashTime;
    Queue<GameObject> dashPool;
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
        if (rb.velocity.z > 0) anim.Play("player_idle_back_right");
        else if (rb.velocity.z < 0) anim.Play("player_idle_front_right");

        if (rb.velocity.x > 0 && sprites.localScale.x < 0) Flip();
        else if (rb.velocity.x < 0 && sprites.localScale.x > 0) Flip();
    }
    void Flip()
    {
        sprites.localScale = new Vector3(sprites.localScale.x * -1, sprites.localScale.y, sprites.localScale.z);
        tailPos.localPosition = new Vector3(tailPos.localPosition.x * -1, tailPos.localPosition.y, tailPos.localPosition.z);
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
