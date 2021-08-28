using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawProjectile : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float projectileVelocity;
    [SerializeField] float projectileLifeTime;
    float startProjectileLifetime;
    [SerializeField] int noOfTarget;
    int startNoOfTarget;
    void Start()
    {
        rb.velocity = transform.forward * projectileVelocity;
        startProjectileLifetime = projectileLifeTime;
        startNoOfTarget = noOfTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (startProjectileLifetime > 0) startProjectileLifetime -= Time.deltaTime;
        else Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            startNoOfTarget--;
            if (startNoOfTarget <= 0) Destroy(gameObject);
        }
    }
}
