using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;
    private GameObject[] enemies; // store alive enemies
    // Start is called before the first frame update
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
            Instantiate(enemyPrefab, spawnPosition, enemySpawnPoint.rotation); //just to stop spawning while testing
        }
        
        
        // instantiate
        // lägg till i enemies[]
    }

    public void RemoveEnemy()
    {
        // hitta fiende i enemies[]
        // ta bort
    }

    private void DoActions()
    {
        // loopa enemies[]
        // ge action beroende på avstånd till "Player" samt vad de andra gör

    }
}
