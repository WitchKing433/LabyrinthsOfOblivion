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
    void Start()
    {
        canvas = FindFirstObjectByType<Canvas>();

    }
    void Update()
    {
        
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
    }
}
