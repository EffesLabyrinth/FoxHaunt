using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterASec : MonoBehaviour
{
    [SerializeField] float destroyTimer;
    void Start()
    {
        Destroy(gameObject, destroyTimer);
    }
}
