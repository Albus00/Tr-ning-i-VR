using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;
    public static List<GameObject> enemies = new List<GameObject>();
    private int currentEnemies;
    private int maxEnemies;
    private int currentlyThrowing = 1;
    private int currentlyDashing = 3;
    private int currentlyAttacking = 1;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnEnemy()
    {
        int numOfEnemies = Random.Range(1, 4);
        for (int i = 0; i < numOfEnemies; i++)
        {
            float randomX = Random.Range(-5f, 5f);
            Vector3 spawnPosition = new Vector3(randomX, enemySpawnPoint.position.y, enemySpawnPoint.position.z);
            GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, enemySpawnPoint.rotation);
            enemies.Add(spawnedEnemy); // Add the spawned enemy to the enemies list
            currentEnemies++; // Increment the currentEnemies variable
        }
    }

    public void RemoveEnemy(GameObject enemyToRemove) // Gets called from the enemy when it dies
    {
        // Remove the enemy from the enemies list
        if (enemies.Contains(enemyToRemove))
        {
            enemies.Remove(enemyToRemove);
            currentEnemies--; // Decrement the currentEnemies variable
        }
    }

    private void DoActions()
    {
        // Loop through the enemies list
        // Assign actions based on the distance to the "Player" and what the other enemies are doing
    }

}