using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClassLibraryMazeGame;
using System.Drawing;
using JetBrains.Annotations;
using System;
using TMPro;
using System.Xml.Serialization;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Canvas canvas;
    public GameObject gameParent;
    public List<ClassCharacter> availableCharacters;
    public List<ClassCharacter> availableCharactersCopy;
    public List<GameObject> availableUnityCharacters;
    public List<GameObject> charactersPrefabs;
    public List<GameObject> trappsPrefabs;
    public GameObject deadLandWall;
    public GameObject deadLandSand;
    public GameObject player1Scroll;
    public GameObject player2Scroll;
    public GameObject player1Board;
    public GameObject player2Board;
    public List<GameObject> playersScrolls;
    public List<GameObject> player1Characters;
    public List<GameObject> player2Characters;
    public TMP_Text gameInfo;
    public enum ActionState { None, Move, Attack, Skill, PlaceBase1, PlaceBase2, GameOver, Pause};
    public static ActionState actionState = ActionState.PlaceBase1;
    public GameObject daedricTower;
    ClassMazeLogic game;
    public TMP_Text turnsCount;
    public int actualTurn = 1;
    public static int maxTurns = 30;
    public Dictionary<(int,int), GameObject> unityCells;
    AudioSource audioSource;
    public List<AudioClip> attackSounds = new List<AudioClip>();
    public List<AudioClip> trappSounds = new List<AudioClip>();
    public AudioClip buttonSound;
    public AudioClip attackBase;
    public GameObject musicManager;
    public GameObject endScene;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playersScrolls = new List<GameObject>() { player1Scroll, player2Scroll };        
        player1Characters = new List<GameObject> ();
        player2Characters = new List<GameObject> ();
        unityCells = new Dictionary<(int,int), GameObject>();
        game = Factory.game;
        game.InitMaze();        
        availableCharacters = new List<ClassCharacter>();
        CreateCharacters();
        availableCharactersCopy = new List<ClassCharacter>();
        availableCharactersCopy.AddRange(availableCharacters);
        float cellSize = deadLandWall.GetComponent<RectTransform>().rect.width;
        for (int iRow = -1; iRow <= ClassMaze.size; iRow++)
        {
            for (int iCol = -1; iCol <= ClassMaze.size; iCol++)
            {
                float x = canvas.GetComponent<RectTransform>().rect.width / 2 - ((ClassMaze.size / 2) - iCol) * cellSize;
                float y = canvas.GetComponent<RectTransform>().rect.height / 2 + ((ClassMaze.size / 2) - iRow) * cellSize;
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
                    unityCells.Add((iRow,iCol), wall);
                }
                else
                {
                    GameObject sand = Instantiate(deadLandSand, pos, Quaternion.identity, gameParent.transform);
                    sand.GetComponent<Cell>().cell = game.maze.maze[iRow, iCol];
                    unityCells.Add((iRow, iCol), sand);
                }
                game.maze.maze[iRow, iCol].X = x;
                game.maze.maze[iRow, iCol].Y = y;
                
            }
        }     
        game.AddPlayer(0, 500, 1);
        game.AddPlayer(1, 500, 1);
        game.playerList[0].opponent = game.playerList[1];
        game.playerList[1].opponent = game.playerList[0];
    }
    public void GameOver()
    {
        actionState = ActionState.GameOver;
        musicManager.GetComponent<MusicManager>().StartEndMusic();
        int winner = 0;
        if (game.playerDead)
        {
            if (game.playerList[0].selfBase.Health <= 0)
                winner = 2;
            else
                winner = 1;
        }
        else if (game.playerList[0].selfBase.Health > game.playerList[1].selfBase.Health)
        {
            winner = 1;
        }
        else if (game.playerList[0].selfBase.Health < game.playerList[1].selfBase.Health)
        {
            winner = 2;
        }
        else if (game.playerList[0].selfBase.Health == game.playerList[1].selfBase.Health)
        {
            winner = 3;
        }
        endScene.GetComponent<EndSceneManager>().SetWinner(winner);
        endScene.SetActive(true);
        gameParent.SetActive(false);
    }
    public void PassTurn()
    {
        if (actionState != ActionState.PlaceBase1 && actionState != ActionState.PlaceBase1) 
        {
            if (game.turn.id == game.playerList.Count - 1)
            {
                if (actualTurn == 30)
                {
                    GameOver();
                }
                actualTurn++;
                turnsCount.text = $"Turno:\n{actualTurn}/30";
            }
            game.turn.PassTurn();
            SelectCharacter(null);
            game.turn = game.playerList[(game.turn.id + 1) % game.playerList.Count];
            actionState = ActionState.None;
            if (game.turn.id == 0)
            {
                canvas.GetComponent<GameManager>().gameInfo.text = "Turno del primer jugador";
            }
            else
            {
                canvas.GetComponent<GameManager>().gameInfo.text = "Turno del segundo jugador";
            }
            if(game.turn.asleep > 0)
                PassTurn();
        }
    }
    public void SelectCharacter(GameObject character, bool charDead = false)
    {
        if (character != null)
        {
            if (character.GetComponent<UnityCharacter>().daedra.owner == game.turn)
            {
                game.selectedCharacter = character.GetComponent<UnityCharacter>().daedra;                
            }
            if (character.GetComponent<UnityCharacter>().daedra.owner.id == 0)
            {
                player1Scroll.GetComponent<CharacterScroll>().SetScrollCharacter(character);
                player1Scroll.SetActive(true);
            }
            else
            {
                player2Scroll.GetComponent<CharacterScroll>().SetScrollCharacter(character);
                player2Scroll.SetActive(true);
            }
        }else
        if (charDead)
            playersScrolls[game.turn.opponent.id].GetComponent<CharacterScroll>().activeCharacter = null;
        else
        {
            game.selectedCharacter = null;
            canvas.GetComponent<GameManager>().player1Scroll.GetComponent<CharacterScroll>().activeCharacter = null;
            canvas.GetComponent<GameManager>().player2Scroll.GetComponent<CharacterScroll>().activeCharacter = null;
        }
    }
    public void AttackToMe(GameObject enemy)
    {
        Vector3 pos = enemy.transform.position;
        if (game.selectedCharacter.Attack(enemy.GetComponent<UnityCharacter>().daedra))
        {
            actionState = ActionState.None;
            Instantiate(playersScrolls[game.turn.id].GetComponent<CharacterScroll>().activeCharacter.GetComponent<UnityCharacter>().RandomAnimation(),pos , Quaternion.identity, gameParent.transform);
            PlayAttackSound();
        }
    }
    public void PlayAttackSound()
    {
        audioSource.volume = 1f;
        System.Random random = new System.Random();
        audioSource.clip = attackSounds[random.Next(attackSounds.Count)];
        audioSource.Play();
    }
    public void PlayTrappSound(int id)
    {
        audioSource.volume = 1f;
        audioSource.clip = trappSounds[id];
        audioSource.Play();
    }
    public void AttackBase(GameObject cell)
    {
        if (game.selectedCharacter.AttackBuilding((ClassBase)cell.GetComponent<Cell>().cell.mazeObject))
        {
            audioSource.volume = 1f;
            actionState = ActionState.None;
            Instantiate(playersScrolls[game.turn.id].GetComponent<CharacterScroll>().activeCharacter.GetComponent<UnityCharacter>().RandomAnimation(), cell.transform.position, Quaternion.identity, gameParent.transform);
            audioSource.clip = attackBase;
            audioSource.Play();
        }
    }
    public void SuscribeToEvent()
    {
        game.playerList[0].selfBase.instantiateCharacterEvent += PlaceCharacter;
        game.playerList[1].selfBase.instantiateCharacterEvent += PlaceCharacter;
        game.playerList[0].selfBase.gameOver += GameOver;
        game.playerList[1].selfBase.gameOver += GameOver;
        for (int i = 0; i < availableCharactersCopy.Count; i++)
        {
            availableCharactersCopy[i].sendToBaseEvent += SendCharacterToBase;
        }
    }
    public void SendCharacterToBase()
    {
        for (int i = 0; i < availableUnityCharacters.Count; i++)
        {
            if (availableUnityCharacters[i].GetComponent<UnityCharacter>().daedra.Health == 0)
            {
                availableUnityCharacters[i].SetActive(false);
                actionState = ActionState.None;
                SelectCharacter(null, true);
            }
        }
    }
    public void PlaceCharacter(ClassCharacter character)
    {
        for(int i = 0; i < availableUnityCharacters.Count; i++)
        {
            if (availableUnityCharacters[i].GetComponent<UnityCharacter>().daedra == character)
            {
                availableUnityCharacters[i].transform.position = new Vector3((float)character.cell.X, (float)character.cell.Y);
                availableUnityCharacters[i].SetActive(true);                
            }
        }
    }
    public void ActivateSkill(GameObject character = null)
    {
        bool playSound;
        if (character != null)
        {
            playSound = game.selectedCharacter.ActivateSkill(character.GetComponent<UnityCharacter>().daedra);
        }
        else
            playSound = game.selectedCharacter.ActivateSkill();
        actionState = ActionState.None;
        if (playSound)
        {
            playersScrolls[game.turn.id].GetComponent<CharacterScroll>().activeCharacter.GetComponent<AudioSource>().Play();
        }
    }


    public void ShowTrapps()
    {
        for(int i = 0;i < ClassMaze.size; i++)
        {
            for(int j = 0; j < ClassMaze.size; j++)
            {
                if(unityCells[(i, j)].GetComponent<Cell>().cell.mazeObject != null && unityCells[(i, j)].GetComponent<Cell>().cell.mazeObject is ClassTrapp)
                    unityCells[(i,j)].GetComponent<Cell>().ShowTrapp();
            }
        }
    }
    public void ClickOnImage(GameObject image)
    {
        for (int i = 0; i < player1Characters.Count; i++)
        {
            if (image.GetComponent<Image>().sprite == player1Characters[i].GetComponent<Image>().sprite)
            {
                if(player1Characters[i].GetComponent<UnityCharacter>().daedra.InactiveTime <= 0)
                {
                    SelectCharacter(player1Characters[i]);
                }
                return;
            }
        }
        for (int i = 0; i < player2Characters.Count; i++)
        {
            if (image.GetComponent<Image>().sprite == player2Characters[i].GetComponent<Image>().sprite)
            {
                if (player2Characters[i].GetComponent<UnityCharacter>().daedra.InactiveTime <= 0)
                {
                    SelectCharacter(player2Characters[i]);
                }
                return;
            }
        }
    }



    public void GameStart()
    {
        musicManager.GetComponent<MusicManager>().StartCombatMusic();
        InstantiateCharacters();
        game.playerList[0].selfBase.RandomPlaceCharacters();
        game.playerList[1].selfBase.RandomPlaceCharacters();
        game.turn = game.playerList[0];
        FillBoard();
        ClassTrapp.PlaceTrapps();
        for (int i = 0; i < availableUnityCharacters.Count; i++)
        {
            availableUnityCharacters[i].transform.SetAsLastSibling();
        }
    }
    public void InstantiateCharacters()
    {
        for (int i = 0; i < charactersPrefabs.Count; i++)
        {
            GameObject character = Instantiate(charactersPrefabs[i], new Vector3(0, 0), Quaternion.identity, gameParent.transform);
            character.GetComponent<UnityCharacter>().daedra = availableCharactersCopy[i];
            if (availableCharactersCopy[i].owner.id == 0)
            {
                character.GetComponent<UnityCharacter>().scroll = player1Scroll;
                player1Characters.Add(character);
            }
            else
            {
                character.GetComponent<UnityCharacter>().scroll = player2Scroll;
                player2Characters.Add(character);
            }
        
            availableUnityCharacters.Add(character);
            character.SetActive(false);
        }
        
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
    public void FillBoard()
    {
        player1Board.GetComponent<PlayersBoard>().owner = game.playerList[0];
        player2Board.GetComponent<PlayersBoard>().owner = game.playerList[1];
        player1Board.GetComponent<PlayersBoard>().playerCharacters = player1Characters;
        player2Board.GetComponent<PlayersBoard>().playerCharacters = player2Characters;
        player1Board.GetComponent<PlayersBoard>().FillBoard();
        player2Board.GetComponent<PlayersBoard>().FillBoard();
    }
    public void ButtonSound()
    {
        audioSource.volume = 0.1f;
        audioSource.clip = buttonSound; 
        audioSource.Play();
    }
    void Update()
    {
        if (actionState != ActionState.PlaceBase1 && actionState != ActionState.PlaceBase2 && actionState != ActionState.GameOver && actionState != ActionState.Pause) 
        {            
            if (game.selectedCharacter != null)
            {                
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    playersScrolls[game.turn.id].GetComponent<CharacterScroll>().Move();
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    playersScrolls[game.turn.id].GetComponent<CharacterScroll>().Attack();
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playersScrolls[game.turn.id].GetComponent<CharacterScroll>().UseSkill();
                }
                if (actionState == ActionState.Move)
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        if (game.selectedCharacter.cell.Row - 1 >= 0)
                        {
                            GameObject cell = unityCells[(game.selectedCharacter.cell.Row - 1, game.selectedCharacter.cell.Column)];
                            cell.GetComponent<Cell>().IsClicked();
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        if (game.selectedCharacter.cell.Row + 1 < ClassMaze.size)
                        {
                            GameObject cell = unityCells[(game.selectedCharacter.cell.Row + 1, game.selectedCharacter.cell.Column)];
                            cell.GetComponent<Cell>().IsClicked();
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        if (game.selectedCharacter.cell.Column + 1 < ClassMaze.size)
                        {
                            GameObject cell = unityCells[(game.selectedCharacter.cell.Row, game.selectedCharacter.cell.Column + 1)];
                            cell.GetComponent<Cell>().IsClicked();
                            Vector3 localScale = playersScrolls[game.turn.id].GetComponent<CharacterScroll>().activeCharacter.transform.localScale;
                            localScale.x = -1;
                            playersScrolls[game.turn.id].GetComponent<CharacterScroll>().activeCharacter.transform.localScale = localScale;

                        }
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        if (game.selectedCharacter.cell.Column - 1 >= 0)
                        {
                            GameObject cell = unityCells[(game.selectedCharacter.cell.Row, game.selectedCharacter.cell.Column - 1)];
                            cell.GetComponent<Cell>().IsClicked();
                            Vector3 localScale = playersScrolls[game.turn.id].GetComponent<CharacterScroll>().activeCharacter.transform.localScale;
                            localScale.x = 1;
                            playersScrolls[game.turn.id].GetComponent<CharacterScroll>().activeCharacter.transform.localScale = localScale;
                        }
                    }
                    if (game.selectedCharacter != null && game.selectedCharacter.ValidSteps == 0)
                    {
                        actionState = ActionState.None;
                    }
                }
            }
        }
    }
}
