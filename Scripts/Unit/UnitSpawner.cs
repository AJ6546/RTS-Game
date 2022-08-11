using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] int maxAttacker, maxHealer, maxForager;
    Dictionary<string, int> unitCountDict = new Dictionary<string, int>();
    [SerializeField] List<GameObject> units = new List<GameObject>();
    CoolDownTimer cdTimer;
    [SerializeField] int attackerCount, healerCount, foragerCount;
    [SerializeField] TextMeshProUGUI attackerText, foragerText, healerText;
    [SerializeField] PoolManager poolManager;
    [SerializeField] bool autoAttack = false, autoHeal = false, autoForage = false;
    [SerializeField] Button autoAttackButton, autoHealButton, autoForageButton;
    void Start()
    {
        cdTimer = GetComponent<CoolDownTimer>();
        poolManager = PoolManager.instance;
        unitCountDict["Attacker"] = maxAttacker; unitCountDict["Healer"] = maxHealer; unitCountDict["Forager"] = maxForager;
    }
    void Update()
    {
        units = GetAllPlayers();
        attackerCount = AttackerCount();
        healerCount = HealerCount();
        foragerCount = ForagerCount();
        attackerText.text = "x " + attackerCount;
        healerText.text = "x " + healerCount;
        foragerText.text = "x " + foragerCount;
        if (Time.time > cdTimer.nextSpawnTime["Unit01"])
        {
            if(attackerCount< unitCountDict["Attacker"])
            {
                poolManager.Spawn("Attacker", gameObject.transform.position, transform);
            }
            cdTimer.nextSpawnTime["Unit01"] = (int)Time.time + cdTimer.coolDownTime["Unit01"];
        }
        if(Time.time > cdTimer.nextSpawnTime["Unit02"])
        {
            if (healerCount < unitCountDict["Healer"])
            {
                poolManager.Spawn("Healer", transform.position, transform);
            }
            cdTimer.nextSpawnTime["Unit02"] = (int)Time.time + cdTimer.coolDownTime["Unit02"];
        }
        if (Time.time > cdTimer.nextSpawnTime["Unit03"])
        {
            if (foragerCount < unitCountDict["Forager"])
            {
                poolManager.Spawn("Forager", transform.position, transform);
            }
            cdTimer.nextSpawnTime["Unit03"] = (int)Time.time + cdTimer.coolDownTime["Unit03"];
        }
        SetResetButtonColor();
    }

    public void UpdateUnitDict(string unitName,int number)
    {
        unitCountDict[unitName] += number;
    }
    public List<GameObject> GetAllPlayers()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!player.GetComponent<Health>().IsDead())
                temp.Add(player);
        }
        return temp;
    }
    private int AttackerCount()
    {
        int count = 0;
        foreach(GameObject unit in units)
        {
            if(unit.GetComponent<UnitAction>() && unit.GetComponent<UnitAction>().characterClass.Equals(CharacterClass.Attacker))
            {
                count++;
            }
        }
        return count;
    }

    private int HealerCount()
    {
        int count = 0;
        foreach (GameObject unit in units)
        {
            if (unit.GetComponent<UnitAction>() && unit.GetComponent<UnitAction>().characterClass == CharacterClass.Healer)
            {
                count++;
            }
        }
        return count;
    }
    private int ForagerCount()
    {
        int count = 0;
        foreach (GameObject unit in units)
        {
            if (unit.GetComponent<UnitAction>() && unit.GetComponent<UnitAction>().characterClass == CharacterClass.Forager)
            {
                count++;
            }
        }
        return count;
    }

    public void SetAutoAttack()
    {
        autoAttack = !autoAttack;
        if(!autoAttack)
        {
            DisableMovement(CharacterClass.Attacker);
        }
    }
    public void SetAutoHeal()
    {
        autoHeal = !autoHeal;
        if (!autoHeal)
        {
            DisableMovement(CharacterClass.Healer);
        }
    }
    public void SetAutoForage()
    {
        autoForage = !autoForage;
        if (!autoForage)
        {
            DisableMovement(CharacterClass.Forager);
        }
    }
    public bool GetAutoAttack()
    {
        return autoAttack;
    }
    public bool GetAutoHeal()
    {
        return autoHeal;
    }
    public bool GetAutoForage()
    {
        return autoForage;
    }
    void SetResetButtonColor()
    {
        if(autoAttack)
        {
            autoAttackButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            autoAttackButton.GetComponent<Image>().color = Color.white;
        }
        if (autoHeal)
        {
            autoHealButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            autoHealButton.GetComponent<Image>().color = Color.white;
        }
        if (autoForage)
        {
            autoForageButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            autoForageButton.GetComponent<Image>().color = Color.white;
        }
    }

    void DisableMovement(CharacterClass character)
    {
        foreach(GameObject player in units)
        {
            if(player.GetComponent<UnitAction>().characterClass.Equals(character))
            {
                player.GetComponent<UnitMovement>().enabled = false;            
            }
        }
    }
}
