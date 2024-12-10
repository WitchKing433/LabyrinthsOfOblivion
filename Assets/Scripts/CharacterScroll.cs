using ClassLibraryMazeGame;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScroll : MonoBehaviour
{
    public Canvas canvas;
    public GameObject activeCharacter;
    public TMP_Text characterName;
    public TMP_Text health;
    public TMP_Text power;
    public TMP_Text steps;
    public TMP_Text coolDown;
    public TMP_Text skillDuration;
    public GameObject actionsControll;
    public GameObject characterImage;
    public GameObject poison;
    UnityEngine.Color color;
    void Start()
    {
       
    }

    void Update()
    {

        if (activeCharacter != null)
        {   
            this.gameObject.SetActive(true);
            characterImage.GetComponent<Image>().sprite = activeCharacter.GetComponent<UnityCharacter>().characterLargeImage;
            characterName.text = activeCharacter.GetComponent<UnityCharacter>().daedra.Name;
            health.text = $"Health:{activeCharacter.GetComponent<UnityCharacter>().daedra.Health.ToString()}";
            power.text = $"Power:{activeCharacter.GetComponent<UnityCharacter>().daedra.Power.ToString()}";
            steps.text = $"Steps:{activeCharacter.GetComponent<UnityCharacter>().daedra.ValidSteps.ToString()}";
            coolDown.text = $"Cd:{activeCharacter.GetComponent<UnityCharacter>().daedra.CoolDown.ToString()}";
            skillDuration.text = $"Sd:{activeCharacter.GetComponent<UnityCharacter>().daedra.SkillDuration.ToString()}";
            if(activeCharacter.GetComponent<UnityCharacter>().daedra.poisoned > 0)
            {
                poison.SetActive(true);
            }
            else
            {
                poison.SetActive(false);
            }

        }
        else { this.gameObject.SetActive(false); }
    }
    public void Move()
    {
        if (GameManager.actionState != GameManager.ActionState.Move)
            GameManager.actionState = GameManager.ActionState.Move;
        else if (GameManager.actionState == GameManager.ActionState.Move)
            GameManager.actionState = GameManager.ActionState.None;
    }
    public void Attack()
    {
        if (GameManager.actionState != GameManager.ActionState.Attack)
            GameManager.actionState = GameManager.ActionState.Attack;
        else if (GameManager.actionState == GameManager.ActionState.Attack)
            GameManager.actionState = GameManager.ActionState.None;
    }
    public void UseSkill()
    {
        if (activeCharacter.GetComponent<UnityCharacter>().daedra.selectionSkill) 
        {
            if (GameManager.actionState != GameManager.ActionState.Skill)
                GameManager.actionState = GameManager.ActionState.Skill;
            else if (GameManager.actionState == GameManager.ActionState.Skill)
                GameManager.actionState = GameManager.ActionState.None;
        }
        else 
            canvas.GetComponent<GameManager>().ActivateSkill();
    }
    public void SetScrollCharacter(GameObject character)
    {
        activeCharacter = character;
        AssignColor();
        characterName.color = color;
        if(character.GetComponent<UnityCharacter>().daedra.owner == Factory.game.turn)
            actionsControll.SetActive(true);
        else
            actionsControll.SetActive(false);
    }
    public void AssignColor()
    {
        if (activeCharacter.GetComponent<UnityCharacter>().daedra.Name == "Hermaeus Mora")
            color = new UnityEngine.Color(6f / 255f, 113 / 255f, 0f);
        if (activeCharacter.GetComponent<UnityCharacter>().daedra.Name == "Vaermina")
            color = new UnityEngine.Color(64 / 255f, 0f, 113 / 255f);
        if (activeCharacter.GetComponent<UnityCharacter>().daedra.Name == "Sheogorath")
            color = new UnityEngine.Color(57 / 255f, 121 / 255f, 56 / 255f);
        if (activeCharacter.GetComponent<UnityCharacter>().daedra.Name == "Mehrunes Dagon")
            color = new UnityEngine.Color(116 / 255f, 10 / 255f, 0f);
        if (activeCharacter.GetComponent<UnityCharacter>().daedra.Name == "Peryite")
            color = new UnityEngine.Color(50 / 255f, 78 / 255f, 44 / 255f);
        if (activeCharacter.GetComponent<UnityCharacter>().daedra.Name == "Boethiah")
            color = new UnityEngine.Color(115 / 255f, 115 / 255f, 115 / 255f);
    }
}
