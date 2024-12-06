using ClassLibraryMazeGame;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayersBoard : MonoBehaviour
{
    public ClassPlayer owner;
    public Canvas canvas;
    public GameObject character1Image;
    public GameObject character2Image;
    public GameObject character3Image;
    public TMP_Text character1Health;
    public TMP_Text character2Health;
    public TMP_Text character3Health;
    public TMP_Text PlayerBaseHealth;
    public TMP_Text playerInfo1;
    public TMP_Text playerInfo2;






    void Start()
    {
        
    }
    public void FillBoard()
    {
        if (owner.id == 0)
        {
            character1Image.GetComponent<Image>().sprite = canvas.GetComponent<GameManager>().player1Characters[0].GetComponent<Image>().sprite;
            character2Image.GetComponent<Image>().sprite = canvas.GetComponent<GameManager>().player1Characters[1].GetComponent<Image>().sprite;
            character3Image.GetComponent<Image>().sprite = canvas.GetComponent<GameManager>().player1Characters[2].GetComponent<Image>().sprite;
        }
        else
        {
            character1Image.GetComponent<Image>().sprite = canvas.GetComponent<GameManager>().player2Characters[0].GetComponent<Image>().sprite;
            character2Image.GetComponent<Image>().sprite = canvas.GetComponent<GameManager>().player2Characters[1].GetComponent<Image>().sprite;
            character3Image.GetComponent<Image>().sprite = canvas.GetComponent<GameManager>().player2Characters[2].GetComponent<Image>().sprite;
        }
        this.gameObject.SetActive(true);
    }

    void Update()
    {
        if(owner != null)
        {
            character1Health.text = $"{owner.team[0].Health}/{owner.team[0].BaseHealth}";
            character2Health.text = $"{owner.team[1].Health}/{owner.team[1].BaseHealth}";
            character3Health.text = $"{owner.team[2].Health}/{owner.team[2].BaseHealth}";
            PlayerBaseHealth.text = $"{owner.selfBase.Health}/500";
            if (owner.asleep != 0)
            {
                playerInfo1.text = $"Dormido por {owner.asleep} turnos";
            }
            else
            {
                playerInfo1.text = "No se encuentra dormido";
            }
            if (owner.canActivateTrapps)
            {
                playerInfo2.text = $"Puede activar trampas";
            }
            else
            {
                playerInfo2.text = $"No puede activar trampas";
            }
        }       
    }
}
