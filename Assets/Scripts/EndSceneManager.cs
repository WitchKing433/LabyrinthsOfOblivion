using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    public GameObject credits;
    public GameObject winner;
    public GameObject winnerText;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void SetWinner(int i)
    {
        if (i != 3)
        {
            winner.GetComponent<TMP_Text>().text = i.ToString();
        }
        else
        {
            winner.SetActive(false);
            winnerText.GetComponent<TMP_Text>().text = "Felicidades \r\na ambos!";
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    public void Credits()
    {
        this.gameObject.SetActive(false);
        credits.SetActive(true);
    }
}
