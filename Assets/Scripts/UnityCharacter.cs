using ClassLibraryMazeGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCharacter : MonoBehaviour
{
    public Canvas canvas;
    public GameObject scroll;
    public ClassCharacter daedra;
    public Sprite characterLargeImage;
    public List<GameObject> attackAnimations;
    void Start()
    {
        canvas = FindFirstObjectByType<Canvas>();

    }
    void Update()
    {
        
    }
    public GameObject RandomAnimation()
    {
        System.Random random = new System.Random();
        return attackAnimations[random.Next(attackAnimations.Count)];
    }
    public void OnClick()
    {
        if (daedra.owner == Factory.game.turn && GameManager.actionState == GameManager.ActionState.None)
        {
            canvas.GetComponent<GameManager>().SelectCharacter(this.gameObject);
        }
        if(daedra.owner != Factory.game.turn && GameManager.actionState != GameManager.ActionState.Move)
        {
            canvas.GetComponent<GameManager>().SelectCharacter(this.gameObject);
        }        
        if(GameManager.actionState == GameManager.ActionState.Attack && daedra.owner == Factory.game.turn.opponent)
        {
            canvas.GetComponent<GameManager>().AttackToMe(this.gameObject);
        }
        if (GameManager.actionState == GameManager.ActionState.Skill)
        {
            canvas.GetComponent<GameManager>().ActivateSkill(this.gameObject);
        }

    }
}
