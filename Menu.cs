using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }
    public void GoToStartMenu()
    {
        GameManager.Instance.GoToStartMenu();
    }

    public void GoToLoadScreen()
    {
        GameManager.Instance.GoToLoadScreen();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
