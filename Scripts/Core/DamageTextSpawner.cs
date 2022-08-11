using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] DamageText damageText;
    public void Spawn(float damage)
    {
        if (damage > 0)
        {
            DamageText instance = Instantiate<DamageText>(damageText, transform);
            instance.SetDamageText((int)(damage));
        }
    }
}
