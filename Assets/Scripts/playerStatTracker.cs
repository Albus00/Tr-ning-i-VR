using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStatTracker : MonoBehaviour
{
    public HealthManager playerHealth;
    public moneyAndScoreManager playerScoreMoney;
    public EnemyHandler enemyStats;
    
    private int amt_vals = 300; //How many values to allocate for each array?

    private int pollRate = 1;
    //1 = Track stats every second
    //3 = Track stats every 3 seconds.

    private int i;
    public float[] currentHealth;
    public int[] currentScore;
    public int[] enemiesDefeated;
    public int[] currentMoney;
    public bool[] isCooldown;
    //public int[] currentPulse;
    public int[] playerMovement;

    public bool isntDead;
    
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindWithTag("HealthHandler").GetComponent<HealthManager>();
        playerScoreMoney = GameObject.Find("ScoreManager").GetComponent<moneyAndScoreManager>();
        enemyStats = GameObject.FindWithTag("EnemyHandler").GetComponent<EnemyHandler>();
        //playerPulse = GameObject.FindWithTag("PulsePoller").GetComponent<PulseScript>();
        //playerMotion = GameObject.FindWithTag("MotionHandler").GetComponent<MotionScript>();

        //Allocate room for all the stats
        currentHealth = new float[amt_vals];
        currentScore = new int[amt_vals];
        enemiesDefeated = new int[amt_vals];
        currentMoney = new int[amt_vals];
        isCooldown = new bool[amt_vals];
        //currentPulse = new int[amt_vals]
        playerMovement = new int[amt_vals];

        i = 0;
        isntDead = true;
        
        StartCoroutine(captureData());

        DontDestroyOnLoad(this);
    }

    IEnumerator captureData(){
        //Captures data every "pollRate" seconds.
        //Debug.Log("Capture data called. isntDead is currently: "+isntDead);

        while(isntDead){
            //Debug.Log("Logging data... Current index = [" + i +"]");

            currentHealth[i] = playerHealth._healthValue;
            currentScore[i] = playerScoreMoney.currentScore;
            currentMoney[i] = playerScoreMoney.currentMoney;
            enemiesDefeated[i] = enemyStats.amtEnemiesDefeated;
            isCooldown[i] = enemyStats.cooldownActive;
            //currentPulse[i] = playerPulse.currentPulse;
            //currentAvgMotion[i] = playerMotion.currentAvgMotion;

            i++;
            yield return new WaitForSeconds(pollRate);
        }
    }
}
