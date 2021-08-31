using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnableOnEnter : MonoBehaviour
{
    [SerializeField] GameObject objectToEnable;
    [SerializeField] string triggererTag;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggererTag)) objectToEnable.SetActive(true);
    }
}
