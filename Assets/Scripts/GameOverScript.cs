using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("r")){ //Load the next scene and destroy last game data
            if(GameObject.Find("playerStatTracker") != null){ Destroy(GameObject.Find("playerStatTracker")); }
            SceneManager.LoadScene(sceneName:"MainMenu");
        }
    }
}
