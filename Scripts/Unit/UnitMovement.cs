using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{

    NavMeshAgent myAgent;
    Camera myCam;
    Animator myAnim;
    public LayerMask ground;

    [SerializeField] UnitSpawner unitSpawner;
    [SerializeField] EnemyUnitSpawner enemyUnitSpawner;
    [SerializeField] TreeSpawner treeSpawner;
    [SerializeField] WolfSpawner wolfSpawner;

    [SerializeField] List<GameObject> players = new List<GameObject>();
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    [SerializeField] List<GameObject> trees = new List<GameObject>();
    [SerializeField] List<GameObject> wolves = new List<GameObject>();
    [SerializeField] float targetPositionOffset=1f, buildingOffset=3f,unitOffset=1f;
    [SerializeField] Vector3 targetPosition;

    [SerializeField] bool autoAttack = false, autoHeal = false, autoForage = false;
    void Start()
    {
        unitSpawner = FindObjectOfType<UnitSpawner>();
        enemyUnitSpawner = FindObjectOfType<EnemyUnitSpawner>();
        treeSpawner = FindObjectOfType<TreeSpawner>();
        wolfSpawner = FindObjectOfType<WolfSpawner>();

        myCam = Camera.main;
        myAnim = GetComponent<Animator>();
        myAgent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        players = unitSpawner.GetAllPlayers();
        enemies = enemyUnitSpawner.GetAllEnemies();
        trees = treeSpawner.GetAllTrees();
        wolves = wolfSpawner.GetAllWolves();

        autoAttack = unitSpawner.GetAutoAttack();
        autoHeal = unitSpawner.GetAutoHeal();
        autoForage = unitSpawner.GetAutoForage();

        enemies.Remove(gameObject);
        if (CompareTag("Player") && Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                targetPosition = hit.point;
                myAgent.isStopped = false;
                myAnim.SetBool("Walk",true);
                myAgent.SetDestination(targetPosition);
            }
        }
        if(CompareTag("Enemy"))
        {
            if (GetComponent<UnitAction>().characterClass == CharacterClass.Attacker)
            {
                GameObject nearestPlayer = FindNearestPlayerOrWolf();
                if(nearestPlayer.GetComponent<Building>())
                {
                    targetPositionOffset = buildingOffset;
                }
                else
                {
                    targetPositionOffset = unitOffset;
                }
                targetPosition = nearestPlayer.transform.position;
            }
            if (GetComponent<UnitAction>().characterClass == CharacterClass.Healer)
            {
                GameObject nearestEnemy = FindNearestEnemyNotHealer();
                if (nearestEnemy.GetComponent<Building>())
                {
                    targetPositionOffset = buildingOffset;
                }
                else
                {
                    targetPositionOffset = unitOffset;
                }
                targetPosition = nearestEnemy.transform.position;
            }
            if (GetComponent<UnitAction>().characterClass == CharacterClass.Forager)
            {
                if (trees.Count > 0 || wolves.Count>0)
                {
                    GameObject nearestTarget = FindNearestTree();
                    targetPosition = nearestTarget.transform.position;
                }
            }
            myAgent.isStopped = false;
            myAnim.SetBool("Walk", true);
            myAgent.SetDestination(targetPosition);
        }
        if(CompareTag("Player"))
        {
            if(autoAttack)
            {
                if (GetComponent<UnitAction>().characterClass == CharacterClass.Attacker)
                {
                    GameObject nearestEnemy = FindNearestEnemyOrWolf();
                    if (nearestEnemy.GetComponent<Building>())
                    {
                        targetPositionOffset = buildingOffset;
                    }
                    else
                    {
                        targetPositionOffset = unitOffset;
                    }
                    targetPosition = nearestEnemy.transform.position;
                    myAgent.isStopped = false;
                    myAnim.SetBool("Walk", true);
                    myAgent.SetDestination(targetPosition);
                }
            }
            if(autoHeal)
            {
                if (GetComponent<UnitAction>().characterClass == CharacterClass.Healer)
                {
                    GameObject nearestPlayer = FindNearestPlayerNotHealer();
                    if (nearestPlayer.GetComponent<Building>())
                    {
                        targetPositionOffset = buildingOffset;
                    }
                    else
                    {
                        targetPositionOffset = unitOffset;
                    }
                    targetPosition = nearestPlayer.transform.position;
                    myAgent.isStopped = false;
                    myAnim.SetBool("Walk", true);
                    myAgent.SetDestination(targetPosition);
                }
            }
            if(autoForage)
            {
                if (GetComponent<UnitAction>().characterClass == CharacterClass.Forager)
                {
                    if (trees.Count > 0 || wolves.Count > 0)
                    {
                        GameObject nearestTarget = FindNearestTree();
                        targetPosition = nearestTarget.transform.position;
                    }
                    myAgent.isStopped = false;
                    myAnim.SetBool("Walk", true);
                    myAgent.SetDestination(targetPosition);
                }
            }
        }
        if(ReachedDestination())
        {
            myAnim.SetBool("Walk", false);
            myAgent.isStopped = true;
        }
    }
    GameObject FindNearestPlayerOrWolf()
    {
        float minDist = Mathf.Infinity;
        GameObject nearestTarget = null;
        foreach (GameObject player in players)
        {
            float dist = FindDistance(player);
            if (minDist > dist)
            {
                nearestTarget = player;
                minDist = dist;
            }
        }
        foreach (GameObject wolf in wolves)
        {
            float dist = FindDistance(wolf);
            if (minDist > dist)
            {
                nearestTarget = wolf;
                minDist = dist;
            }
        }
        return nearestTarget;
    }
    GameObject FindNearestPlayerNotHealer()
    {
        float minDist = Mathf.Infinity;
        GameObject nearestPlayer = null;
        foreach (GameObject player in players)
        {
            if (player.GetComponent<UnitAction>() && !player.GetComponent<UnitAction>().characterClass.Equals(CharacterClass.Healer))
            {
                float dist = FindDistance(player);
                if (minDist > dist)
                {
                    nearestPlayer = player;
                    minDist = dist;
                }
            }
        }
        return nearestPlayer;
    }
    private GameObject FindNearestEnemyOrWolf()
    {
        float minDist = Mathf.Infinity;
        GameObject nearestTarget = null;
        foreach (GameObject enemy in enemies)
        {
            float dist = FindDistance(enemy);
            if (minDist > dist)
            {
                nearestTarget = enemy;
                minDist = dist;
            }
        }
        foreach (GameObject wolf in wolves)
        {
            float dist = FindDistance(wolf);
            if (minDist > dist)
            {
                nearestTarget = wolf;
                minDist = dist;
            }
        }
        return nearestTarget;
    }
    private GameObject FindNearestEnemyNotHealer()
    {
        float minDist = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<UnitAction>() && !enemy.GetComponent<UnitAction>().characterClass.Equals(CharacterClass.Healer))
            {
                float dist = FindDistance(enemy);
                if (minDist > dist)
                {
                    nearestEnemy = enemy;
                    minDist = dist;
                }
            }
        }
        return nearestEnemy;
    }
    private GameObject FindNearestTree()
    {
        float minDist = Mathf.Infinity;
        GameObject nearestTarget = null;
        foreach (GameObject tree in trees)
        {
            float dist = FindDistance(tree);
            if (minDist > dist)
            {
                nearestTarget = tree;
                minDist = dist;
            }
        }
        return nearestTarget;
    }
    private float FindDistance(GameObject targt)
    {
        return Vector3.Distance(targt.transform.position, transform.position);
    }
    bool ReachedDestination()
    {
        return Vector3.Distance(transform.position, targetPosition) <= targetPositionOffset;
    }
}
