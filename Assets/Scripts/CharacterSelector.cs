using ClassLibraryMazeGame;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public Canvas canvas;
    public List<ClassCharacter> availableCharacters;
    public List<GameObject> availableCharactersPreview;
    public ClassCharacter characterToShow;
    public TMP_Text characterName;
    public TMP_Text daedricName;
    public TMP_Text health;
    public TMP_Text power;
    public TMP_Text steps;
    public TMP_Text coolDown;
    public TMP_Text skillDuration;
    public TMP_Text daedraDescription;
    public TMP_Text skillDescription;
    public TMP_Text player1Count;
    public TMP_Text player2Count;
    public GameObject rick;
    int index = 0;
    void Start()
    {
        
    }


    void Update()
    {
        availableCharacters = canvas.GetComponent<GameManager>().availableCharacters;
        if (availableCharacters != null && availableCharacters.Count != 0)
        {
            daedricName.text = availableCharacters[index].Name;
            characterName.text = availableCharacters[index].Name;
            health.text = $"Vida: {availableCharacters[index].Health}";
            power.text = $"Daño de ataque: {availableCharacters[index].Power}";
            steps.text = $"Velocidad: {availableCharacters[index].Steps}";
            coolDown.text = $"Cooldown: {SetCooldown()}";
            skillDuration.text = $"Duración:   {SetSkillDuration()}";
            daedraDescription.text = SetDaedraDescription();
            skillDescription.text = SetSkillDescription();
            player1Count.text = $"Cantidad de personajes: {Factory.game.playerList[0].team.Count}/3";
            player2Count.text = $"Cantidad de personajes: {Factory.game.playerList[1].team.Count}/3";
        }
        else 
        { 
            rick.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
    public void AddCharacterToTeam1()
    {
        if (Factory.game.playerList[0].team.Count < 3)
        {
            Factory.game.playerList[0].AddCharacterToTeam(availableCharacters[index]);
            availableCharacters.RemoveAt(index);
            availableCharactersPreview[index].SetActive(false);
            availableCharactersPreview.RemoveAt(index);
            if (index >= availableCharacters.Count)
                index = availableCharacters.Count - 1;
            if (availableCharactersPreview.Count > 0)
                availableCharactersPreview[index].SetActive(true);
        }
    }
    public void AddCharacterToTeam2()
    {
        if (Factory.game.playerList[1].team.Count < 3)
        {
            Factory.game.playerList[1].AddCharacterToTeam(availableCharacters[index]);
            availableCharacters.RemoveAt(index);
            availableCharactersPreview[index].SetActive(false);
            availableCharactersPreview.RemoveAt(index);
            if (index >= availableCharacters.Count)
                index = availableCharacters.Count - 1;
            if (availableCharactersPreview.Count > 0)
                availableCharactersPreview[index].SetActive(true);
        }
    }
    public void PrevCharacter()
    {
        availableCharactersPreview[index].SetActive(false);
        index--;
        if(index < 0)
            index = canvas.GetComponent<GameManager>().availableCharacters.Count - 1;
        availableCharactersPreview[index].SetActive(true);
    }
    public void NextCharacter() 
    {
        availableCharactersPreview[index].SetActive(false);
        index = (index + 1) % canvas.GetComponent<GameManager>().availableCharacters.Count;
        availableCharactersPreview[index].SetActive(true);
    }
    string SetCooldown()
    {
        string n = availableCharacters[index].Name;
        if (n == "Hermaeus Mora")
            return "5";
        else if (n == "Vaermina")
            return "7";
        else if (n == "Sheogorath")
            return "7";
        else if (n == "Mehrunes Dagon")
            return "5";
        else if (n == "Boethiah")
            return "2";
        else
            return "7";
    }
    string SetSkillDuration()
    {
        string n = availableCharacters[index].Name;
        if (n == "Hermaeus Mora")
            return "3";
        else if (n == "Vaermina")
            return "2";
        else if (n == "Sheogorath")
            return " ";
        else if (n == "Mehrunes Dagon")
            return "3";
        else if (n == "Boethiah")
            return " ";
        else
            return " ";
    }
    string SetDaedraDescription()
    {
        string n = availableCharacters[index].Name;
        if (n == "Hermaeus Mora")
            return "Príncipe Daédrico del destino, el conocimiento y la memoria.";
        else if (n == "Vaermina")
            return "Príncipe Daédrico de los sueños y las pesadillas.";
        else if (n == "Sheogorath")
            return "Príncipe Daédrico de la locura.";
        else if (n == "Mehrunes Dagon")
            return "Príncipe Daédrico de la destrucción, la energía y la ambición mortal";
        else if (n == "Boethiah")
            return "Príncipe Daédrico del engaño, las conspiraciones, el secretismo y la traición";
        else
            return "Príncipe Daédrico de las enfermedades y la pestilencia";
    }
    string SetSkillDescription()
    {
        string n = availableCharacters[index].Name;
        if (n == "Hermaeus Mora")
            return "Hace uso de sus conocimientos ocultos para impedir que los miembros de su equipo activen las trampas.";
        else if (n == "Vaermina")
            return "Duerme al oponente haciendo que se pasen sus turnos";
        else if (n == "Sheogorath")
            return "Aleatoriza las estadísticas de todos los personajes rivales en juego";
        else if (n == "Mehrunes Dagon")
            return "Aumenta el daño que causa en una cantidad igual a la vida perdida";
        else if (n == "Boethiah")
            return "Elimina a un aliado con una cantidad de vida menor o igual a 25 y aumenta su poder de ataque en 10";
        else
            return "Coloca 5 trampas de veneno en lugares aleatorios del mapa";
    }

}
