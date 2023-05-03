//Träning i VR
//2023-04-12
//Victor Persson


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HealthManager : MonoBehaviour
{
    public Image _bar;
    public bool _POWERUP_invulnerable = false;

    public float _healthValue = 10.0f;
    private float prevHealth = 0.0f;
    
    private float _fullHealth;
    private float _halfHealth;
    private float _thirdHealth;
    private float _quarterHealth;

    private bool playerIsntDead = true;

    public float _flashSpeed = 3.0f;
    
    [SerializeField]
    private Color startColor = Color.green;
    private Color middleColor = Color.yellow;
    private Color endColor = Color.red;
    private Color flashColorSecondary = Color.black;
    private Color invulnerableColor = Color.cyan;
    private Color lerpColor;
    private Color flashColor;
    
    //DEBUG - decrease health 
    public bool _DEBUG_decreaseHealth = false;
    public float _DEBUG_decreasePerSecond = 5.0f;

    // --------- Difficulty --------- //
    public DifficultyManager difficultyManager; //if gameification isnt == 3 (maxed), there's no score screen
    private int difficulty = 0;
    private int gameification = 0;
    private float enemyDamage = 0;

    void Start()
    {
        _fullHealth = _healthValue;
        _halfHealth = _healthValue/2.0f;
        _thirdHealth = _healthValue/3.0f;
        _quarterHealth = _healthValue/4.0f;

        _bar.color = startColor;

        difficultyManager = GameObject.Find("DifficultyManager").GetComponent<DifficultyManager>();
        difficulty = difficultyManager.difficulty;
        gameification = difficultyManager.gameification;
    }
    
    void Update()
    {
        if(_POWERUP_invulnerable){
            _bar.color = invulnerableColor;
        }else {
            if(_healthValue != prevHealth){ //Lazy Update for performance
                if(_healthValue > _halfHealth){
                    lerpColor = Color.Lerp(startColor, middleColor, (_fullHealth - _healthValue)/_halfHealth);
                } else{
                    lerpColor = Color.Lerp(middleColor, endColor, (_halfHealth - _healthValue)/_thirdHealth);
                }

                _bar.color = lerpColor;
                float amount = (_healthValue/_fullHealth);
                _bar.fillAmount = amount;

                Debug.Log("Health at: "+_healthValue);
            }
        }

        if(_healthValue < _quarterHealth){
            flashColor = Color.Lerp(lerpColor, flashColorSecondary, Mathf.PingPong(Time.time*_flashSpeed, 1));
            _bar.color = flashColor;
        }
        
        if(_healthValue < 0 && playerIsntDead){
            playerIsntDead = false;
            Debug.Log("You're dead!");
            if(gameification == 3){
                SceneManager.LoadScene(sceneName:"GameOver");
            }else{
                Destroy(GameObject.Find("DifficultyManager"));
                SceneManager.LoadScene(sceneName:"MainMenu");
            }
        }

        prevHealth = _healthValue;
    }

    public void restoreFullHealth(){
        _healthValue = _fullHealth;
    }

    public void subtractHealth(float dmg){
        _healthValue -= dmg;
    }

    public void addHealth(float heal){
        _healthValue += heal;
    }

}
