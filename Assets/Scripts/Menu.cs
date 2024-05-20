using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Canvas pauseMenu;

    void Start()
    {
        pauseMenu.gameObject.SetActive(false);
    }

    public void handlePauseMenu()
    {
        pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
        Cursor.lockState = pauseMenu.gameObject.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = pauseMenu.gameObject.activeSelf ? 0 : 1;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StopGame()
    {
        Application.Quit();
    }

     void Update()
    {
        if(pauseMenu != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                handlePauseMenu();
            }
        }
    }
}
