using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyUnitSpawner : MonoBehaviour
{
    [SerializeField] int maxEnemyAttacker, maxEnemyHealer, maxEnemyForager;
    Dictionary<string, int> enemyUnitCountDict = new Dictionary<string, int>();
    [SerializeField] List<GameObject> enemyUnits = new List<GameObject>();
    CoolDownTimer cdTimer;
    [SerializeField] int enemyAttackerCount, enemyHealerCount, enemyForagerCount;
    [SerializeField] TextMeshProUGUI attackerText, foragerText, healerText;

    [SerializeField] PoolManager poolManager;
    void Start()
    {
        cdTimer = GetComponent<CoolDownTimer>();
        poolManager = PoolManager.instance;
        enemyUnitCountDict["EnemyAttacker"] = maxEnemyAttacker; enemyUnitCountDict["EnemyHealer"] = maxEnemyHealer; enemyUnitCountDict["EnemyForager"] = maxEnemyForager;
    }
    void Update()
    {
        enemyUnits = GetAllEnemies();
        enemyAttackerCount = EnemyAttackerCount();
        enemyHealerCount = EnemyHealerCount();
        enemyForagerCount = EnemyForagerCount();
        attackerText.text = "x " + enemyAttackerCount;
        healerText.text = "x " + enemyHealerCount;
        foragerText.text = "x " + enemyForagerCount;
        if (Time.time > cdTimer.nextSpawnTime["Unit01"])
        {
            if (enemyAttackerCount < enemyUnitCountDict["EnemyAttacker"])
            {
                poolManager.Spawn("EnemyAttacker", gameObject.transform.position, transform);
            }
            cdTimer.nextSpawnTime["Unit01"] = (int)Time.time + cdTimer.coolDownTime["Unit01"];
        }
        if (Time.time > cdTimer.nextSpawnTime["Unit02"])
        {
            if (enemyHealerCount < enemyUnitCountDict["EnemyHealer"])
            {
                poolManager.Spawn("EnemyHealer", transform.position, transform);
            }
            cdTimer.nextSpawnTime["Unit02"] = (int)Time.time + cdTimer.coolDownTime["Unit02"];
        }
        if (Time.time > cdTimer.nextSpawnTime["Unit03"])
        {
            if (enemyForagerCount < enemyUnitCountDict["EnemyForager"])
            {
                poolManager.Spawn("EnemyForager", transform.position, transform);
            }
            cdTimer.nextSpawnTime["Unit03"] = (int)Time.time + cdTimer.coolDownTime["Unit03"];
        }

    }

    public void UpdateEnemyUnitDict(string unitName, int number)
    {
        enemyUnitCountDict[unitName] += number;
    }
    public List<GameObject> GetAllEnemies()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (!player.GetComponent<Health>().IsDead())
                temp.Add(player);
        }
        return temp;
    }
    private int EnemyAttackerCount()
    {
        int count = 0;
        foreach (GameObject unit in enemyUnits)
        {
            if (unit.GetComponent<UnitAction>() && unit.GetComponent<UnitAction>().characterClass == CharacterClass.Attacker)
            {
                count++;
            }
        }
        return count;
    }

    private int EnemyHealerCount()
    {
        int count = 0;
        foreach (GameObject unit in enemyUnits)
        {
            if (unit.GetComponent<UnitAction>() && unit.GetComponent<UnitAction>().characterClass == CharacterClass.Healer)
            {
                count++;
            }
        }
        return count;
    }
    private int EnemyForagerCount()
    {
        int count = 0;
        foreach (GameObject unit in enemyUnits)
        {
            if (unit.GetComponent<UnitAction>() && unit.GetComponent<UnitAction>().characterClass == CharacterClass.Forager)
            {
                count++;
            }
        }
        return count;
    }
}
