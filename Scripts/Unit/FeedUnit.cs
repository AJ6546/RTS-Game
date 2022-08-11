using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedUnit : MonoBehaviour
{
    /* Used to manually feed bread to player unit with least stamina by clicking bread icon at top right corner.
     Used to manually feed meat to player unit with least health by clicking meat icon at top right corner.*/
    [SerializeField] List<Unit> units = new List<Unit>();
    [SerializeField] GameManager gameManager;
    [SerializeField] int bread = 0, meat=0;
    [SerializeField] Unit unitWithLeastStamina=null, unitWithLeastHealth=null;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        bread = gameManager.GetPlayerBread();
        meat = gameManager.GetPlayerMeat();
        units = GetAllPlayerUnits();
    }
    public void OnBreadClick()
    {
        FeedUnitWithLeastStamina();
    }
    public void OnMeatClick()
    {
        FeedUnitWithLeastStamina();
    }
    List<Unit> GetAllPlayerUnits()
    {
        List<Unit> temp = new List<Unit>();
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (unit.CompareTag("Player") && unit.GetComponent<Stamina>().GetStaminaFraction() < 1)
            {
                temp.Add(unit);
            }
        }
        return temp;
    }
    void FeedUnitWithLeastStamina()
    {
        float minStamina = 1f;
        unitWithLeastStamina = null;
        if (bread > 0)
        {
            foreach (Unit unit in units)
            {
                float unitStamina = unit.GetComponent<Stamina>().GetStaminaFraction();
                if (unitStamina < minStamina)
                {
                    minStamina = unitStamina;
                    unitWithLeastStamina = unit;
                }
            }
        }
        if (unitWithLeastStamina != null)
        {
            gameManager.UpdatePlayerBread(-1);
            unitWithLeastStamina.GetComponent<Stamina>().UpdateStamina();
        }
    }
    void HealUnitWithLEastHealth()
    {
        float minHealth = 1f;
        unitWithLeastHealth = null;
        if (meat > 0)
        {
            foreach (Unit unit in units)
            {
                float unitHealth = unit.GetComponent<Health>().GetHealthFraction();
                if (unitHealth < minHealth)
                {
                    minHealth = unitHealth;
                    unitWithLeastHealth = unit;
                }
            }
        }
        if (unitWithLeastHealth != null)
        {
            gameManager.UpdatePlayerMeat(-1);
            unitWithLeastHealth.GetComponent<Health>().UpdateHealth();
        }
    }
}
