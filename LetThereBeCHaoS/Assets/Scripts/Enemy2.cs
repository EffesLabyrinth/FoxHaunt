using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour,IDamagable
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
    //homing
    [SerializeField] float stopDistance;
    [SerializeField] float reverseDistance;
    bool isReversing;
    //animations
    [SerializeField] Transform sprites;
    //attack
    [SerializeField] float timeBetweenAttack;
    float startTimeBetweenAttack;
    [SerializeField] float timeBetweenProjectile;
    float startTimeBetweenProjectile;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectilePos;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileDuration;
    //hurt
    [SerializeField] GameObject hurtParticle;
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
        if (!isTargeting && startTargetingCheckTimer <= 0)
        {
            startTargetingCheckTimer = targetingCheckTimer;
            CheckForTarget();
        }
        if (isTargeting)
        {
            if((target.position-transform.position).sqrMagnitude> stopDistance * stopDistance)
            {
                isReversing = false;
                rb.velocity = (target.position - transform.position).normalized * moveSpeed;
            }
            else if ((target.position - transform.position).sqrMagnitude < reverseDistance * reverseDistance)
            {
                isReversing = true;
                rb.velocity = (target.position - transform.position).normalized * -moveSpeed;
            }
            else
            {
                isReversing = false;
                rb.velocity = Vector3.zero;
            }
            if (startTimeBetweenProjectile <= 0)
            {
                startTimeBetweenProjectile = timeBetweenProjectile;
                Instantiate(projectile, projectilePos.position, Quaternion.identity);
            }
        }
        else
        {
            if (startRoamingTime <= 0)
            {
                startRoamingTime = roamingTime;
                Roam();
            }

        }
        if(!isReversing) Flip();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isTargeting && collision.gameObject.CompareTag("Ground"))
        {
            Roam();
        }
        else if (collision.gameObject.CompareTag("Player") && startTimeBetweenAttack <= 0)
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
        if (startTimeBetweenProjectile > 0) startTimeBetweenProjectile -= Time.deltaTime;
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
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, reverseDistance);
    }
}
