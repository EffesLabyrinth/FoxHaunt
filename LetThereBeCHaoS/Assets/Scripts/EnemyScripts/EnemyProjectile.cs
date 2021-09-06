using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    float damage;
    bool startProjectile;
    float startProjectileDuration;
    [SerializeField] Transform sprites;

    public void InitializeProjectile(Vector3 direction, float speed, float duration,float damage)
    {
        rb.velocity = direction.normalized * speed;
        startProjectileDuration = duration;
        startProjectile = true;
        if (direction.x < 0) sprites.localScale = new Vector3(-sprites.localScale.x, sprites.localScale.y, sprites.localScale.z);
        this.damage = damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStat>().Hurt(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (startProjectile && startProjectileDuration>0)
        {
            startProjectileDuration -= Time.deltaTime;
        }
        else if (startProjectile)
        {
            Destroy(gameObject);
        }
    }
}
