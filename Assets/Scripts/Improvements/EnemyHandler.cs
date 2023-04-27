//TRÄNING I VR
//Klas Nordquist
//Victor Persson


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public moneyAndScoreManager scoreManager;
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;
    public static List<GameObject> enemies = new List<GameObject>();

    public int moneyPerWave = 400;
    public int scorePerWave = 400;
    
    private int enemiesLeftToSpawn;
    private int maxAliveEnemies = 4;
    private int amtEnemiesWave = 4;
    private int amtSpawnSimult = 2;
    private int addEnemyPerWave = 2;
    private int addMaxAlive = 1;
    
    private int currentlyThrowing = 1;
    private int currentlyDashing = 3;
    private int currentlyAttacking = 1;

    private int cooldownTimer = 15; //Cooldown in seconds

    public int amtEnemiesDefeated = 0;
    public int currentWave = 0;
    public int amtEnemiesAlive = 0;

    private bool waveActive = false;
    private bool waveStart = false;

    private bool cooldownActive = true;

    private IEnumerator coroutine = null; //Dont initalize cooldown more than once



    void Start()
    {
        //First wave only
        enemiesLeftToSpawn = amtEnemiesWave;
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldownActive && this.coroutine == null){
            if(currentWave > 0){
                scoreManager.giveMoney(moneyPerWave, false);
                scoreManager.giveScore(scorePerWave);
            }
            this.coroutine = cooldown();
            StartCoroutine(this.coroutine);
        }

        //Start wave
        if(waveStart == true && waveActive == false){
            //Calculate amount of enemies for next wave
            amtEnemiesWave += addEnemyPerWave;
            maxAliveEnemies += addMaxAlive;
            enemiesLeftToSpawn = amtEnemiesWave;
            waveStart = false;
            waveActive = true;
        }

        //Wait for player to defeat enemies
        if(waveActive == true && amtEnemiesAlive <= 0 && enemiesLeftToSpawn == 0){
            Debug.Log("-----ALL ENEMIES DEFEATED-----");
            waveActive = false;
            cooldownActive = true;
        }
 
    }

    IEnumerator cooldown(){
        Debug.Log("Cooldown() called, waiting for 15 seconds before spawning more fellas...");

        yield return new WaitForSeconds(cooldownTimer);
        
        currentWave += 1;

        cooldownActive = false;
        waveStart = true;
        this.coroutine = null;
        Debug.Log("Waited for 15 sec, resuming...");
        Debug.Log("-----WAVE "+ currentWave +"-----");
    }

    public void SpawnEnemy()
    {
        if(!(cooldownActive) && amtEnemiesAlive <= maxAliveEnemies && enemiesLeftToSpawn >= 1){
            int numOfEnemies = Random.Range(1, amtSpawnSimult);
            for (int i = 0; i < numOfEnemies; i++)
            {
                float randomX = Random.Range(-5f, 5f);
                Vector3 spawnPosition = new Vector3(randomX, enemySpawnPoint.position.y, enemySpawnPoint.position.z);
                GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, enemySpawnPoint.rotation);
                enemies.Add(spawnedEnemy); // Add the spawned enemy to the enemies list
                amtEnemiesAlive++; // Increment the currentEnemies variable
                enemiesLeftToSpawn--;
            }
        }else{
            Debug.Log("INFO: Attempting to spawn, but maxAliveEnemies is already reached, no more enemies can be spawned this wave, or a wave cooldown is active.");
        }
    }

    public void RemoveEnemy(GameObject enemyToRemove) // Gets called from the enemy when it dies
    {
        // Remove the enemy from the enemies list
        if (enemies.Contains(enemyToRemove))
        {
            enemies.Remove(enemyToRemove);
            amtEnemiesAlive--; // Decrement the currentEnemies variable

            scoreManager.giveMoney(100, false);
            scoreManager.giveScore(100);

            amtEnemiesDefeated += 1;
        }
    }

    private void DoActions()
    {
        foreach(GameObject enemy in enemies){
            // Assign actions based on the distance to the "Player" and what the other enemies are doing
        }        
    }

}