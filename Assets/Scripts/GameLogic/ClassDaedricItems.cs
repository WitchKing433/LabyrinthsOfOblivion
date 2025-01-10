using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryMazeGame
{
    public class ClassDaedricItems:ClassMazeObject
    {
        string name;
        int id;
        public int Id { get { return id; } }
        public string Name {  get { return name; } }

        public ClassDaedricItems(string n, int i)
        {
            name = n;
            id = i;
        }
        public void SetDaedricItem(ClassCell cell)
        {
            cell.SetMazeObject(this);
        }
        public int PickDaedricItem(ClassCharacter character)
        {
            if (id == character.Id)
            {
                cell.SetMazeObject(null);
                cell = null;
                character.daedricItem = this;
                character.PickDaedricItem();
                return 3;
            }
            ClassPlayer player = character.owner;
            for (int i = 0; i < player.team.Count; i ++)
            {
                if (player.team[i].Id == id)
                {
                    return 1;
                }
            }
            character.BuffPower(10);
            character.basePower += 10;
            return 3;
        }
    }
}
