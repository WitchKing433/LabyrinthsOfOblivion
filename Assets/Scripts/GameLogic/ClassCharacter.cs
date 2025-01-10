using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.TextCore.Text;
using static UnityEngine.EventSystems.EventTrigger;

namespace ClassLibraryMazeGame
{
    public class ClassCharacter:ClassMazeObject
    {
        public delegate void SendToBaseEvent();
        public event SendToBaseEvent sendToBaseEvent;
        public ClassPlayer owner;
        public ClassDaedricItems daedricItem;
        string name;
        int id;
        int baseHealth;
        int health;
        int steps;
        public int basePower;
        int power;
        int modifySteps = 0;
        int validSteps;
        int cooldown;
        int skillDuration;
        int inactiveTime;
        public int poisoned;
        public bool canAttack = true;
        public bool selectionSkill;
        bool canActivateTrapps = true;
        int armor = 0;
        bool mehrunesRazor;
        bool poisonAura = false;
        public ClassCharacter(string n, int h, int s, int p, int i)
        {
            name = n;
            baseHealth = h;
            health = h;
            steps = s;
            validSteps = s;
            basePower = p;
            power = p;
            id = i;
            if (name == "Boethiah")
            {
                selectionSkill = true;
            }
        }
        public int BaseHealth{ get {return baseHealth; } }
        public string Name { get { return name; } }
        public int InactiveTime { get { return inactiveTime; } }
        public int CoolDown { get { return cooldown; } }
        public int SkillDuration { get { return skillDuration; } }
        public int Id { get { return id; } }
        public int Power
        {
            get { return power; }

            set
            {
                if (value <= 0)
                {
                    power = 0;
                }
                else { power = value; }
            }
        }
        public int Health
        {
            get{ return health;}
            set
            {
                if (value <= 0)
                {
                    health = 0;
                    GotoBase();
                }
                else { health = value; }
            }               
        }
        public int Steps
        {
            get { return steps; }
            private 
            set
            {
                if (value <= 0)
                {
                    steps = 0;
                }
                else { steps = value; }
            }
        }
        public int ValidSteps
        {
            get { return validSteps; }

            set
            {
                if (value <= 0)
                {
                    validSteps = 0;
                }
                else { validSteps = value; }
            }
        }
        public int MoveTo(ClassCell destination)
        {
            if (ValidSteps > 0)
            {
                bool validRow = (destination.Row + 1 == cell.Row || destination.Row - 1 == cell.Row) && destination.Column == cell.Column;
                bool validColumn = (destination.Column + 1 == cell.Column || destination.Column - 1 == cell.Column) && destination.Row == cell.Row;
                if ((validColumn ^ validRow) && !(destination.mazeObject is ClassWall) && !(destination.character is ClassCharacter))
                {
                    if (destination.mazeObject is ClassTrapp && owner.canActivateTrapps && canActivateTrapps)
                    {                        
                        Teleport(destination);
                        ValidSteps--;
                        return 2;
                    }
                    else if (destination.mazeObject is ClassDaedricItems)
                    {
                        Teleport(destination);
                        ValidSteps--;
                        return ((ClassDaedricItems)destination.mazeObject).PickDaedricItem(this);
                    }
                    Teleport(destination);
                    ValidSteps--;
                    return 1;
                }
            }
            return 0;
        }
        public void Teleport(ClassCell destination)
        {
            this.cell.SetCharacter(null);
            destination.SetCharacter(this);
        }
        public void RestoreSteps()
        {
            ValidSteps = steps + modifySteps;
        }
        public void GotoBase()
        {
            if(owner.asleep > 0 && owner.opponent.skullOfCorruption)
            {
                for (int i = 0; i < owner.opponent.team.Count; i++)
                {
                    owner.opponent.team[i].BuffPower(10);
                    owner.opponent.team[i].basePower += 10;
                }
            }
            poisoned = 0;
            owner.selfBase.AddCharacterToBase(this);
            this.cell.SetCharacter(null);
            this.cell = null;
            inactiveTime = 5;
            sendToBaseEvent.Invoke();
        }

