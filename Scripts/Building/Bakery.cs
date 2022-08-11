using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bakery : MonoBehaviour
{
    [SerializeField] int bread = 0, meat = 0; // Bread and Meat Palyer or Enemy Has
    [SerializeField] GameManager gameManager; // Instance of GameManager Script
    [SerializeField] int time=20; // Bakery makes bread after every 20 seconds
    [SerializeField] UnityEvent spawnBread; // Unity Event is Invoked every time bread count is updated
    [SerializeField] List<Unit> units = new List<Unit>(); // List to store units
    [SerializeField]
    Unit unitWithLeastStamina=null; // Unit with the least stamina among all units.
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(UpdateBread()); // Updates BreadCount after 20 seconds
    }
    private void Update()
    {
        if (CompareTag("Player"))
        {
            /* If Bakery Belongs to player update bread with playerBreadCount,
             meat with playerMeatCount and units becomes list of all player units*/
            bread = gameManager.GetPlayerBread(); 
            units = GetAllPlayerUnits();
            meat=gameManager.GetPlayerMeat();
        }
        if (CompareTag("Enemy"))
        {
            /* If Bakery Belongs to enemy update bread with enemyBreadCount,
             meat with enemyMeatCount and units becomes list of all enemy units*/
            bread = gameManager.GetEnemyBread();
            units = GetAllEnemyUnits();
            meat = gameManager.GetEnemyMeat();
        }
        FeedUnitWithLeastStamina();
    }
    IEnumerator UpdateBread()
    {
        /* This Recursive Coroutine updates bread Count every 20 seconds */
        yield return new WaitForSeconds(time);
        spawnBread.Invoke();
        if(CompareTag("Player"))
            gameManager.UpdatePlayerBread(1);
        if(CompareTag("Enemy"))
            gameManager.UpdateEnemyBread(1);
        StartCoroutine(UpdateBread());
    }
    List<Unit> GetAllPlayerUnits()
    {
        /* Method to get all Player Units who has stamina lower than max stamina*/
        List<Unit> temp = new List<Unit>();
        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            if(unit.CompareTag("Player") && unit.GetComponent<Stamina>().GetStaminaFraction() < 1)
            {
                temp.Add(unit);
            }
        }
        return temp;
    }
    List<Unit> GetAllEnemyUnits()
    {
        /* Method to get all Enemy Units who has stamina lower than max stamina*/
        List<Unit> temp = new List<Unit>();
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (unit.CompareTag("Enemy") && unit.GetComponent<Stamina>().GetStaminaFraction() < 1)
            {
                temp.Add(unit);
            }
        }
        return temp;
    }
    void FeedUnitWithLeastStamina()
    {
        /* Method to get Unit with least Stamin by going through the list units
         if unit with least stamina is not null, we use up bread to restore the unit's stamina,
         also if the unit has health lower than max health, it's health is updated using up meat, if meat>1.*/
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
        if(unitWithLeastStamina!=null)
        {
            unitWithLeastStamina.GetComponent<Stamina>().UpdateStamina();
            if (CompareTag("Player"))
            {
                gameManager.UpdatePlayerBread(-1);
                if (unitWithLeastStamina.GetComponent<Health>().GetHealthFraction() < 1 && meat > 0)
                {
                    unitWithLeastStamina.GetComponent<Health>().UpdateHealth();
                    gameManager.UpdatePlayerMeat(-1);
                }
            }
            if (CompareTag("Enemy"))
            {
                gameManager.UpdateEnemyBread(-1);
                if (unitWithLeastStamina.GetComponent<Health>().GetHealthFraction() < 1 && meat > 0)
                {
                    unitWithLeastStamina.GetComponent<Health>().UpdateHealth();
                    gameManager.UpdateEnemyMeat(-1);
                }
            }
           
        }
    }
}
