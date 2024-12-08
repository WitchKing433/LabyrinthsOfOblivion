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
    public List<GameObject> playerCharacters;
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
        
            character1Image.GetComponent<Image>().sprite = playerCharacters[0].GetComponent<Image>().sprite;
            character2Image.GetComponent<Image>().sprite = playerCharacters[1].GetComponent<Image>().sprite;
            character3Image.GetComponent<Image>().sprite = playerCharacters[2].GetComponent<Image>().sprite;
            this.gameObject.SetActive(true);
    }

    void Update()
    {
        if(owner != null)
        {
            character1Health.text = $"{playerCharacters[0].GetComponent<UnityCharacter>().daedra.Health}/{playerCharacters[0].GetComponent<UnityCharacter>().daedra.BaseHealth}";
            character2Health.text = $"{playerCharacters[1].GetComponent<UnityCharacter>().daedra.Health}/{playerCharacters[1].GetComponent<UnityCharacter>().daedra.BaseHealth}";
            character3Health.text = $"{playerCharacters[2].GetComponent<UnityCharacter>().daedra.Health}/{playerCharacters[2].GetComponent<UnityCharacter>().daedra.BaseHealth}";
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
