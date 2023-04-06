using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://subscription.packtpub.com/book/game-development/9781838828424/2/ch02lvl1sec20/setting-up-our-enemyspawner-and-enemy-scripts
public class EnemySpawner : MonoBehaviour // spawns enemy with "spawnInterval" seconds inbetween
{
    public GameObject enemy;
    //private GameObject[] enemies;
    public float spawnInterval = 5f;
    private float timePassed;
    
  


    private void Update()
    {
        timePassed += Time.deltaTime; // counts up to 5
        if(timePassed > spawnInterval) 
        {
            Instantiate(enemy); // spawns enemy
            timePassed = 0f; // resets counter
        }
        

    }

   





}
