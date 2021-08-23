using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    [HideInInspector] public CameraMovement movement; 
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject.transform.parent.gameObject);

        movement = GetComponent<CameraMovement>();
    }
}
