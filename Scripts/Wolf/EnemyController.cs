using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Unit nearestTarget = null;
    [SerializeField] float chaseDistance = 5f, suspicionTime = 5f, timeSinceLastSawTarget = Mathf.Infinity;
    [SerializeField] bool targetLocked = false;
    Vector3 targetPos;
    EnemyMover em;
    EnemyFighter ef;
    [SerializeField] int enemyBehaviour;
    [SerializeField] List<Unit> targets = new List<Unit>();

    void Start()
    {
        em = GetComponent<EnemyMover>();
        ef = GetComponent<EnemyFighter>();
    }

    void Update()
    {
        targets = GetAllTargets();
        UpdateNearestTarget(FindNearestTarget());
        timeSinceLastSawTarget += Time.deltaTime;
        if (nearestTarget != null)
        { targetPos = nearestTarget.transform.position; }
        if (GetComponent<Health>().IsDead())
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            return;
        }

        if (targetLocked)
        {
            timeSinceLastSawTarget = 0;
            transform.LookAt(nearestTarget.transform);
            em.EnemyBehaviour(1, targetPos);
        }
        else if (timeSinceLastSawTarget <= suspicionTime)
        {
            em.EnemyBehaviour(3, targetPos);
        }
        else
        {
            em.EnemyBehaviour(0, targetPos);
        }
        ef.UpdateTargetPos(targetPos);
    }
    public bool IsTargetLocked()
    {
        return targetLocked;
    }

    public Unit GetLockedTarget()
    {
        return nearestTarget;
    }
    public void UpdateNearestTarget(Unit nearestTarget)
    {
        this.nearestTarget = nearestTarget;
        StartCoroutine(LockTarget());
    }

    IEnumerator LockTarget()
    {
        targetLocked = IsInRange() && !nearestTarget.GetComponent<Health>().IsDead() ? true : false;
        yield return new WaitForSeconds(5f);
        StartCoroutine(LockTarget());
    }
    bool IsInRange()
    {
        return Vector3.Distance(transform.position, nearestTarget.transform.position) <= chaseDistance;
    }

    public Vector3 GetTargetPosition()
    {
        return nearestTarget.transform.position;
    }

    public void StartAttacking()
    {
        ef.AtackBehaviour(nearestTarget.GetComponent<Health>());
    }

    List<Unit> GetAllTargets()
    {
        List<Unit> temp = new List<Unit>();
        foreach (Unit target in FindObjectsOfType<Unit>())
        {
            if (!target.GetComponent<Health>().IsDead())
            {
                temp.Add(target);
            }
        }
        return temp;
    }
    private Unit FindNearestTarget()
    {
        float minDist = Mathf.Infinity;
        Unit nearestTarget = null;
        foreach (Unit target in targets)
        {
            float dist = FindDistance(target);
            if (minDist > dist)
            {
                nearestTarget = target;
                minDist = dist;
            }
        }
        return nearestTarget;
    }
    private float FindDistance(Unit target)
    {
        return Vector3.Distance(target.transform.position, transform.position);
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

}
