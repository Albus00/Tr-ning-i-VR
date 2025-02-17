using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private Slider difficultySlider;
    [SerializeField] private TextMeshProUGUI difficultyVal;
    [SerializeField] private Slider gameificationSlider;
    [SerializeField] private TextMeshProUGUI gameificationVal;

    public int difficulty = 0;
    public int gameification = 3;

    void Start(){
        gameification = 3;
    }

    //Calling these functions directly from a slider seems to have broke in unity 2018.2
    public void SetDifficulty(int newVal){
        Debug.Log("Changed difficulty to " + newVal);
        difficultyVal.text = newVal.ToString();
        difficulty = newVal;
    }

    public void SetGameification(int newVal){
        Debug.Log("Changed Gameification to " + newVal);
        gameificationVal.text = newVal.ToString();
        gameification = newVal;
    }

    void Awake(){
        DontDestroyOnLoad(this);
        
        difficultySlider.onValueChanged.AddListener((diffval) => {
            difficulty = (int)Mathf.Round(diffval);
            Debug.Log("Changed difficulty to " + diffval);
        });

        gameificationSlider.onValueChanged.AddListener((mystval) => {
            gameification = (int)Mathf.Round(mystval);
            Debug.Log("Changed Gameification to " + mystval);
        });
    }

}
