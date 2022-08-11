using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawner : MonoBehaviour
{
    /* Used to Spawn wolves are random position within map. In range of (xMin=400, xMax=600),y=0 and (zMin=400,zMAx=600).
     Once a wolf is killed a new one spawns after 30 seconds*/
    [SerializeField] string wolf;
    [SerializeField] PoolManager poolManager;
    [SerializeField] int wolfCount = 5;
    [SerializeField] float xMin = 400, xMax = 600, zMin = 400, zMax = 600, waitTime = 30;
    [SerializeField] List<GameObject> wolves = new List<GameObject>();
    [SerializeField] bool spawnWolf = true;
    void Start()
    {
        poolManager = PoolManager.instance;
        StartCoroutine(SpawnAllWolves());
    }
    private void Update()
    {
        wolves = GetAllWolves();
        if (wolves.Count < wolfCount && spawnWolf)
        {
            spawnWolf = false;
            StartCoroutine(SpawnWolf());
        }
    }
    IEnumerator SpawnAllWolves()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < wolfCount; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(xMin, xMax), 0f, Random.Range(zMin, zMax));
            poolManager.Spawn(wolf, spawnPos, transform);
        }
    }
    IEnumerator SpawnWolf()
    {
        yield return new WaitForSeconds(waitTime);
        Vector3 spawnPos = new Vector3(Random.Range(xMin, xMax), 0f, Random.Range(zMin, zMax));
        poolManager.Spawn(wolf, spawnPos, transform);
        spawnWolf = true;
    }
    public List<GameObject> GetAllWolves()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject tree in GameObject.FindGameObjectsWithTag("Wolf"))
        {
            if (!tree.GetComponent<Health>().IsDead())
                temp.Add(tree);
        }
        return temp;
    }
}
