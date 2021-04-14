using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string currentScene;

    //Loads scene when called
    public void PlayGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    //Loads scene when called
    public void Menu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    //Loads scene when called
    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    //Loads scene when called
    public void Level1()
    {
        SceneManager.LoadScene("Level1");
    }

    //Loads scene when called
    public void Level2()
    {
        SceneManager.LoadScene("Level2");
    }

    //Loads scene when called
    public void Credits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    //Quits game when called
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
