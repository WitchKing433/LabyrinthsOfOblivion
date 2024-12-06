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
        public bool playerDead = false;
        public void InitMaze()
        {
            maze.Init();
            playerList = new List<ClassPlayer>();
        }

        public void SelectCharacter(ClassCharacter character)
        {
            selectedCharacter = character;
        }
        public void AddPlayer(int id, int baseHealth, int baseRadius)
        {
            playerList.Add(new ClassPlayer(id, baseHealth, baseRadius));
        }
    }

}
