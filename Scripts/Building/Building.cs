using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] bool isPlaced=false; // Set to true when a building is placed. 
    [SerializeField] bool isReady = false; // Set to true when a building is ready.
    [SerializeField] int readyTime = 5;
    public BuildingType buildingType;
    public bool IsPlaced()
    {
        return isPlaced;
    }
    public bool IsReady()
    {
        return isReady;
    }
    public void IsPlaced(bool isPlaced)
    {
        this.isPlaced = isPlaced;
    }
    IEnumerator Ready()
    {
        /* Once the building is ready, we see the transarent block around it go and
         building becomes active */
        yield return new WaitForSeconds(readyTime);
        isReady = true;
    }
    public void SetReady()
    {
        /* Called when a building gets placed. Starts a coroutine to construct the building. 
         The default values are 60 seconds for Taverna and 90 seconds for Bakery*/
        StartCoroutine(Ready());
    }
    public void ResetPlacedandReady()
    {
        /* This Method is called when a building is destroyed */
        isReady = false;
        isPlaced = false;
    }
}
