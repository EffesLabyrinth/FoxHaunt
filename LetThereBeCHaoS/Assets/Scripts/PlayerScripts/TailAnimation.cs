using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailAnimation : MonoBehaviour
{
    LineRenderer lineRenderer;
    Rigidbody rb;

    [SerializeField] int tailLength;

    Vector3[] segmentPoses;
    Vector3[] segmentV;
    [SerializeField] float smoothSpeed;
    [SerializeField] Transform targetDir;
    [SerializeField] float targetDistance;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.positionCount = tailLength;
        segmentPoses = new Vector3[tailLength];
        segmentV = new Vector3[tailLength];
        rb = PlayerManager.Instance.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.z > 0 && transform.localEulerAngles.y == 0)
        {
            lineRenderer.sortingOrder = 1;
            transform.Rotate(new Vector3(0, 180, 0), Space.Self);
        } 
        else if(rb.velocity.z < 0 && transform.localEulerAngles.y == 180)
        {
            lineRenderer.sortingOrder = -1;
            transform.Rotate(new Vector3(0, -180, 0), Space.Self);
        }

        segmentPoses[0] = targetDir.position;
        for (int i = 1; i < tailLength; i++)
        {
            Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * targetDistance;
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], smoothSpeed);
        }
        lineRenderer.SetPositions(segmentPoses);
    }
}
