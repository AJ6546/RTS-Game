using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAction : MonoBehaviour
{
    [SerializeField] float reach=3f, healthBoost = 25f, damage=5f, unitReach=3f,buildingReach=5f;
    public CharacterClass characterClass;
    [SerializeField]
    GameObject target;
    Animator animator;
    CoolDownTimer cdTimer;
    [SerializeField] bool canDoAction=false;
    
    [SerializeField]  List<GameObject> players = new List<GameObject>();
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    [SerializeField] List<GameObject> trees = new List<GameObject>();
    [SerializeField] List<GameObject> wolves = new List<GameObject>();
    [SerializeField] SoundManager soundManager;
    [SerializeField] string sfx;
    private void Start()
    {
        animator= GetComponent<Animator>();
        cdTimer = GetComponent<CoolDownTimer>();
        soundManager = SoundManager.instance;
    }
    void Update()
    {
        enemies = GetAllEnemiesWithinRange();
        players = GetAllPlayersWithinRange();
        trees = GetAllTreesWithinRange();
        wolves = GetAllWolvesWithinRange();
        if (Time.time> cdTimer.nextAttackTime["Action01"] && CanDoAction())
        {
            animator.SetTrigger("Action");
            cdTimer.nextAttackTime["Action01"] = (int)Time.time + cdTimer.coolDownTime["Action01"];
            soundManager.Play(sfx, transform.position);
        }
    }
    bool CanDoAction()
    {
        canDoAction = false;
        switch(characterClass)
        {
            case CharacterClass.Attacker:
                // if near enemy or enemy Building
                if (CompareTag("Player"))
                {

                    if (enemies.Count >= 1)
                    {
                        foreach (GameObject enemy in enemies)
                        {
                            if (enemy.GetComponent<Health>().GetHealthFraction() >= 0)
                            {
                                canDoAction = true;
                            }
                        }
                    }
                }
                if (CompareTag("Enemy"))
                {
                    if (players.Count >= 1)
                    {
                        foreach (GameObject player in players)
                        {
                            if (player.GetComponent<Health>().GetHealthFraction() >= 0)
                            {
                                canDoAction = true;
                            }
                        }
                    }
                }
                if (wolves.Count >= 1)
                {
                    foreach (GameObject wolf in wolves)
                    {
                        if (wolf.GetComponent<Health>().GetHealthFraction() >= 0)
                        {
                            canDoAction = true;
                        }
                    }
                }
                break;
            case CharacterClass.Healer:
                // if near player and player health < fullHealth
                if (CompareTag("Player"))
                {
                    if (players.Count >= 1)
                    {
                        foreach (GameObject player in players)
                        {
                            if (player.GetComponent<Health>().GetHealthFraction() < 1)
                            {
                                canDoAction = true;
                            }
                        }
                    }
                }
                if(CompareTag("Enemy"))
                {
                    if (enemies.Count >= 1)
                    {
                        foreach (GameObject enemy in enemies)
                        {
                            if (enemy.GetComponent<Health>().GetHealthFraction() < 1)
                            {
                                canDoAction = true;
                            }
                        }
                    }
                }
                break;
            case CharacterClass.Forager:
                // if near tree
                if(trees.Count>=1)
                {
                    foreach (GameObject tree in trees)
                    {
                        if (tree.GetComponent<Health>().GetHealthFraction() >= 0)
                        {
                            canDoAction = true;
                        }
                    }
                }
                break;
        }
        return canDoAction;
    }

    private List<GameObject> GetAllEnemiesWithinRange()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (!enemy.GetComponent<Health>().IsDead() && IsInRange(enemy))
                temp.Add(enemy);
        }
        return temp;
    }
    private List<GameObject> GetAllPlayersWithinRange()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!player.GetComponent<Health>().IsDead() && IsInRange(player))
                temp.Add(player);
        }
        return temp;
    }

    private List<GameObject> GetAllTreesWithinRange()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject tree in GameObject.FindGameObjectsWithTag("Tree"))
        {
            if (!tree.GetComponent<Health>().IsDead() && IsInRange(tree))
                temp.Add(tree);
        }
        return temp;
    }

    private List<GameObject> GetAllWolvesWithinRange()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject wolf in GameObject.FindGameObjectsWithTag("Wolf"))
        {
            if (!wolf.GetComponent<Health>().IsDead() && IsInRange(wolf))
                temp.Add(wolf);
        }
        return temp;
    }


    bool IsInRange(GameObject go)
    {
        if(go.GetComponent<Building>())
        {
            reach = buildingReach;
        }
        else
        {
            reach = unitReach;
        }
        return Vector3.Distance(transform.position, go.transform.position) <= reach;
    }

    void MedicAction()
    {
        if (CompareTag("Player"))
        {
            foreach (GameObject player in players)
            {
                if (player.GetComponent<Health>().GetHealthFraction() < 1)
                {
                    player.GetComponent<Health>().Heal(healthBoost / players.Count);
                }
            }
        }
        if (CompareTag("Enemy"))
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<Health>().GetHealthFraction() < 1)
                {
                    enemy.GetComponent<Health>().Heal(healthBoost / enemies.Count);
                }
            }
        }
    }

    void AttackerAction()
    {
        if (CompareTag("Player"))
        {
            target = FindNearestEnemyorWolf();
            if (target != null)
            {
                gameObject.transform.LookAt(target.transform);
                target.GetComponent<Health>().TakeDamage(gameObject, damage);
            }
        }
        if (CompareTag("Enemy"))
        {
            target = FindNearestPlayerorWolf();
            if (target != null)
            {
                gameObject.transform.LookAt(target.transform);
                target.GetComponent<Health>().TakeDamage(gameObject, damage);
            }
        }
    }
    void WorkerAction()
    {
        target = FindNearestTree();
        gameObject.transform.LookAt(target.transform);
        target.GetComponent<Health>().TakeDamage(gameObject, damage);
    }

    private GameObject FindNearestEnemyorWolf()
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

    private GameObject FindNearestPlayerorWolf()
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

    
}