        public void Damaged(int h)
        {
            if(armor - h < 0)
                Health -= h - armor;
        }
        public void Heal(int h)
        {
            if (health < baseHealth)
            {
                Health += h;
                if(health > baseHealth) 
                    health = baseHealth;
            }                       
        }
        public void DeSpeed(int s)
        {
            ValidSteps -= s;
        }
        public void SpeedUp(int s)
        {
            ValidSteps += s;
        }
        public void BuffPower(int p)
        {
            Power += p;
        }
        public void DeBuffPower(int p)
        {
            Power -= p;
        }
        public bool Attack(ClassCharacter enemy)
        {
            if (canAttack && Factory.game.maze.CloseCells(cell,enemy.cell))
            {
                if (mehrunesRazor)
                {
                    Random random = new Random();
                    if(random.Next(6) == 2)
                        enemy.Damaged(10000);
                }
                else
                    enemy.Damaged(power);
                canAttack = false;
                return true;
            }
            return false;
        }
        public bool AttackBuilding(ClassBase target)
        {
            if (canAttack && Factory.game.maze.CloseCells(cell, target.cell))
            {                                            
                target.Damaged(power);
                canAttack = false;
                return true;
            }
            return false;
        }
        public bool ActivateSkill(ClassCharacter target = null)
        {
            Random random = new Random();
            if(cooldown == 0)
            {
                if(name == "Vaermina")
                {
                    owner.opponent.asleep += 2;
                    cooldown = 7;
                    return true;
                }
                if(name == "Hermaeus Mora")
                {
                    owner.canActivateTrapps = false;
                    cooldown = 5;
                    skillDuration = 3;
                    if (daedricItem != null)
                        skillDuration++;
                    return true;
                }
                if (name == "Sheogorath")
                {
                    for(int i = 0; i < owner.opponent.team.Count; i++)
                    {
                        if (owner.opponent.team[i].inactiveTime <= 0) 
                        {
                            owner.opponent.team[i].Health = random.Next(1, 100);
                            owner.opponent.team[i].ValidSteps = random.Next(5, 31);
                            owner.opponent.team[i].Power = random.Next(1, 31);
                            if(daedricItem != null)
                            {
                                owner.opponent.team[i].modifySteps = -random.Next(1, 20);
                                skillDuration = 2;
                            }
                        }
                    }
                    cooldown = 7;
                    return true;
                }
                if (name == "Mehrunes Dagon")
                {
                    BuffPower(baseHealth - health);
                    cooldown = 5;
                    skillDuration = 1;
                    return true;
                }
                if(name == "Peryite")
                {
                    for(int i = 5; i > 0; i--)
                    {
                        if (Factory.game.maze.freeCells.Count > 1)
                        {
                            ClassCell destination = Factory.game.maze.RandomNotOcupiedCell();
                            ClassTrapp.SetTrapp(destination, 3);
                        }
                    }          
                    cooldown = 7;
                    return true;
                }
                if (name == "Boethiah")
                {
                    if (target != null && target.owner == owner) 
                    { 
                        return ActivateBoethiahSkill(target);
                    }
                }
            }
            return false;
        }
        bool ActivateBoethiahSkill(ClassCharacter ally)
        {
            if (ally.Health <= 25)
            {
                ally.Damaged(25);
                BuffPower(10);
                basePower += 10;
                cooldown = 2;
                return true;
            }
            return false;
        }
        public void ResetStats()
        {
            health = baseHealth;
            validSteps = steps;
            power = basePower;
            poisoned = 0;
            modifySteps = 0;
        }
        public void PassTurn()
        {
            if(poisoned > 0)
            {
                Damaged(poisoned);
                poisoned--;
            }
            if(inactiveTime > 0)
            {
                inactiveTime--;
                if(inactiveTime == 0)
                {
                    owner.selfBase.RandomPlaceCharacters();
                }
            }
            if(cooldown != 0)
            {
                cooldown--;
            }
            if (skillDuration > 0)
            {
                skillDuration--;
                if(skillDuration == 0)
                {
                    DeactivateSkill();
                }
            }
            if (poisonAura)
                PoisonAura();
            RestoreSteps();
            canAttack = true;
        }
        public void PickDaedricItem()
        {
            switch (id)
            {
                case 0:
                    canActivateTrapps = false;
                    steps += 10;
                    break;
                case 1:
                    owner.skullOfCorruption = true;
                    break;
                case 2:
                    break;
                case 3:
                    armor += 10;
                    break;
                case 4:
                    mehrunesRazor = true;
                    break;
                case 5:
                    armor += 5;
                    poisonAura = true;
                    break;
            }
        }
        void PoisonAura()
        {
            int[] dirRow = new int[] { 0, 1, 0, -1 };
            int[] dirCol = new int[] { 1, 0, -1, 0 };
            int iRow = cell.Row - 1;
            int iCol = cell.Column - 1;
            bool flag = true;
            for (int i = 0; ; iRow += dirRow[i % 4], iCol += dirCol[i % 4])
            {
                if (iRow >= 0 && iRow < ClassMaze.size && iCol >= 0 && iCol < ClassMaze.size && Factory.game.maze.maze[iRow, iCol].character != null && Factory.game.maze.maze[iRow, iCol].character.owner != owner)
                {
                    Factory.game.maze.maze[iRow, iCol].character.Damaged(10);
                }
                if (flag)
                {
                    flag = false;
                    continue;
                }
                if (iRow == cell.Row - 1 && iCol == cell.Column - 1)
                    break;
                if ((iRow == cell.Row + 1 && iCol == cell.Column + 1) || (iRow == cell.Row + 1 && iCol == cell.Column - 1) || (iRow == cell.Row - 1 && iCol == cell.Column + 1))
                    i++;
            }
        }
        public void DeactivateSkill()
        {
            if (name == "Hermaeus Mora")
                owner.canActivateTrapps = true;
            if(name == "Mehrunes Dagon")
                Power = basePower;
            if (name == "Sheogorath")
                modifySteps = 0;
        }

    }
}
