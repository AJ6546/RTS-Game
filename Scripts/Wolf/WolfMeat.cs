using UnityEngine;

public class WolfMeat : MonoBehaviour
{
    [SerializeField] int meat = 1;
    [SerializeField] GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            gameManager.UpdatePlayerMeat(meat);
        if(other.CompareTag("Enemy"))
            gameManager.UpdateEnemyMeat(meat);
        gameObject.SetActive(false);
    }
}
