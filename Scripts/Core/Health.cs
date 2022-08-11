using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    float healthPoints, startHealth = 100;
    [SerializeField] int killPoint=5, wood=5,meat=1;
    [SerializeField]  bool isDead;
    [SerializeField] UnityEvent<float> takeDamage;
    [SerializeField] GameManager gameManager;
    [SerializeField] PoolManager poolManager;
    [SerializeField] SoundManager soundManager;
    void Start()
    {
        healthPoints = startHealth; // health points is set to start health
        gameManager = FindObjectOfType<GameManager>();
        poolManager = PoolManager.instance;
        soundManager = SoundManager.instance;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            TakeDamage(gameObject, 30);
        }
    }
    private void OnEnable()
    {
        isDead = false;
        healthPoints = startHealth;
    }
    public void TakeDamage(GameObject instigator, float damage)
    {
        if (isDead)
            return;
        healthPoints = Mathf.Lerp(healthPoints, Mathf.Max(healthPoints - damage, 0), Time.deltaTime * 1000);
        if (healthPoints <= 0)
        {
            isDead = true;
            soundManager.Play("Death", transform.position);
            if (instigator.CompareTag("Player") && CompareTag("Enemy"))
            {
                gameManager.UpdatePlayerScore(killPoint); // Player gets points for killing Enemy
            }
            if (instigator.CompareTag("Enemy") && CompareTag("Player"))
            {
                gameManager.UpdateEnemyScore(killPoint); // Enemy gets points for killing Player
            }
            if (instigator.CompareTag("Player") && CompareTag("Tree"))
            {
                gameManager.UpdatePlayerWood(wood); // Player gets points for cutting down Tree
            }
            if (instigator.CompareTag("Enemy") && CompareTag("Tree"))
            {
                gameManager.UpdateEnemyWood(wood); // Enemy gets points for cutting down Tre
            }
            if (instigator.CompareTag("Player") && CompareTag("Wolf"))
            {
                /* When player kills wolf meat is spawned. Player meat count gets updated when player walks through
                and picks it up */
                poolManager.Spawn("Ham", transform.position+transform.up*.12f, transform);
            }
            if (instigator.CompareTag("Enemy") && CompareTag("Wolf")) 
            {
                gameManager.UpdateEnemyMeat(meat); // Update Enemy meat count
            }
            if (instigator.CompareTag("Player") && CompareTag("Wolf"))
            {
                gameManager.UpdatePlayerScore(killPoint); // Player gets points for killing wolf 
            }
            if (instigator.CompareTag("Enemy") && CompareTag("Wolf"))
            {
                gameManager.UpdateEnemyScore(killPoint); // Enemy gets points for killing wolf 
            }
            gameObject.SetActive(false);
        }
        else
        {
            takeDamage.Invoke(damage);
        }
    }
    public float GetHealthFraction()
    {
        return healthPoints / startHealth;
    }
    public void UpdateHealth()
    {
        healthPoints = startHealth; // Revives Health points
    }
    public bool IsDead()
    {
        return isDead;
    }
    public void Heal(float hp)
    {
        healthPoints = Mathf.Min(startHealth, healthPoints + hp);// heals by an amount
    }
}
