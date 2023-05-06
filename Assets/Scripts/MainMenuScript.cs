using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

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
    }

    public void playGame(){
        SceneManager.LoadScene(sceneName:"testingGrounds 1");
    }
}
