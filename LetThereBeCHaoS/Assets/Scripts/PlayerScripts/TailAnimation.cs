using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailAnimation : MonoBehaviour
{
    [SerializeField] PlayerManager manager;
    LineRenderer lineRenderer;

    float tailPosOriginal;

    [SerializeField] int tailLength;

    Vector3[] segmentPoses;
    Vector3[] segmentV;
    [SerializeField] float smoothSpeed;
    [SerializeField] Transform targetDir;
    [SerializeField] float targetDistance;

    [SerializeField] float wiggleSpeed;
    [SerializeField] float wiggleMagnitude;
    [SerializeField] Transform wiggleDir;

    [SerializeField] Material normal;
    [SerializeField] Material chaos;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        tailPosOriginal = transform.position.x;
    }
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.positionCount = tailLength;
        segmentPoses = new Vector3[tailLength];
        segmentV = new Vector3[tailLength];
    }

    // Update is called once per frame
    void Update()
    {
        wiggleDir.localRotation = Quaternion.Euler(0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude, 0);

        segmentPoses[0] = targetDir.position;
        for (int i = 1; i < tailLength; i++)
        {
            //Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * targetDistance;
            //segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], smoothSpeed);
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + targetDir.forward * targetDistance, ref segmentV[i], smoothSpeed);
        }
        lineRenderer.SetPositions(segmentPoses);
    }
    public void UpdateTailDirection()
    {
        if (!manager.anim.isFacingFront && base.transform.localEulerAngles.y == 0)
        {
            lineRenderer.sortingOrder = 1;
            base.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
        }
        else if (manager.anim.isFacingFront && base.transform.localEulerAngles.y == 180)
        {
            lineRenderer.sortingOrder = -1;
            base.transform.Rotate(new Vector3(0, -180, 0), Space.Self);
        }
        if (manager.anim.isFacingRight) transform.localPosition = new Vector3(tailPosOriginal, transform.localPosition.y, transform.localPosition.z);
        else if(!manager.anim.isFacingRight) transform.localPosition = new Vector3(-tailPosOriginal, transform.localPosition.y, transform.localPosition.z);
    }
    public void ChaosForm(bool isChaos)
    {
        if (isChaos) lineRenderer.material = chaos;
        else lineRenderer.material = normal;
    }
}
