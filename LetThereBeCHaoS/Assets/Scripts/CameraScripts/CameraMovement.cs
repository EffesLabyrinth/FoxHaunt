using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    CameraManager manager;
    Transform camerAnchor;
    Transform followTarget;
    [SerializeField] float smoothTime;
    Vector3 velocity;
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
        Follow();
    }
    void Follow()
    {
        camerAnchor.position = Vector3.SmoothDamp(camerAnchor.position, followTarget.position,ref velocity, smoothTime * Time.deltaTime);
    }
}
