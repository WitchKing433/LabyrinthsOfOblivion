using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string pdfReportName;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Tutorial()
    {
        string filePath = Application.dataPath + "/" + pdfReportName;
        Application.OpenURL("file://" + filePath);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
