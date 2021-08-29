using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawNormal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IDamagable>().TakeDamage(PlayerManager.Instance.stat.GetStrength());
        }
    }
}
