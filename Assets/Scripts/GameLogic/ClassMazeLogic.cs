using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryMazeGame
{
    public static class Factory
    {
        public static ClassMazeLogic game = new ClassMazeLogic();
    }
    public class ClassMazeLogic
    {
        public ClassMaze maze = new ClassMaze();
        public List<ClassPlayer> playerList;
        public ClassPlayer turn;
        public ClassCharacter selectedCharacter;
        public ClassCharacter target;
        public ClassCell dest;
        public static int maxTurns = 30;
        public int actualTurn;
        bool isAttacking = false;
        bool isMoving = false;
        bool isUsingSkill = false;
        bool isPassingPlayerTurn = false;
        public bool playerDead = false;
        public void InitMaze()
        {
            maze.Init();
            playerList = new List<ClassPlayer>();
        }

        public void ClearMaze()
        {
            //maze.Clear();
        }
        public void CallAttack()
        {
            isAttacking = true;
        }
        void Attack(ClassCharacter enemy)
        {
            if(selectedCharacter.canAttack && selectedCharacter.owner == turn && enemy.owner != selectedCharacter.owner)
            {
                selectedCharacter.Attack(enemy);
            }
        }
        public void CallUseSkill()
        {
            isUsingSkill = true;
        }
        void UseSkill(ClassCharacter enemy)
        {
            if (selectedCharacter.owner == turn && enemy.owner != selectedCharacter.owner)
            {
                selectedCharacter.ActivateSkill(enemy);
            }
        }
        public void CallMoveTo()
        {
            isMoving = true;
        }
        void MoveTo(ClassCell destination)
        {
            if(selectedCharacter.owner == turn)
            {
                selectedCharacter.MoveTo(destination);
            }
        }
        public void SelectCharacter(ClassCharacter character)
        {
            selectedCharacter = character;
        }
        public void AddPlayer(int id, int baseHealth, int baseRadius)
        {
            playerList.Add(new ClassPlayer(id, baseHealth, baseRadius));
        }
        public void PassPlayerTurn()
        {
            if (!isPassingPlayerTurn) 
            {
                turn.PassTurn();
                turn = playerList[(turn.id + 1) % playerList.Count];
                isPassingPlayerTurn = true;

            }
        }
        public void GameOver()
        {

        }
        public void GameStart()
        {
            turn = playerList[0];
            selectedCharacter = turn.team[0];
            for (int i = 1; i <= maxTurns && !playerDead; i++)
            {
                actualTurn = i;
                while(!playerDead)
                {
                    if (turn.asleep != 0)
                    {
                        PassPlayerTurn();
                    }
                    if (isAttacking)
                    {
                        Attack(target);
                        isAttacking = false;
                    }
                    if (isMoving)
                    {
                        MoveTo(dest);
                        isMoving = false;
                    }
                    if (isUsingSkill)
                    {
                        UseSkill(target);
                        isUsingSkill = false;
                    }
                    if(isPassingPlayerTurn && turn == playerList[0])
                    {                       
                        break;
                    }
                    isPassingPlayerTurn = false;
                }
            }
            GameOver();
        }
    }

}
