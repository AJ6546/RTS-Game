using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public int size;
        public GameObject prefab;
        public string tag;
        public float x_max, y_max, z_max;
    }
    public static PoolManager instance;
    private void Awake()
    {
        instance = this;
    }
    public Dictionary<string, Queue<GameObject>> pooldictionary;
    [SerializeField] List<Pool> pools = new List<Pool>();
    void Start()
    {
        pooldictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                Vector3 pos = new Vector3(Random.Range(0, pool.x_max), pool.y_max, Random.Range(0, pool.z_max));
                GameObject prefab = Instantiate(pool.prefab, pos, pool.prefab.transform.rotation);
                ActivatePrefab(ref prefab);
                objPool.Enqueue(prefab);
            }
            pooldictionary.Add(pool.tag, objPool);
        }
    }
    public void Spawn(string tag, Vector3 pos, Transform instantiator, bool instantiatorRot = false)
    {
        GameObject obj = pooldictionary[tag].Dequeue();
        obj.transform.position = pos;
        obj.SetActive(true);
        pooldictionary[tag].Enqueue(obj);
    }
    void ActivatePrefab(ref GameObject prefab)
    {
        switch (prefab.tag)
        {
            case "Pickup":
                prefab.SetActive(true);
                break;
            default:
                prefab.SetActive(false);
                break;
        }
    }
}
