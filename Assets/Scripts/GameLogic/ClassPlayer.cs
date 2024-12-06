using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryMazeGame
{
    public class ClassPlayer
    {
        public ClassPlayer opponent;
        public int id;
        public List<ClassCharacter> team;
        public ClassBase selfBase;
        public bool canActivateTrapps = true;
        public int asleep = 0;
        public ClassPlayer(int i, int baseHealth, int baseRadius)
        {
            id = i;
            selfBase = new ClassBase(this, baseHealth, baseRadius );
            team = new List<ClassCharacter>();
        }
        public void AddCharacterToTeam(ClassCharacter character)
        {
            team.Add(character);
            character.owner = this;
            selfBase.AddCharacterToBase(character);            
        }
        public void PassTurn()
        {
            for (int i = 0; i < team.Count; i++)
            {
                team[i].PassTurn();
            }
            if(asleep != 0)
                asleep--;
            selfBase.RandomPlaceCharacters();
        }
    }
}
