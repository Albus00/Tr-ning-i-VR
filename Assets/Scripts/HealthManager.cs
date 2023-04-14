//Träning i VR
//2023-04-12
//Victor Persson


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image _bar;
    public bool _POWERUP_invulnerable = false;

    public float _healthValue = 100.0f;
    private float prevHealth = 0.0f;
    
    private float _fullHealth;
    private float _halfHealth;
    private float _thirdHealth;
    private float _quarterHealth;

    public float _flashSpeed = 3.0f;
    
    public Color startColor = Color.green;
    public Color middleColor = Color.yellow;
    public Color endColor = Color.red;
    public Color flashColorSecondary = Color.black;
    public Color invulnerableColor = Color.cyan;
    private Color lerpColor;
    private Color flashColor;
    
    //DEBUG - decrease health 
    public bool _DEBUG_decreaseHealth = false;
    public float _DEBUG_decreasePerSecond = 5.0f;

    void Start()
    {
        _fullHealth = _healthValue;
        _halfHealth = _healthValue/2.0f;
        _thirdHealth = _healthValue/3.0f;
        _quarterHealth = _healthValue/4.0f;

        _bar.color = startColor;
    }
    
    void Update()
    {
        
        if(_POWERUP_invulnerable){
            _bar.color = invulnerableColor;
        }else {
            ////DEBUG: remove health each second
            if(_DEBUG_decreaseHealth){
                _healthValue -= _DEBUG_decreasePerSecond*Time.deltaTime;
            }

            if(_healthValue != prevHealth){ //Lazy Update for performance
                if(_healthValue > _halfHealth){
                    lerpColor = Color.Lerp(startColor, middleColor, (_fullHealth - _healthValue)/_halfHealth);
                } else{
                    lerpColor = Color.Lerp(middleColor, endColor, (_halfHealth - _healthValue)/_thirdHealth);
                }

                _bar.color = lerpColor;
                float amount = (_healthValue/_fullHealth);
                _bar.fillAmount = amount;
                
                if(_healthValue < _quarterHealth){
                    flashColor = Color.Lerp(lerpColor, flashColorSecondary, Mathf.PingPong(Time.time*_flashSpeed, 1));
                    _bar.color = flashColor;
                }

                if(_healthValue < 0){
                    Debug.Log("You're fucking dead dawg");
                    //Kalla på death();
                }
            }
        }

        prevHealth = _healthValue;
    }
}
