using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    void Start()
    {
        UnitSelections.Instance.unitsList.Add(this.gameObject);
    }
    void OnDestroy()
    {
        UnitSelections.Instance.unitsList.Remove(this.gameObject);
    }
}
