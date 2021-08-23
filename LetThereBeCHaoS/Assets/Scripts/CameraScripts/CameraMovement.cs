using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    CameraManager manager;
    Transform camerAnchor;
    Transform followTarget;
    private void Awake()
    {
        manager = GetComponent<CameraManager>();
        camerAnchor = transform.parent.transform;
    }
    void Start()
    {
        followTarget = PlayerManager.Instance.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        camerAnchor.position = followTarget.position;
    }
}
