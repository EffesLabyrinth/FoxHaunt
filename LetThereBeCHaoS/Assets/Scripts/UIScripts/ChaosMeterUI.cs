using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaosMeterUI : MonoBehaviour
{
    [SerializeField] Image container;
    [SerializeField] Image meter;
    [SerializeField] Sprite containerNormal;
    [SerializeField] Sprite containerBroken1;
    [SerializeField] Sprite containerBroken2;
    [SerializeField] Material normal;
    [SerializeField] Material chaos;
    private void Awake()
    {
        meter.fillAmount = 0;
    }

    // Update is called once per frame
    public void UpdateChaosMeter(float currentAmount,float maxAmount,bool isChaos)
    {
        float ratio = currentAmount / maxAmount;
        meter.fillAmount = ratio;
        if (!isChaos && ratio >= 0.8f) container.sprite = containerBroken1;
        else if (isChaos) container.sprite = containerBroken2;
        else container.sprite = containerNormal;

        if (isChaos)
        {
            if(meter.material == normal)
            {
                meter.material = chaos;
            }
        }
        else
        {
            if (meter.material == chaos)
            {
                meter.material = normal;
            }
        }
    }
}
