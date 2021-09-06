using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour,IDamagable
{
    Rigidbody rb;
    [SerializeField] AudioSource sfxSource;
    //stats
    [SerializeField] bool isAlive;
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    [SerializeField] float moveSpeed;
    [SerializeField] float power;

    //homing
    bool isTargeting;
    float targetingCheckTimer = 0.5f;
    float startTargetingCheckTimer;
    Transform target;
    [SerializeField] float checkTargetRad;
    [SerializeField] LayerMask whatIsTarget;
    [SerializeField] float stopDistance;
    [SerializeField] float reverseDistance;
    bool isReversing;

    //roaming
    [SerializeField] float roamingTime;
    float startRoamingTime;
    
    //animations
    [SerializeField] Transform sprites;

    //attack
    [SerializeField] float timeBetweenAttack;
    float startTimeBetweenAttack;
    [SerializeField] float timeBetweenMinionSpawn;
    float startTimeBetweenMinionSpawn;
    [SerializeField] GameObject minion;
    [SerializeField] Transform minionSpawnPos;

    //hurt
    [SerializeField] GameObject hurtParticle;
    [SerializeField] AudioClip hurtSfx;

    private void Awake()
    {
        currentHealth = maxHealth;
        isAlive = true;
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;
    }
    void Update()
    {
        UpdateTimer();
        //check for target within range
        if (!isTargeting && startTargetingCheckTimer <= 0)
        {
            startTargetingCheckTimer = targetingCheckTimer;
            CheckForTarget();
        }
        if (isTargeting)
        {
            if((target.position-transform.position).sqrMagnitude> stopDistance * stopDistance) //home to target when distance greater than stop distance
            {
                isReversing = false;
                rb.velocity = (target.position - transform.position).normalized * moveSpeed;
            }
            else if ((target.position - transform.position).sqrMagnitude < reverseDistance * reverseDistance) //reverse from target when distance less than reverse distance
            {
                isReversing = true;
                rb.velocity = (target.position - transform.position).normalized * -moveSpeed;
            }
            else
            {
                isReversing = false;
                rb.velocity = Vector3.zero;
            }

            if (startTimeBetweenMinionSpawn <= 0)
            {
                startTimeBetweenMinionSpawn = timeBetweenMinionSpawn;
                Instantiate(minion, minionSpawnPos.position, Quaternion.identity);
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
        else if (collision.gameObject.CompareTag("Player") && startTimeBetweenAttack <= 0) // damage player when collided with them
        {
            startTimeBetweenAttack = timeBetweenAttack;
            collision.gameObject.GetComponent<PlayerStat>().Hurt(power);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && startTimeBetweenAttack <= 0) // damage player when collided with them
        {
            startTimeBetweenAttack = timeBetweenAttack;
            collision.gameObject.GetComponent<PlayerStat>().Hurt(power);
        }
    }
    //check for target within range
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
        Vector2 dir = (new Vector2(Random.Range(-1, 1), Random.Range(-1, 1))).normalized * moveSpeed / 2f; //chose random horizontal direction mutliplied by half of home speed
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
        if (startTimeBetweenMinionSpawn > 0) startTimeBetweenMinionSpawn -= Time.deltaTime;
    }
    void Flip()
    {
        if (rb.velocity.x > 0 && sprites.localScale.x < 0) sprites.localScale = new Vector3(-sprites.localScale.x, sprites.localScale.y, sprites.localScale.z);
        else if (rb.velocity.x < 0 && sprites.localScale.x > 0) sprites.localScale = new Vector3(-sprites.localScale.x, sprites.localScale.y, sprites.localScale.z);
    }
    void PlaySfx(AudioClip clip)
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.pitch = Random.Range(0.95f, 1.5f);
            sfxSource.clip = clip;
            sfxSource.Play();
        }
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkTargetRad);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, reverseDistance);
    }
    */
}
