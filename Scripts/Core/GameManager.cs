using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int playerScore;
    [SerializeField] int enemyScore;
    [SerializeField] int playerWood;
    [SerializeField] int enemyWood;
    [SerializeField] int playerBread;
    [SerializeField] int enemyBread;
    [SerializeField] int playerMeat;
    [SerializeField] int enemyMeat;

    [SerializeField] int buildIndex;

    [SerializeField] TextMeshProUGUI playerScoreText;
    [SerializeField] TextMeshProUGUI enemyScoreText;
    [SerializeField] TextMeshProUGUI playerWoodText;
    [SerializeField] TextMeshProUGUI enemyWoodText;
    [SerializeField] TextMeshProUGUI playerBreadText;
    [SerializeField] TextMeshProUGUI enemyBreadText;
    [SerializeField] TextMeshProUGUI playerMeatText;
    [SerializeField] TextMeshProUGUI enemyMeatText;

    [SerializeField] BuildingSystem buildingSystem;
    private void Start()
    {
        buildingSystem = FindObjectOfType<BuildingSystem>();
        buildIndex = SceneManager.GetActiveScene().buildIndex;
    }
    private void Update()
    {
        playerScoreText.text = "Player Score: " + playerScore.ToString();
        enemyScoreText.text = "Enemy Score: " + enemyScore.ToString();
        playerWoodText.text = "x " + playerWood.ToString();
        enemyWoodText.text = "x " + enemyWood.ToString();
        playerBreadText.text = "x " + playerBread.ToString();
        enemyBreadText.text = "x " + enemyBread.ToString();
        playerMeatText.text = "x " + playerMeat.ToString();
        enemyMeatText.text = "x " + enemyMeat.ToString();
    }
    public void UpdatePlayerScore(int score)
    {
        /* Gained from cutting wood, killing fox and enemy units and destroying enemy buildings */
        playerScore += score;
    }

    public void UpdateEnemyScore(int score)
    {
        /* Gained from cutting wood, killing fox and player units and destroying player buildings */
        enemyScore += score;
    }
    public void UpdatePlayerWood(int wood)
    {
        // Wood is add when the forager unit cuts down trees
        playerWood += wood;
        buildingSystem.UpdateWoodCount(playerWood);
    }

    public void UpdateEnemyWood(int wood)
    {
        // Wood is add when the forager unit cuts down trees
        enemyWood += wood;
    }
    public void UpdatePlayerBread(int bread)
    {
        // Bakery Spawns bread every 20 seconds
        playerBread += bread;
    }

    public void UpdateEnemyBread(int bread)
    {
        // Bakery Spawns bread every 20 seconds
        enemyBread += bread;
    }
    public void UpdatePlayerMeat(int meat)
    {
        // Meat is added when attacker unit kills fox 
        playerMeat += meat;
    }

    public void UpdateEnemyMeat(int meat)
    {
        // Meat is added when attacker unit kills fox 
        enemyMeat += meat;
    }
    public int GetWoodCount()
    {
        // return player Wood Count
        return playerWood;
    }
    public void SetPlayerWood(int wood)
    {
        // Sets player Wood Count
        playerWood = wood;
    }
    public int GetEnemyWood()
    {
        // returns Enemy wood Count
        return enemyWood;
    }
    public int GetPlayerMeat()
    {
        // returns Player Meat Count
        return playerMeat;
    }
    public int GetEnemyMeat()
    {
        // returns Enemy Meat Count
        return enemyMeat;
    }
    public int GetPlayerBread()
    {
        // returns Player Bread Count
        return playerBread;
    }
    public int GetEnemyBread()
    {
        // returns Enemy Bread Count
        return enemyBread;
    }
    public void ExitGame()
    {
        // Go back to Start Game Window
        SceneManager.LoadScene(buildIndex - 1);
    }
    public void RestartGame()
    {
        // Restarts Game 
        SceneManager.LoadScene(buildIndex);
    }
}
