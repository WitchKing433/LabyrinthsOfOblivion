using ClassLibraryMazeGame;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Canvas canvas;
    public ClassCell cell;
    public GameObject daedricTower;

    void Start()
    {
        canvas = FindFirstObjectByType<Canvas>();
           
    }

    void Update()
    {
        
    }
    public void IsClicked()
    {
        if(!(cell.mazeObject is ClassWall))
        {
            if (GameManager.actionState == GameManager.ActionState.PlaceBase1)
            {
                if (Factory.game.playerList[0].selfBase.SetBase(cell))
                {
                    SetBase(cell.X, cell.Y,cell,0);
                    GameManager.actionState = GameManager.ActionState.PlaceBase2;
                }
            }
            else if(GameManager.actionState == GameManager.ActionState.PlaceBase2)
            {
                if (Factory.game.playerList[1].selfBase.SetBase(cell))
                {
                    SetBase(cell.X, cell.Y, cell, 1);
                    GameManager.actionState = GameManager.ActionState.None;
                    canvas.GetComponent<GameManager>().GameStart();
                }
            }
            else if (GameManager.actionState == GameManager.ActionState.Move)
            {
                if (Factory.game.selectedCharacter.MoveTo(cell))
                {
                    if(Factory.game.turn.id == 0)
                    {
                        canvas.GetComponent<GameManager>().player1Scroll.GetComponent<CharacterScroll>().activeCharacter.transform.position = this.gameObject.transform.position;
                    }
                    else if (Factory.game.turn.id == 1)
                    {
                        canvas.GetComponent<GameManager>().player2Scroll.GetComponent<CharacterScroll>().activeCharacter.transform.position = this.gameObject.transform.position;
                    }
                }
            }
            else if (GameManager.actionState == GameManager.ActionState.Skill)
            {

            }
            else if (GameManager.actionState == GameManager.ActionState.None)
            {
                canvas.GetComponent<GameManager>().SelectCharacter(null);
                
            }





        }
    }
    public void SetBase(float x, float y, ClassCell destination, int p)
    {
        Vector3 pos = new Vector3(x, y, 0);
        GameObject tower = Instantiate(daedricTower, pos, Quaternion.identity, Object.FindFirstObjectByType<Canvas>().transform);
        tower.GetComponent<Cell>().cell = destination;
        Factory.game.playerList[p].selfBase.SetBase(destination);
    }
}
