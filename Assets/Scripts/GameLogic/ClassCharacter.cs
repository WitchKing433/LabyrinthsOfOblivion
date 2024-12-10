using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.EventSystems.EventTrigger;

namespace ClassLibraryMazeGame
{
    public class ClassCharacter:ClassMazeObject
    {
        public delegate void SendToBaseEvent();
        public event SendToBaseEvent sendToBaseEvent;
        public ClassPlayer owner;
        string name;
        int baseHealth;
        int health;
        int steps;
        int basePower;
        int power;
        int modifySteps = 0;
        int validSteps;
        int cooldown;
        int skillDuration;
        int inactiveTime;
        public int poisoned;
        public bool canAttack = true;
        public bool selectionSkill;
        public ClassCharacter(string n, int h, int s, int p)
        {
            name = n;
            baseHealth = h;
            health = h;
            steps = s;
            validSteps = s;
            basePower = p;
            power = p;
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
            if (validSteps > 0)
            {
                bool validRow = (destination.Row + 1 == cell.Row || destination.Row - 1 == cell.Row) && destination.Column == cell.Column;
                bool validColumn = (destination.Column + 1 == cell.Column || destination.Column - 1 == cell.Column) && destination.Row == cell.Row;
                if ((validColumn ^ validRow) && !(destination.mazeObject is ClassWall) && !(destination.character is ClassCharacter))
                {
                    if (destination.mazeObject is ClassTrapp && owner.canActivateTrapps)
                    {                        
                        Teleport(destination);
                        validSteps--;
                        return 2;
                    }
                    Teleport(destination);
                    validSteps--;
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
            poisoned = 0;
            owner.selfBase.AddCharacterToBase(this);
            this.cell.SetCharacter(null);
            this.cell = null;
            inactiveTime = 5;
            sendToBaseEvent.Invoke();
        }

        public void Damaged(int h)
        {
            Health -= h;
        }
        public void Heal(int h)
        {
            Health += h;
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
                    owner.opponent.asleep += 2;                           //agregar el efecto 
                    cooldown = 7;
                    return true;
                }
                if(name == "Hermaeus Mora")
                {
                    owner.canActivateTrapps = false;
                    cooldown = 5;
                    skillDuration = 3;
                    return true;
                }
                if (name == "Sheogorath")
                {
                    for(int i = 0; i < owner.opponent.team.Count; i++)
                    {
                        owner.opponent.team[i].Health = random.Next(1, 100);
                        owner.opponent.team[i].ValidSteps = random.Next(21);
                        owner.opponent.team[i].Power = random.Next(30);                        
                    }
                    cooldown = 7;
                    return true;
                }
                if (name == "Mehrunes Dagon")
                {
                    BuffPower(baseHealth - health);
                    cooldown = 5;
                    skillDuration = 3;
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
                        ActivateBoethiahSkill(target);
                        return true;
                    }
                }
            }
            return false;
        }
        void ActivateBoethiahSkill(ClassCharacter ally)
        {
            if (ally.Health <= 25)
            {
                ally.Damaged(25);
                BuffPower(10);
                basePower += 10;
                cooldown = 2;
            }
        }
        public void ResetStats()
        {
            health = baseHealth;
            validSteps = steps;
            power = basePower;
            poisoned = 0;
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
            RestoreSteps();
            canAttack = true;
        }
        public void DeactivateSkill()
        {
            if (name == "Hermaeus Mora")
                owner.canActivateTrapps = true;
            if(name == "Mehrunes Dagon")
                Power = basePower;
        }

    }
}
