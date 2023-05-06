using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public DifficultyManager difficultyManager;
    [SerializeField] TextMeshProUGUI difficultyText;
    [SerializeField] TextMeshProUGUI gameificationText;

    public AudioClip announcerChitChat;
    public AudioSource speaker;
    public float volume = 0.9f;
    // Start is called before the first frame update
    void Start()
    {
        speaker.PlayOneShot(announcerChitChat);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("r")){
            SceneManager.LoadScene(sceneName:"testingGrounds 1");
        }

        gameificationText.text = (difficultyManager.gameification + 1).ToString();

        switch(difficultyManager.difficulty)
        {
            case 0:
                difficultyText.text = "Easy";
                break;
            case 1:
                difficultyText.text = "Normal";
                break;
            case 2:
                difficultyText.text = "Hard";
                break;
            case 3:
                difficultyText.text = "Difficult";
                break;
        }
    }

    public void playGame(){
        SceneManager.LoadScene(sceneName:"testingGrounds 1");
    }
}
