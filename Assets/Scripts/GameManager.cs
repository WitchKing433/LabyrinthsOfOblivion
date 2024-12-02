using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClassLibraryMazeGame;
using System.Drawing;
using JetBrains.Annotations;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{
    public Canvas canvas;
    public GameObject gameParent;
    public List<ClassCharacter> availableCharacters;
    public GameObject deadLandWall;
    public GameObject deadLandSand;
    public GameObject player1Scroll;
    public GameObject player2Scroll;
    public static bool placeBaseState1;
    public static bool placeBaseState2;
    public enum ActionState { None, Move, Attack, Skill };
    public static ActionState actionState = ActionState.None;
    public GameObject daedricTower;
    ClassMazeLogic game;

    void Start()
    {
        actionState = ActionState.None;
        game = Factory.game;
        game.InitMaze();
        placeBaseState1 = true;
        placeBaseState2 = false;
        availableCharacters = new List<ClassCharacter>();
        CreateCharacters();
        float cellSize = deadLandWall.GetComponent<RectTransform>().rect.width;
        for (int iRow = -1; iRow <= ClassMaze.size; iRow++)
        {
            for (int iCol = -1; iCol <= ClassMaze.size; iCol++)
            {
                float x = canvas.GetComponent<RectTransform>().rect.width / 2 - ((ClassMaze.size / 2) - iCol) * cellSize;
                float y = canvas.GetComponent<RectTransform>().rect.height / 2 - ((ClassMaze.size / 2) - iRow) * cellSize;
                Vector3 pos = new Vector3(x, y, 10f);
                if ((iRow == -1 || iRow == ClassMaze.size || iCol == ClassMaze.size || iCol == -1))
                {
                    GameObject wall = Instantiate(deadLandWall, pos, Quaternion.identity, gameParent.transform);
                    continue;
                }
                if (!(game.maze.maze[iRow, iCol] == null) && !(game.maze.maze[iRow, iCol].mazeObject == null))
                {
                    GameObject wall = Instantiate(deadLandWall, pos, Quaternion.identity, gameParent.transform);
                    wall.GetComponent<Cell>().cell = game.maze.maze[iRow, iCol];
                }
                else
                {
                    GameObject sand = Instantiate(deadLandSand, pos, Quaternion.identity, gameParent.transform);
                    sand.GetComponent<Cell>().cell = game.maze.maze[iRow, iCol];
                }
                game.maze.maze[iRow, iCol].X = x;
                game.maze.maze[iRow, iCol].Y = y;
            }
        }     
        game.AddPlayer(0, 500, 1);
        game.AddPlayer(1, 500, 1);

    }
    void CreateCharacters()
    {
        availableCharacters.Add(new ClassCharacter("Hermaeus Mora", 100, 10, 12));
        availableCharacters.Add(new ClassCharacter("Vaermina", 100, 13, 12));
        availableCharacters.Add(new ClassCharacter("Sheogorath", 100, 20, 15));
        availableCharacters.Add(new ClassCharacter("Peryite", 100, 20, 10));
        availableCharacters.Add(new ClassCharacter("Mehrunes Dagon", 150, 5, 30));
        availableCharacters.Add(new ClassCharacter("Boethiah", 100, 20, 20));
    }
    void Update()
    {
        
    }
}
