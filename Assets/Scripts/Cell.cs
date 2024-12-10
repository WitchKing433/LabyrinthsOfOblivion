using ClassLibraryMazeGame;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
                    canvas.GetComponent<GameManager>().gameInfo.text = "Coloque la base del segundo jugador";
                }
            }
            else if(GameManager.actionState == GameManager.ActionState.PlaceBase2)
            {
                if (Factory.game.playerList[1].selfBase.SetBase(cell))
                {
                    SetBase(cell.X, cell.Y, cell, 1);
                    GameManager.actionState = GameManager.ActionState.None;
                    canvas.GetComponent<GameManager>().GameStart();
                    canvas.GetComponent<GameManager>().gameInfo.text = "Turno del primer jugador";
                }
            }
            else if (GameManager.actionState == GameManager.ActionState.Move)
            {
                int movement = Factory.game.selectedCharacter.MoveTo(cell);
                if (movement != 0)
                {
                    if(Factory.game.turn.id == 0)
                    {
                        canvas.GetComponent<GameManager>().player1Scroll.GetComponent<CharacterScroll>().activeCharacter.transform.position = this.gameObject.transform.position;
                    }
                    else if (Factory.game.turn.id == 1)
                    {
                        canvas.GetComponent<GameManager>().player2Scroll.GetComponent<CharacterScroll>().activeCharacter.transform.position = this.gameObject.transform.position;
                    }
                    if(movement == 2)
                    {
                        ((ClassTrapp)cell.mazeObject).ActivateTrapp(cell.character);
                        if (((ClassTrapp)cell.mazeObject).Id == 2)
                        {
                            if (Factory.game.turn.id == 0)
                            {
                                ClassCell dest = Factory.game.maze.RandomNotOcupiedCell();
                                canvas.GetComponent<GameManager>().player1Scroll.GetComponent<CharacterScroll>().activeCharacter.transform.position = new Vector3(dest.X, dest.Y);
                                canvas.GetComponent<GameManager>().player1Scroll.GetComponent<CharacterScroll>().activeCharacter.GetComponent<UnityCharacter>().daedra.Teleport(dest);
                            }
                            if(Factory.game.turn.id == 1)
                            {
                                ClassCell dest = Factory.game.maze.RandomNotOcupiedCell();
                                canvas.GetComponent<GameManager>().player2Scroll.GetComponent<CharacterScroll>().activeCharacter.transform.position = new Vector3(dest.X, dest.Y);
                                canvas.GetComponent<GameManager>().player2Scroll.GetComponent<CharacterScroll>().activeCharacter.GetComponent<UnityCharacter>().daedra.Teleport(dest);
                            }
                        }
                    }
                }
            }
            else if (GameManager.actionState == GameManager.ActionState.Skill)
            {

            }
            else if (GameManager.actionState == GameManager.ActionState.Attack)
            {
                if (cell.mazeObject is ClassBase && ((ClassBase)cell.mazeObject).owner != Factory.game.selectedCharacter.owner)
                {
                    canvas.GetComponent<GameManager>().AttackBase(this.gameObject);
                }
            }
            else if (GameManager.actionState == GameManager.ActionState.None)
            {
                canvas.GetComponent<GameManager>().SelectCharacter(null);
                
            }





        }
    }
    public void TpToMe()
    {
        if (Factory.game.turn.id == 0)
        {
            canvas.GetComponent<GameManager>().player1Scroll.GetComponent<CharacterScroll>().activeCharacter.transform.position = this.gameObject.transform.position;
        }
        else if (Factory.game.turn.id == 1)
        {
            canvas.GetComponent<GameManager>().player2Scroll.GetComponent<CharacterScroll>().activeCharacter.transform.position = this.gameObject.transform.position;
        }
    }
    public void ShowTrapp()
    {
        switch (((ClassTrapp)cell.mazeObject).Id)
        {
            case 0:
                Instantiate(canvas.GetComponent<GameManager>().trappsPrefabs[0], new Vector3(cell.X, cell.Y), Quaternion.identity, canvas.GetComponent<GameManager>().gameParent.transform);
                break;
            case 1:
                Instantiate(canvas.GetComponent<GameManager>().trappsPrefabs[1], new Vector3(cell.X, cell.Y), Quaternion.identity, canvas.GetComponent<GameManager>().gameParent.transform);
                break;
            case 2:
                Instantiate(canvas.GetComponent<GameManager>().trappsPrefabs[2], new Vector3(cell.X, cell.Y), Quaternion.identity, canvas.GetComponent<GameManager>().gameParent.transform);
                break;
            case 3:
                Instantiate(canvas.GetComponent<GameManager>().trappsPrefabs[3], new Vector3(cell.X, cell.Y), Quaternion.identity, canvas.GetComponent<GameManager>().gameParent.transform);
                break;
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
