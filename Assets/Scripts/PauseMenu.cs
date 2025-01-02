using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pause;
    void Start()
    {
        
    }

    void Update()
    {
        if(GameManager.actionState != GameManager.ActionState.Pause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pause.SetActive(true);
                GameManager.actionState = GameManager.ActionState.Pause;
            }
        }
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void GotoMainMenu()
    {
        GameManager.actionState = GameManager.ActionState.PlaceBase1;
        SceneManager.LoadScene(0);
    }
    public void Continue()
    {
        pause.SetActive(false);
        GameManager.actionState = GameManager.ActionState.None;
    }
    public void Tutorial()
    {

    }
}
