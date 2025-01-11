using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pause;
    public GameObject itemInfo;
    public string pdfReportName;
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
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Continue();
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
        itemInfo.SetActive(false);
        pause.SetActive(false);
        GameManager.actionState = GameManager.ActionState.None;
    }
    public void Tutorial()
    {
        string filePath = Application.dataPath + "/" + pdfReportName;
        Application.OpenURL("file://" + filePath);
    }
}
