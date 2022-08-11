using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitMovement : MonoBehaviour
{
    [SerializeField] UnitSpawner unitSpawner;
    void Start()
    {
        unitSpawner = FindObjectOfType<UnitSpawner>();
    }

    void Update()
    {
        if(GetComponent<UnitAction>().characterClass.Equals(CharacterClass.Attacker))
        {
            if(unitSpawner.GetAutoAttack())
            {
                GetComponent<UnitMovement>().enabled = true;
            }
        }
        if (GetComponent<UnitAction>().characterClass.Equals(CharacterClass.Healer))
        {
            if (unitSpawner.GetAutoHeal())
            {
                GetComponent<UnitMovement>().enabled = true;
            }
        }
        if (GetComponent<UnitAction>().characterClass.Equals(CharacterClass.Forager))
        {
            if (unitSpawner.GetAutoForage())
            {
                GetComponent<UnitMovement>().enabled = true;
            }
        }
    }
}
