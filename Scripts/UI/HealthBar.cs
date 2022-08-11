using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Health healthComponent;
    [SerializeField] RectTransform foreground;
    [SerializeField] Canvas rootCanvas = null;
    void Start()
    {
        healthComponent = GetComponentInParent<Health>();
    }

    void Update()
    {
        if (Mathf.Approximately(healthComponent.GetHealthFraction(), 0) || Mathf.Approximately(healthComponent.GetHealthFraction(), 1))
        {
            rootCanvas.enabled = false;
            return;
        }
        rootCanvas.enabled = true;
        foreground.localScale = new Vector3(Mathf.Max(healthComponent.GetHealthFraction(), 0), 1, 1);
    }
}
