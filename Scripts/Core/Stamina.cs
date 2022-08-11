using System.Collections;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    /* All units starts with stamina of 100. Stamina is kept by consuming bread. 
     Bread is made when you have Bakery. Each bakery produces 1 bread per 20 seconds.
     Units starts loosing stamins after 20 seconds of being active. 5 stamina points every 5 seconds.
     If stamina of a unit reaches 0. Then the unit starts loosing health. 5 health points every 5 seconds.*/
    [SerializeField]
    float staminaPoints, startStamina = 100, time=20f,inerval=5f;
    [SerializeField] bool reduceStamina = false;
    [SerializeField] float damage=5f;
    void Start()
    {
        staminaPoints = startStamina;
        StartCoroutine(StaminaReduction());
        StartCoroutine(ReduceStamina());
        StartCoroutine(TakeDamage());
    }
    public void UpdateStamina()
    {
        staminaPoints = startStamina;
        reduceStamina = false;
        StartCoroutine(StaminaReduction());
    }
    IEnumerator StaminaReduction()
    {
        yield return new WaitForSeconds(time);
        reduceStamina = true;
    }
    IEnumerator TakeDamage()
    {
        yield return new WaitForSeconds(inerval);
        if(staminaPoints<=0)
            GetComponent<Health>().TakeDamage(gameObject,damage);
        StartCoroutine(TakeDamage());
    }
    IEnumerator ReduceStamina()
    {
        yield return new WaitForSeconds(inerval);
        if (reduceStamina)
            staminaPoints -= damage;
        StartCoroutine(ReduceStamina());
    }
    public float GetStaminaFraction()
    {
        return staminaPoints / startStamina;
    }
}
