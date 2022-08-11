using UnityEngine;

public class EnemyFighter : MonoBehaviour
{
    [SerializeField]
    float dmg = 5, distanceToTarget, damagingDistance = 1f;
    Animator animator;
    Health targetHealth;
    CoolDownTimer cdTimer;
    Vector3 targetPos;
    void Start()
    {
        animator = GetComponent<Animator>();
        cdTimer = GetComponent<CoolDownTimer>();
    }

    public void AtackBehaviour(Health targetHealth)
    {
        this.targetHealth = targetHealth;
        if (targetHealth.IsDead())
        {
            animator.ResetTrigger("Action");
            return;
        }
        if (Time.time > cdTimer.nextAttackTime["Action01"])
        {
            animator.SetTrigger("Action");
            cdTimer.nextAttackTime["Action01"] = (int)Time.time + cdTimer.coolDownTime["Action01"];
        }
    }
    void Hit01()
    {
        if (AtDamagingDistance())
            targetHealth.TakeDamage(gameObject,dmg);
    }
    public void UpdateTargetPos(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }

    public bool AtDamagingDistance()
    {
        distanceToTarget = Vector3.Distance(transform.position, targetPos);
        return distanceToTarget <= damagingDistance;
    }
}
