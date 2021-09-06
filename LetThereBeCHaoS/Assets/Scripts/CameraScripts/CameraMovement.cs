using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    CameraManager manager;
    Transform cameraAnchor;
    Transform followTarget;
    [SerializeField] float smoothTime;
    Vector3 velocity;
    Vector3 followPosition;

    //screenShake
    public bool enableScreenShake;
    Vector3 screenShakeOffset;
    float startShakeDuration;
    float startShakeMagnitude;
    private void Awake()
    {
        manager = GetComponent<CameraManager>();
        cameraAnchor = transform.parent.transform;
        screenShakeOffset = Vector3.zero;
    }
    void Start()
    {
        followTarget = PlayerManager.Instance.transform;
    }

    void LateUpdate()
    {
        Follow();
        cameraAnchor.position = followPosition + screenShakeOffset;
    }
    void Follow()
    {
        followPosition = Vector3.SmoothDamp(cameraAnchor.position, followTarget.position,ref velocity, smoothTime * Time.deltaTime);
    }
    public void ScreenShake(float duration, float magnitude)
    {
        if (enableScreenShake)
        {
            startShakeDuration = duration;
            startShakeMagnitude = magnitude;
            StopCoroutine(StartScreenShake());
            StartCoroutine(StartScreenShake());
        }
    }
    IEnumerator StartScreenShake()
    {
        while (startShakeDuration > 0)
        {
            startShakeDuration -= Time.deltaTime;
            screenShakeOffset.x = Random.Range(-startShakeMagnitude, startShakeMagnitude);
            screenShakeOffset.y = Random.Range(-startShakeMagnitude, startShakeMagnitude);
            screenShakeOffset.z = Random.Range(-startShakeMagnitude, startShakeMagnitude);
            yield return null;
        }
        screenShakeOffset = Vector3.zero;
    }
}
