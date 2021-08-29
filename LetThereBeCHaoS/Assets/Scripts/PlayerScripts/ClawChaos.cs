using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawChaos : MonoBehaviour
{
    [SerializeField] GameObject clawProjectile;
    [SerializeField] Transform[] clawProjectileInitialPos;
    [SerializeField] float delay;
    float startDelay;
    bool isShot;
    void Start()
    {
        startDelay = delay;
        isShot = false;
    }
    private void OnEnable()
    {
        startDelay = delay;
        isShot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startDelay > 0) startDelay -= Time.deltaTime;
        else
        {
            if (!isShot)
            {
                isShot = true;
                for (int i = 0; i < clawProjectileInitialPos.Length; i++)
                {
                    Instantiate(clawProjectile, clawProjectileInitialPos[i].position, clawProjectileInitialPos[i].rotation);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IDamagable>().TakeDamage(PlayerManager.Instance.stat.GetStrength());
        }
    }
}
