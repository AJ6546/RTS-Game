using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuildingSystem : MonoBehaviour
{
    [SerializeField] bool  saveWood=false; 
    [SerializeField] GameManager gameManager;
    [SerializeField] PoolManager poolManager;
    [SerializeField] int enemyWoodCount;
    [SerializeField] List<Building> enemyBuildings = new List<Building>();
    [SerializeField] int maxCount=5, building1WoodCount=100, building2WoodCount=150;
    [SerializeField] string building1="EnemyTavern", building2= "EnemyBakery";
    [SerializeField] float xMin=500f, xMax=600f, zMin=500f, zMax=600f;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        poolManager = PoolManager.instance;
        saveOrBuilt();
    }
    void Update()
    {
        enemyBuildings = GetEnemyBuildings();
        enemyWoodCount = gameManager.GetEnemyWood(); // get enemy wood count from game manager
        if (enemyBuildings.Count >= maxCount)
        {
            /* If enemy has maxed out number of buildings returns without executing rest of the script, unless
             enemy has wood count of 150. In this case one of the existing building is removed and the 
             rest of the script is executed to build a new one.*/
            if(enemyWoodCount < building2WoodCount)
            {
                return;
            }
            else
            {
                int temp = Random.Range(0, 5);
                enemyBuildings[temp].ResetPlacedandReady();
                enemyBuildings[temp].gameObject.SetActive(false);
            }
        }
        
        if(enemyWoodCount > building1WoodCount && !saveWood)
        {
            // spawns an enemy Tavern at random location ranging (xMin=500, xMax=600) y=0 and (zMin=500, zMax=600) 
            poolManager.Spawn(building1, new Vector3(Random.Range(xMin, xMax), 0, Random.Range(zMin, zMax)), transform);
            gameManager.UpdateEnemyWood(-building1WoodCount); // reduces wood count by 100
            saveOrBuilt();
        }
        if (enemyWoodCount > building2WoodCount && saveWood)
        {
            // spawns an enemy Bakey at random location ranging (xMin=500, xMax=600) y=0 and (zMin=500, zMax=600) 
            poolManager.Spawn(building2, new Vector3(Random.Range(xMin, xMax), 0, Random.Range(zMin, zMax)), transform);
            gameManager.UpdateEnemyWood(-building2WoodCount); // reduces wood count by 150
            saveOrBuilt();
        }
    }

    List<Building> GetEnemyBuildings()
    {
        // Returns all enemy buildings
        List<Building> temp = new List<Building>();
        foreach(Building b in FindObjectsOfType<Building>())
        {
            if(b.CompareTag("Enemy"))
            {
                temp.Add(b);
                if(!b.IsReady())
                {
                    b.SetReady();
                }
            }
        }
        return temp;
    }
    void saveOrBuilt()
    {
        /* A method to randomly decide whether to build Tavern or Bakery. Called at the Start of the Game and
         every time a new building is built. if saveWood is true, Enemy saves the wood after reaching 100. 
         Does not build Tavern, waits till wood count reaches 150 to build Bakery
         */
        int temp = Random.Range(0, 10);
        if (temp % 2 == 0)
        {
            saveWood = true;
        }
        else
        {
            saveWood = false;
        }
    }
}
