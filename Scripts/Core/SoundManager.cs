using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public string tag;
    }
    public static SoundManager instance;
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
            GameObject prefab = Instantiate(pool.prefab, transform.position, pool.prefab.transform.rotation);
            ActivatePrefab(ref prefab);
            objPool.Enqueue(prefab);
            pooldictionary.Add(pool.tag, objPool);
        }

    }
    void ActivatePrefab(ref GameObject prefab)
    {
        prefab.SetActive(false);
    }
    public void Play(string tag, Vector3 pos)
    {
        GameObject obj = pooldictionary[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.position = pos;
        if (obj.GetComponent<AudioSource>())
            obj.GetComponent<AudioSource>().Play();
        pooldictionary[tag].Enqueue(obj);
    }
}
