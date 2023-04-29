//TRÄNING I VR
//Klas Nordquist
//Victor Persson


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHandler : MonoBehaviour
{
    public moneyAndScoreManager scoreManager;
    public AudioLowPassFilter musicLP;
    public AudioHighPassFilter musicHP;
    public AudioSource musicSource;
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;
    public static List<GameObject> enemies = new List<GameObject>();

    public TMPro.TextMeshProUGUI waveText;

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

    public bool cooldownActive = true;

    private IEnumerator coroutine = null; //Dont initalize cooldown more than once



    void Start()
    {
        //First wave only
        enemiesLeftToSpawn = amtEnemiesWave;
        musicLP = GameObject.FindWithTag("music").GetComponent<AudioLowPassFilter>();
        musicHP = GameObject.FindWithTag("music").GetComponent<AudioHighPassFilter>();
        musicSource = GameObject.FindWithTag("music").GetComponent<AudioSource>();
        waveText = GameObject.FindWithTag("shopWaveNumber").GetComponent<TextMeshProUGUI>();

        waveText.text = currentWave.ToString();

        musicSource.spatialBlend = 0.0f;
        musicSource.reverbZoneMix = 0.0f;
        musicLP.cutoffFrequency = 22000.0f;
        musicHP.cutoffFrequency = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //-----COOLDOWN-----//
        if(cooldownActive && this.coroutine == null){
            if(currentWave > 0){
                scoreManager.giveMoney(moneyPerWave, false);
                scoreManager.giveScore(scorePerWave);
            }
            this.coroutine = cooldown();
            StartCoroutine(this.coroutine);

            StartCoroutine(muffleMusic(2.0f)); //Muffle music linearly over five seconds
        }

        //-----WAVE START-----//
        if(waveStart == true && waveActive == false){
            //Calculate amount of enemies for next wave
            amtEnemiesWave += addEnemyPerWave;
            maxAliveEnemies += addMaxAlive;
            enemiesLeftToSpawn = amtEnemiesWave;

            StartCoroutine(unMuffleMusic(2.0f)); //Unmuffle music linearly over 2 seconds
            waveText.text = currentWave.ToString();

            waveStart = false;
            waveActive = true;
        }

        //-----WAVE ACTIVE-----//
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

    IEnumerator muffleMusic(float seconds){
        musicSource.bypassReverbZones = false;

        while(musicLP.cutoffFrequency >= 3000.0f){
            musicLP.cutoffFrequency -= (19000.0f)/seconds * Time.deltaTime;
            musicHP.cutoffFrequency += (750.0f)/seconds * Time.deltaTime;
            musicSource.reverbZoneMix += 0.5f/seconds * Time.deltaTime;
            musicSource.spatialBlend += 0.7f/seconds * Time.deltaTime;

            yield return null; //wait for next frame
        }
    }

    IEnumerator unMuffleMusic(float seconds){
        while(musicLP.cutoffFrequency <= 22000.0f && musicHP.cutoffFrequency > 0.0f){
            musicLP.cutoffFrequency += (19000.0f)/seconds * Time.deltaTime;
            musicHP.cutoffFrequency -= (750.0f)/seconds * Time.deltaTime;
            musicSource.reverbZoneMix -= 0.5f/seconds * Time.deltaTime;
            musicSource.spatialBlend -= 0.7f/seconds * Time.deltaTime;

            yield return null; //wait for next frame
        }
        
        musicSource.bypassReverbZones = true;
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