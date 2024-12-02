using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClassLibraryMazeGame
{
    public class ClassMaze
    {
        public int emptyCells = 0;
        public int roads = 0;
        public static int size = 31;
        public ClassCell[,] maze = new ClassCell[size,size];
        public bool[,] boolMask = new bool[size,size];
        public List<ClassCell> freeCells;
        public void Init() 
        {
            for (int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    maze[i,j] = new ClassCell(i,j);
                }
            }
            Random random = new Random();
            int initRow, initCol;
            initRow = random.Next(size);
            initCol = random.Next(size);
            CreateRoad();
            VerifyMaze(0,0);
            while (roads != emptyCells)
            {
                roads = 0;
                boolMask = new bool[size, size];
                maze = new ClassCell[size, size];
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        maze[i, j] = new ClassCell(i, j);
                    }
                }
                CreateRoad();
                VerifyMaze(0, 0);
            }

        }

        /*public void Clear()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (maze[i, j] != null)
                    {
                        if (maze[i, j].mazeObject != null)
                        {
                            if (maze[i, j].mazeObject.cell != null)
                            {
                                maze[i, j].mazeObject.cell = null;
                            }
                            maze[i, j].mazeObject = null;
                        }
                        maze[i, j] = null;
                    }
                }
            }
        }
        */
        public void CreateRoad()
        {
            freeCells = new List<ClassCell>();
            emptyCells = 0;
            Random random = new Random();
            for(int iCol = 1;iCol < size;iCol+=2)
            {               
                for (int freeSpaces = random.Next((int)((2.0 / 10) * size), (int)((5.0 / 10) * size));
                    freeSpaces != 0; freeSpaces--)
                {
                    int iRow;
                    do
                    {
                        iRow = random.Next(0, size);
                    }
                    while (maze[iRow, iCol].mazeObject == null);   
                    CleanCell(iRow, iCol);
                }
            }
            for (int iCol = 0; iCol < size; iCol += 2)
            {
                bool walleable = false;
                bool walleable2 = false;
                bool walleable1 = false;

                for (int iRow = 0; iRow < size; iRow++)
                {

                    if (iRow != size - 1 && walleable && (random.Next(2) == 0 ? true : false) && (iCol + 1 == size || !(maze[iRow, iCol + 1].mazeObject == null)) && (iCol - 1 == -1 || !(maze[iRow, iCol - 1].mazeObject == null)))
                    {
                        walleable2 = false;
                        walleable1 = false;
                    }
                    else 
                    {
                        CleanCell(iRow, iCol);
                    }

                    if (iCol + 1 == size || maze[iRow, iCol + 1].mazeObject == null)
                        walleable2 = true;
                    if (iCol - 1 == -1 || maze[iRow, iCol - 1].mazeObject == null )
                        walleable1 = true;
                    walleable = walleable1 && walleable2;
                }
            }
            for(int iCol = 0; iCol < size; iCol+= 2) 
            {
                int iRow = size-1;
                bool available1 = false;
                bool available2 = false;
                for (;maze[iRow,iCol].mazeObject == null; iRow--)
                {
                    
                    if (iCol + 1 == size || maze[iRow, iCol + 1].mazeObject == null)
                        available1 = true;
                    if (iCol - 1 == -1 || maze[iRow, iCol - 1].mazeObject == null)
                        available2 = true;
                    if (available1 && available2) break;
                }
                if (available1 && available2) continue;
                if(iCol-1 != -1)
                {
                    CleanCell(random.Next(iRow + 1, size), iCol - 1);
                }
                if (iCol + 1 != size)
                {
                    CleanCell(random.Next(iRow + 1, size), iCol + 1);
                }
            }
        }
        public void CleanCell(int row, int col)
        {
            if (maze[row,col].mazeObject != null)
            {
                maze[row,col].mazeObject = null;
                emptyCells++;
                boolMask[row, col] = true;
                freeCells.Add(maze[row, col]);
            }
        }
        public void VerifyMaze(int iRow,int iCol)
        {
            int[] dirRows = new int[] {0,1,0,-1};
            int[] dirCols = new int[] {1,0,-1,0};
            boolMask[iRow,iCol] = false;
            roads++;
            for (int i = 0; i < dirRows.Length; i++)
            {
                if(!OutOfRange(iRow + dirRows[i],iCol + dirCols[i]) && boolMask[iRow + dirRows[i], iCol + dirCols[i]])
                {
                    VerifyMaze(iRow + dirRows[i], iCol + dirCols[i]);
                }
            }

        }
        public bool OutOfRange(int iRow, int iCol)
        {
            return iRow  == -1 || iRow == size || iCol  == -1 || iCol == size;
        }
        public void SetTrapps(int number)
        {
            Random random = new Random();
            for(; number > 0; number--)
            {

            }
        }
        public void SetMazeObject(ClassMazeObject mzObj, int iRow, int iColumn)
        {
            maze[iRow, iColumn].SetMazeObject(mzObj);
            
        }
        public ClassCell RandomNotOcupiedCell()
        {
            Random ran = new Random();
            ClassCell randomCell;
            do
            {
                randomCell = Factory.game.maze.freeCells[ran.Next(Factory.game.maze.freeCells.Count)];
            } while (randomCell.character != null || randomCell.mazeObject != null);
            return randomCell;
        }
    }
}
