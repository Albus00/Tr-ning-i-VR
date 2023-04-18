using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://subscription.packtpub.com/book/game-development/9781838828424/2/ch02lvl1sec20/setting-up-our-enemyspawner-and-enemy-scripts
public class EnemySpawner : MonoBehaviour // spawns enemy with "spawnInterval" seconds inbetween
{
    public GameObject enemy;            // Enemy prefab.
    //private GameObject[] enemies;     // For later use with multiple types of enemies.
    public GameObject playerGameObject; // Player gameobject

    public int difficulty = 2;  // Game difficulty 1-3 (easy-hard). Controls how the enemies will spawn.

    public float spawnInterval = 10f;   // Time between enemies spawning. Increases every "wave".
    private float timePassed;           // Keeps track of the time that has passed. Resets every enemy spawn.
    private bool isPaused = false;      // Pauses the spawner.
    private float timeRemaining = 0;    // Keeps track of how much time is left of the pause.=

    private void Update()
    {
        // Prevent the spawner form functioning when it is paused.
        if (isPaused) 
        {
            timeRemaining -= Time.deltaTime;    // Count down until pause has passed.
            if (timeRemaining <= 0)
            {
                isPaused = false;   // Unpause the game when countdown has been reached.
            }

            return;
        }

        timePassed += Time.deltaTime;   // Updates the counter until spawn
        if(timePassed > spawnInterval) 
        {
            Instantiate(enemy); // Spawns enemy
            timePassed = 0f;    // Resets counter
        }
    }

    /* #region Description */
    // <summary>
    //   Gets called when a new wave has begun. Sets conditions for the new wave.
    // </summary>
    /* #endregion */
    public void newWave()
    {
        isPaused = true;
        spawnInterval *= 0.8f;
    }

    /* #region Description */
    // <summary>
    //   Pauses the spawner for a set amount of time
    // </summary>
    /* #endregion */
    public void pauseSpawner(float time)
    {
        isPaused = false;
        timeRemaining = time;
    }
}
