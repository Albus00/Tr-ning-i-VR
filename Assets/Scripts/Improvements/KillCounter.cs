using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillCounter : MonoBehaviour
{
    public TextMeshProUGUI killText;
    int kills;
    int lastKill;

    public GameObject killCounterScreen;

    // --------- Difficulty --------- //
    public DifficultyManager difficultyManager; //controls damage taken by enemies in close range
    private int difficulty = 0;
    private int gameification = 0;

    // Start is called before the first frame update
    void Start()
    {
        killCounterScreen = GameObject.Find("KillCounterGeometry");

        difficultyManager = GameObject.Find("DifficultyManager").GetComponent<DifficultyManager>();
        difficulty = difficultyManager.difficulty;
        gameification = difficultyManager.gameification;

        if(gameification >= 1){ //Killcounter screen active

        }else{ //No killcounterscreen for you!
            killCounterScreen.transform.GetChild(0).GetComponent<Canvas>().enabled = false;
        }

        kills = 0;
        lastKill = 0;

        killText.text = kills.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(kills != lastKill){
            ShowKills();
        }

        lastKill = kills;
    }

    private void ShowKills()
    {
        killText.text = kills.ToString();
    }

    public void AddKill()
    {
        kills++;
    }
}
