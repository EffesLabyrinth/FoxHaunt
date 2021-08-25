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
    Vector3 followPosition;

    //screenShake
    Vector3 screenShakeOffset;
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeMagnitude;
    float startShakeDuration;
    float startShakeMagnitude;
    private void Awake()
    {
        manager = GetComponent<CameraManager>();
        camerAnchor = transform.parent.transform;
        screenShakeOffset = Vector3.zero;
    }
    void Start()
    {
        followTarget = PlayerManager.Instance.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Follow();
        camerAnchor.position = followPosition + screenShakeOffset;
    }
    void Follow()
    {
        followPosition = Vector3.SmoothDamp(camerAnchor.position, followTarget.position,ref velocity, smoothTime * Time.deltaTime);
    }
    public void ScreenShake(float duration, float magnitude)
    {
        startShakeDuration = duration;
        startShakeMagnitude = magnitude;
        StopCoroutine(StartScreenShake());
        StartCoroutine(StartScreenShake());
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
