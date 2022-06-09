using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MyPauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu, MyCamera;
    bool pauseMenuActive;
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     pauseMenuActive = !pauseMenuActive;
        //     if (pauseMenuActive)
        //     {
        //         PauseMenuOpen();
        //     }
        //     if (!pauseMenuActive)
        //     {
        //         PauseMenuClose();
        //     }
        // }
    }

    // void PauseMenuOpen()
    // {
    //     pauseMenu.SetActive(true);
    //     Time.timeScale = 0;
    //     MyCamera.GetComponent<FirstPersonAIO>().enabled = false;
    // }

    // public void PauseMenuClose()
    // {
    //     pauseMenuActive = false;
    //     pauseMenu.SetActive(false);
    //     Time.timeScale = 1;
    //     MyCamera.GetComponent<FirstPersonAIO>().enabled = true;
    // }

    public void Exit()
    {
        Application.Quit();
    }
}
