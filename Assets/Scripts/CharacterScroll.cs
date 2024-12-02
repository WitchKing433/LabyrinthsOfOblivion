using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterScroll : MonoBehaviour
{
    public GameObject activeCharacter;
    public TMP_Text characterName;
    public TMP_Text health;
    public TMP_Text power;
    public TMP_Text steps;
    void Start()
    {
        
    }

    void Update()
    {

        if (activeCharacter != null)
        {   
            this.gameObject.SetActive(true);
            characterName.text = activeCharacter.GetComponent<UnityCharacter>().daedra.Name;
            health.text = $"Health:{activeCharacter.GetComponent<UnityCharacter>().daedra.Health.ToString()}";
            power.text = $"Power:{activeCharacter.GetComponent<UnityCharacter>().daedra.Power.ToString()}";
            steps.text = $"Steps:{activeCharacter.GetComponent<UnityCharacter>().daedra.ValidSteps.ToString()}";
            characterName.color = activeCharacter.GetComponent<UnityCharacter>().color;
        }
        else { this.gameObject.SetActive(false); }
    }
    public void Move()
    {
        GameManager.actionState = GameManager.ActionState.Move;
    }
    public void Attack()
    {
        GameManager.actionState = GameManager.ActionState.Attack;
    }
    public void UseSkill()
    {
        GameManager.actionState = GameManager.ActionState.Skill;
    }
}
