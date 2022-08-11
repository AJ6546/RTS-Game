using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    /* Used to Spawn trees are random position within map. In range of (xMin=400, xMax=600),y=0 and (zMin=400,zMAx=600).
     Once a tree is cut down, a new one spawns after 30 seconds*/
    [SerializeField] string tree;
    [SerializeField] PoolManager poolManager;
    [SerializeField] int treeCount=50;
    [SerializeField] float xMin=400, xMax=600, zMin= 400, zMax=600, waitTime=30;
    [SerializeField] List<GameObject> trees = new List<GameObject>();
    [SerializeField] bool spawnTree = true;
    void Start()
    {
        poolManager = PoolManager.instance;
        StartCoroutine(SpawnAllTree());
    }
    private void Update()
    {
        trees = GetAllTrees();
        if(trees.Count<treeCount && spawnTree)
        {
            spawnTree = false;
            StartCoroutine(SpawnTree());
        }
    }
    IEnumerator SpawnAllTree()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < treeCount; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(xMin, xMax), 0f, Random.Range(zMin, zMax));
            poolManager.Spawn(tree, spawnPos, transform);
        }
    }
    IEnumerator SpawnTree()
    {
        yield return new WaitForSeconds(waitTime);
        Vector3 spawnPos = new Vector3(Random.Range(xMin, xMax), 0f, Random.Range(zMin, zMax));
        poolManager.Spawn(tree, spawnPos, transform);
        spawnTree = true;
    }
    public List<GameObject> GetAllTrees()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject tree in GameObject.FindGameObjectsWithTag("Tree"))
        {
            if (!tree.GetComponent<Health>().IsDead())
                temp.Add(tree);
        }
        return temp;
    }
}
