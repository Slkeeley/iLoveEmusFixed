using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void startGame()
    {
        Debug.Log("startingGame");
        //start game
    }

    public void toCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void backToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }
    public void quitGame()
    {
        Debug.Log("attmepting to quit game");
        Application.Quit();
    }
}
