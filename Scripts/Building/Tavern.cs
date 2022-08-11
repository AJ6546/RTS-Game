using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tavern : MonoBehaviour
{
    [SerializeField] EnemyUnitSpawner enemyUnitSpawner;
    [SerializeField] UnitSpawner unitSpawner;
    [SerializeField] bool updateCapacity=false;
    [SerializeField] int attakerUpdateAmt=4, healerUpdateAmt=3, foragerUpdateAmt=2;
    void Start()
    {
        enemyUnitSpawner = FindObjectOfType<EnemyUnitSpawner>();
        unitSpawner = FindObjectOfType<UnitSpawner>();
        updateCapacity = false;
    }
    void Update()
    {
        /* Tavern is used to update the capacity of player or enemy units.
         Attacker+4, Healers+3 and Foragers+2 for every Tavern*/
        if(!updateCapacity)
        {
            if (GetComponent<Building>().IsReady())
            {
                updateCapacity = true;
                if(CompareTag("Player"))
                {
                    unitSpawner.UpdateUnitDict("Attacker", attakerUpdateAmt);
                    unitSpawner.UpdateUnitDict("Healer", attakerUpdateAmt);
                    unitSpawner.UpdateUnitDict("Forager", attakerUpdateAmt);
                }
                if (CompareTag("Enemy"))
                {
                    enemyUnitSpawner.UpdateEnemyUnitDict("EnemyAttacker", attakerUpdateAmt);
                    enemyUnitSpawner.UpdateEnemyUnitDict("EnemyHealer", attakerUpdateAmt);
                    enemyUnitSpawner.UpdateEnemyUnitDict("EnemyForager", attakerUpdateAmt);
                }
            }
        }
    }
}
