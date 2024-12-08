using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryMazeGame
{
    public class ClassTrapp : ClassMazeObject
    {
        public static int trappsCount = 5;
        static int trapps = 4;                                    //cambiar cuando se agreguen más trampas
        int id;
        
        public int Id {get { return id;}}

        public ClassTrapp(int i)
        {
            id = i;
        }
        public static void SetTrapp(ClassCell destination, int i)
        {
            Factory.game.maze.freeCells.Remove(destination);
            destination.SetMazeObject(new ClassTrapp(i));
        }

        public static void PlaceTrapps()
        {
            for (int i = 0; i < trapps; i++)
            {
                for(int j = trappsCount; j > 0; j--)
                {
                    ClassCell destination = Factory.game.maze.RandomNotOcupiedCell();
                    if(destination.character != null)
                    {
                        j++;
                        continue;
                    }
                    SetTrapp(destination, i);
                }
            }
        }
        public void ActivateTrapp(ClassCharacter character) 
        {
            switch(id)
            {
                case 0:
                    ActivateTentacleTrapp(character); 
                    break;
                case 1:
                    ActivateFireTrapp();
                    break;
                case 2:
                    ActivateOblivionGate(character);
                    break;
                case 3:
                    ActivatePoisonCharacter(character);
                    break;
            }
        }
        void ActivateTentacleTrapp(ClassCharacter character)
        {
            character.Damaged(5);
            character.DeSpeed(3);
        }
        void ActivateFireTrapp() 
        {
            if (Factory.game.maze.maze[cell.Row, cell.Column].character != null)
                Factory.game.maze.maze[cell.Row, cell.Column].character.Damaged(10);
            int[] dirRow = new int[] { 0, 1, 0, -1 };
            int[] dirCol = new int[] { 1, 0, -1, 0 };
            for(int i = 0; i < dirRow.Length; i++)
            {
                int iRow = cell.Row + dirRow[i];
                int iCol = cell.Column + dirCol[i];
                while (iRow != -1 && iRow != ClassMaze.size && iCol != -1 && iCol != ClassMaze.size && !(Factory.game.maze.maze[iRow, iCol].mazeObject is ClassWall))
                {                    
                    if (Factory.game.maze.maze[iRow, iCol].character != null)
                        Factory.game.maze.maze[iRow, iCol].character.Damaged(10);
                    iRow += dirRow[i];
                    iCol += dirCol[i];
                }
            }
        }
        void ActivateOblivionGate(ClassCharacter character)
        {            
        }
        void ActivatePoisonCharacter(ClassCharacter character)
        {
            character.poisoned += 5;
        }
    }
}
