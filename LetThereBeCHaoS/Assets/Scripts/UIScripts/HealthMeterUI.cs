using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMeterUI : MonoBehaviour
{
    [SerializeField] Image container;
    [SerializeField] Image healthMeter;
    [SerializeField] Image followMeter;
    [SerializeField] float followSpeeed;
    float startFollowAmount;
    [SerializeField] Color normal;
    [SerializeField] Color medium;
    [SerializeField] Color low;
    // Start is called before the first frame update
    void Start()
    {
        healthMeter.fillAmount = 0;
        followMeter.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (followMeter.fillAmount > healthMeter.fillAmount)
        {
            startFollowAmount = followMeter.fillAmount - Time.deltaTime * followSpeeed;
            if (startFollowAmount < healthMeter.fillAmount) startFollowAmount = healthMeter.fillAmount;
            followMeter.fillAmount = startFollowAmount;
        }
        else if (followMeter.fillAmount < healthMeter.fillAmount) followMeter.fillAmount = healthMeter.fillAmount;
    }
    public void UpdateHealthMeter(float current, float max)
    {
        float ratio = current / max;
        healthMeter.fillAmount = ratio;
        if (ratio <= 0.2) healthMeter.color = low;
        else if (ratio <= 0.5) healthMeter.color = medium;
        else healthMeter.color = normal;
    }
}
