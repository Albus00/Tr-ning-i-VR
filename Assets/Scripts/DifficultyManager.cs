using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private Slider difficultySlider;
    [SerializeField] private Slider gameificationSlider;

    public int difficulty = 1;
    public int gameification = 3;

    void Start(){
        gameification = 3;
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
