using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownTimer : MonoBehaviour
{
    [SerializeField] int[] actionTimers = new int[1];
    [SerializeField] int[] spawnTimers = new int[1];
    public Dictionary<string, int> coolDownTime = new Dictionary<string, int>();
    public Dictionary<string, int> nextAttackTime = new Dictionary<string, int>();
    public Dictionary<string, int> nextSpawnTime = new Dictionary<string, int>();
    void Start()
    {
        // Used to set time Interval between attacks
        for (int i = 0; i < actionTimers.Length; i++)
        {
            coolDownTime["Action0" + (i + 1).ToString()] = actionTimers[i];
            nextAttackTime["Action0" + (i + 1).ToString()] = 0;
        }
        // Used to set time interval between spawning units
        for (int i = 0; i < spawnTimers.Length; i++)
        {
            coolDownTime["Unit0" + (i + 1).ToString()] = spawnTimers[i];
            nextSpawnTime["Unit0" + (i + 1).ToString()] = 0;
        }
    }
}
