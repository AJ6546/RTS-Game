using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] Stamina staminaComponent;
    [SerializeField] RectTransform foreground;
    [SerializeField] Canvas rootCanvas = null;
    void Start()
    {
        staminaComponent = GetComponentInParent<Stamina>();
    }
    void Update()
    {
        if (Mathf.Approximately(staminaComponent.GetStaminaFraction(), 0) || Mathf.Approximately(staminaComponent.GetStaminaFraction(), 1))
        {
            rootCanvas.enabled = false;
            return;
        }
        rootCanvas.enabled = true;
        foreground.localScale = new Vector3(Mathf.Max(staminaComponent.GetStaminaFraction(), 0), 1, 1);
    }
}
