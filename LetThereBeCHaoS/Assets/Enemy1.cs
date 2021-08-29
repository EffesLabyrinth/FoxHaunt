using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour,IDamagable
{
    Rigidbody rb;
    //stats
    [SerializeField] bool isAlive;
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;

    [SerializeField] float moveSpeed;

    [SerializeField] float power;

    //
    bool isTargeting;
    float targetingCheckTimer = 0.5f;
    float startTargetingCheckTimer;
    [SerializeField] float checkTargetRad;
    [SerializeField] LayerMask whatIsTarget;
    Transform target;
    //roaming
    [SerializeField] float roamingTime;
    float startRoamingTime;
    //animations
    [SerializeField] Transform sprites;
    //attack
    [SerializeField] float timeBetweenAttack;
    float startTimeBetweenAttack;
    //hurt
    [SerializeField]GameObject hurtParticle;
    private void Awake()
    {
        currentHealth = maxHealth;
        isAlive = true;
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
        if (!isTargeting && startTargetingCheckTimer<=0)
        {
            startTargetingCheckTimer = targetingCheckTimer;
            CheckForTarget();
        }
        if (isTargeting)
        {
            rb.velocity = (target.position - transform.position).normalized * moveSpeed;
        }
        else
        {
            if (startRoamingTime <= 0)
            {
                startRoamingTime = roamingTime;
                Roam();
            }
            
        }
        Flip();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isTargeting && collision.gameObject.CompareTag("Ground"))
        {
            Roam();
        }
        else if (collision.gameObject.CompareTag("Player") && startTimeBetweenAttack<=0)
        {
            startTimeBetweenAttack = timeBetweenAttack;
            collision.gameObject.GetComponent<PlayerStat>().Hurt(power);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && startTimeBetweenAttack <= 0)
        {
            startTimeBetweenAttack = timeBetweenAttack;
            collision.gameObject.GetComponent<PlayerStat>().Hurt(power);
        }
    }
    void CheckForTarget()
    {
        Collider[] temp = Physics.OverlapSphere(transform.position, checkTargetRad, whatIsTarget);
        if (temp.Length > 0)
        {
            target = temp[0].transform;
            isTargeting = true;
        }
    }
    void Roam()
    {
        Vector2 dir = (new Vector2(Random.Range(-1, 1), Random.Range(-1, 1))).normalized * moveSpeed / 2f;
        rb.velocity = new Vector3(dir.x, rb.velocity.y, dir.y);
    }
    public void TakeDamage(float damage)
    {
        if (isAlive)
        {
            currentHealth -= damage;
            Instantiate(hurtParticle, transform.position, Quaternion.identity);
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isAlive = false;
                gameObject.SetActive(false);
            }
        }
    }
    void UpdateTimer()
    {
        if (startTargetingCheckTimer > 0) startTargetingCheckTimer -= Time.deltaTime;
        if (startRoamingTime > 0) startRoamingTime -= Time.deltaTime;
        if (startTimeBetweenAttack > 0) startTimeBetweenAttack -= Time.deltaTime;
    }
    void Flip()
    {
        if (rb.velocity.x > 0 && sprites.localScale.x < 0) sprites.localScale = new Vector3(-sprites.localScale.x, sprites.localScale.y, sprites.localScale.z);
        else if (rb.velocity.x < 0 && sprites.localScale.x > 0) sprites.localScale = new Vector3(-sprites.localScale.x, sprites.localScale.y, sprites.localScale.z);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkTargetRad);
    }
}
