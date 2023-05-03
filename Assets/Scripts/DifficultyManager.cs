using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private Slider difficultySlider;
    [SerializeField] private Slider gameificationSlider;

    public int difficulty = 0;
    public int gameification = 0;

    void Start(){

    }

    //Calling these functions directly from a slider seems to have broke in unity 2018.2
    public void SetDifficulty(int newVal){
        Debug.Log("Changed difficulty to " + newVal);
        difficulty = newVal;
    }

    public void SetGameification(int newVal){
        Debug.Log("Changed Gameification to " + newVal);
        gameification = newVal;
    }

    void Awake(){
        DontDestroyOnLoad(this);
        
        difficultySlider.onValueChanged.AddListener((diffval) => {
            difficulty = (int)Mathf.Round(diffval);
            Debug.Log("Changed difficulty to " + diffval);
        });

        difficultySlider.onValueChanged.AddListener((mystval) => {
            gameification = (int)Mathf.Round(mystval);
            Debug.Log("Changed Gameification to " + mystval);
        });
    }

}
