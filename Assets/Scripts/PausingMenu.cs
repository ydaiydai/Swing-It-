using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausingMenu : MonoBehaviour {
    public static bool GameIsPaused = false;

    Rigidbody[] rigids;
    Vector3[] velocity;
    Vector3[] angularVelocity;
    bool[] isKinematic;

    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
          
                QuitFirstMenu();

        }

        
    }

    /*
    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
    }
    */

    void QuitFirstMenu() 
    {
        pauseMenuUI.SetActive(true);
        // Time.timeScale = 0;
        //Application.LoadLevel("Menu");
        //GameIsPaused = true;
    }

}
