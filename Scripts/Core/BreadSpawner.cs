using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadSpawner : MonoBehaviour
{
    [SerializeField] Bread bread;
    public void Spawn()
    {
        // Spawns a bread sprite from the bakery
        Bread instance = Instantiate<Bread>(bread,transform);
    }
}
