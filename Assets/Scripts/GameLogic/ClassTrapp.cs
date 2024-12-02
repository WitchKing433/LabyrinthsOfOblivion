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
        int trapps = 4;                                    //cambiar cuando se agreguen más trampas
        int id;

        public ClassTrapp(int i)
        {
            id = i;
        }
        public static void SetTrapp(ClassCell destination, int i)
        {
            Factory.game.maze.freeCells.Remove(destination);
            destination.SetMazeObject(new ClassTrapp(i));
        }

        public void PlaceTrapps()
        {
            for (int i = 0; i < trapps; i++)
            {
                for(int j = trappsCount; j > 0; j--)
                {
                    ClassCell destination = Factory.game.maze.RandomNotOcupiedCell();
                    SetTrapp(destination, i);
                }
            }
        }
        public void ActivateTrapp(ClassCharacter character) 
        {
            ShowTrapp();
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
        public void ShowTrapp()
        {

        }
        void ActivateTentacleTrapp(ClassCharacter character)
        {
            character.Damaged(5);
            character.DeSpeed(3);
        }
        void ActivateFireTrapp() 
        {
            int[] dirRow = new int[] { 0, 1, 0, -1 };
            int[] dirCol = new int[] { 1, 0, -1, 0 };
            for(int i = 0; i < dirRow.Length; i++)
            {
                int iRow = cell.Row;
                int iCol = cell.Column;
                while (iRow != -1 && iRow != ClassMaze.size && iCol != -1 && iCol != ClassMaze.size && !(Factory.game.maze.maze[iRow, iCol].mazeObject is ClassWall))
                {
                    if (Factory.game.maze.maze[iRow, iCol].character != null)
                        Factory.game.maze.maze[iRow, iCol].character.Damaged(10);
                }
            }
        }
        void ActivateOblivionGate(ClassCharacter character)
        {         
            character.Teleport(Factory.game.maze.RandomNotOcupiedCell());
        }
        void ActivatePoisonCharacter(ClassCharacter character)
        {
            character.poisoned += 5;
        }
    }
}
