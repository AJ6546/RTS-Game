using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [SerializeField] Text damageText;
    void Start()
    {
        Destroy(gameObject, 1f);
    }
    public void SetDamageText(float damage)
    {
        damageText.text = Convert.ToString(damage);
    }
}
