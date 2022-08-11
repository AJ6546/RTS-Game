using UnityEngine;
using UnityEngine.AI;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] int enemyBehaviour = 0;
    NavMeshAgent navMeshAgent;
    Animator animator;
    [SerializeField] Vector3 originalPosition, randomWaypoint, targetPos;

    [SerializeField]
    float wayPointTolrance = 1f, distanceToWaypoint, xMax = 10f,
        zMax = 10f, distanceFromOriginalPos, offset = 3f, timeSinceArrivedAtWaypoint = Mathf.Infinity,
        wayPointDwellTime = 5f, wayPointMoveTime = Mathf.Infinity, timeToReachWaypoint = 30f,
        distanceToTarget, stoppingDistance = 1f, chaseSpeed = 1f, walkSpeed = 0.5f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        originalPosition = randomWaypoint = transform.position;
        walkSpeed = navMeshAgent.speed;
    }

    void Update()
    {
        distanceFromOriginalPos = Vector3.Distance(transform.position, originalPosition);
        if (enemyBehaviour == 0)
        {
            PatrolBehaviour();
        }
        else if (enemyBehaviour == 3)
        {
            SuspicionBehaviour();
        }
        else if (enemyBehaviour == 1)
        {
            ChaseBehaviour();
        }
        UpdateTimers();
    }
    #region Suspicion Behaviour
    private void SuspicionBehaviour()
    {
        navMeshAgent.isStopped = true;
    }
    #endregion
    #region Chase Behaviour
    private void ChaseBehaviour()
    {
        if (AtAttackingDistance())
        {
            enemyBehaviour = 2;
            navMeshAgent.isStopped = true;
            GetComponent<EnemyController>().StartAttacking();
        }
        else
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(targetPos);
            enemyBehaviour = 1;
        }
    }

    public bool AtAttackingDistance()
    {
        distanceToTarget = Vector3.Distance(transform.position, targetPos);
        return distanceToTarget <= stoppingDistance;
    }
    #endregion
    #region Patrol Behaviour
    private void PatrolBehaviour()
    {
        if (AtWayPoint() || wayPointMoveTime > timeToReachWaypoint)
        {
            if (distanceFromOriginalPos <= 1f)
            {
                float xPoint = Random.Range(-xMax, xMax);
                float zPoint = Random.Range(-zMax, zMax);
                xPoint = xPoint < 0 ? transform.position.x + xPoint - offset : transform.position.x + xPoint + offset;
                zPoint = zPoint < 0 ? transform.position.z + zPoint - offset : transform.position.z + zPoint + offset;
                randomWaypoint = new Vector3(xPoint, transform.position.y, zPoint);
            }
            else
            {
                randomWaypoint = originalPosition;
            }
            animator.ResetTrigger("Walk");
            timeSinceArrivedAtWaypoint = 0;
            wayPointMoveTime = 0;
            navMeshAgent.isStopped = true;
        }
        if (timeSinceArrivedAtWaypoint > wayPointDwellTime)
        {
            animator.SetTrigger("Walk");
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(randomWaypoint);
        }
    }

    private bool AtWayPoint()
    {
        distanceToWaypoint = Vector3.Distance(transform.position, randomWaypoint);
        return distanceToWaypoint <= wayPointTolrance;
    }
    #endregion
    void UpdateTimers()
    {
        timeSinceArrivedAtWaypoint += Time.deltaTime;
        wayPointMoveTime += Time.deltaTime;
    }

    public void EnemyBehaviour(int enemyBehaviour, Vector3 targetPos)
    {
        this.enemyBehaviour = enemyBehaviour;
        this.targetPos = targetPos;
    }
}
