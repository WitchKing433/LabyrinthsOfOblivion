using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryMazeGame
{
    public class ClassBase:ClassMazeObject
    {
        int radius;
        List<ClassCell> influenceArea;
        ClassPlayer owner;
        int health;
        List<ClassCharacter> inactiveCharacters;
        public int Health {  get { return health; } }
        public ClassBase(ClassPlayer o, int h, int r)
        {
            owner = o;
            health = h;
            inactiveCharacters = new List<ClassCharacter>();
            radius = r;
        }
        public void Damaged(int h)
        {
            health -= h;
            if (health <= 0)
            {
                Factory.game.playerDead = true;
            } 
        }
        public void AddCharacterToBase(ClassCharacter character)
        {
            if(character.owner == owner) 
            { 
                inactiveCharacters.Add(character);
            }
        }
        public bool SetBase(ClassCell destination)
        {
            if(cell == null && destination.mazeObject == null)
            {
                influenceArea = new List<ClassCell>();
                AssignArea(destination.Row, destination.Column);
                if (influenceArea.Count < 3)                            //cambiar el número si se aumentan la cantidad de personajes
                    return false;
                destination.SetMazeObject(this);                                
                Factory.game.maze.freeCells.Remove(destination);
                return true;
            }
            return false;
        }
        void AssignArea(int row, int col)
        {
            int[] dirRow = new int[] {0,1,0,-1 };
            int[] dirCol = new int[] {1,0,-1,0 };
            for(int r = 1; r <= radius; r++)
            {
                int iRow = row-r;
                int iCol = col-r;
                bool flag = true;
                for(int i = 0; ; iRow += dirRow[i % 4],iCol += dirCol[i % 4]) 
                { 
                    if(iRow >= 0 && iRow < ClassMaze.size && iCol >= 0 && iCol < ClassMaze.size && Factory.game.maze.maze[iRow,iCol].mazeObject == null)
                    {
                        influenceArea.Add(Factory.game.maze.maze[iRow, iCol]);
                    }
                    if(flag)
                    {
                        flag = false;
                        continue;
                    }
                    if (iRow == row - r && iCol == col - r)
                        break;
                    if ((iRow == row + r && iCol == col + r) || (iRow == row + r && iCol == col - r) || (iRow == row - r && iCol == col + r))
                        i++;
                }
            }
        }
        public void RandomPlaceCharacters()
        {
            Random random = new Random();
            while(inactiveCharacters.Count != 0 && inactiveCharacters[0].InactiveTime == 0)
            {
                int r;
                do
                {
                    r = random.Next(influenceArea.Count);
                }
                while (influenceArea[r].character != null);
                influenceArea[r].SetCharacter(inactiveCharacters[0]);
                inactiveCharacters[0].ResetStats();
                inactiveCharacters.RemoveAt(0);
            }
        }
    }
}
