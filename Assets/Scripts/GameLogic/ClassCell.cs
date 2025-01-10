using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryMazeGame
{
    public class ClassCell
    {
        int row, column;
        float x, y;
        public ClassMazeObject mazeObject;
        public ClassCharacter character;
        public int Row
        {
            get { return row; }
        }
        public int Column
        {
            get { return column; }
        }
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        public ClassCell(int x, int y)
        {
            mazeObject = new ClassWall();
            row = x; 
            column = y;
        }

        public void SetMazeObject(ClassMazeObject mzObj)
        {
            mazeObject = mzObj;
            if(mzObj != null)
                mzObj.cell = this;
        }
        public void SetCharacter(ClassCharacter entity)
        {
            character = entity;
            if(entity != null)
                entity.cell = this;
        }

    }
}
