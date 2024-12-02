﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryMazeGame
{
    public class ClassCharacter:ClassMazeObject
    {
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
        public ClassCharacter(string n, int h, int s, int p)
        {
            name = n;
            baseHealth = h;
            health = h;
            steps = s;
            validSteps = s;
            basePower = p;
            power = p;
        }
        public string Name { get { return name; } }
        public int InactiveTime { get { return inactiveTime; } }
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
        public bool MoveTo(ClassCell destination)
        {
            if (validSteps != 0)
            {
                bool validRow = (destination.Row + 1 == cell.Row || destination.Row - 1 == cell.Row) && destination.Column == cell.Column;
                bool validColumn = (destination.Column + 1 == cell.Column || destination.Column - 1 == cell.Column) && destination.Row == cell.Row;
                if ((validColumn ^ validRow) && !(destination.mazeObject is ClassWall) && !(destination.character is ClassCharacter))
                {
                    if (destination.mazeObject is ClassTrapp && owner.canActivateTrapps)
                    {
                        ((ClassTrapp)destination.mazeObject).ActivateTrapp(this);
                    }
                    Teleport(destination);
                    validSteps--;
                    return true;
                }
            }
            return false;
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
            owner.selfBase.AddCharacterToBase(this);
            this.cell.SetCharacter(null);
            this.cell = null;
            inactiveTime = 5;
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
        public void Attack(ClassCharacter enemy)
        {
            if (canAttack)
            {                                             // agregar condicion de que esten uno al lado del otro
                enemy.Damaged(power);
                canAttack = false;
            }
        }
        public void AttackBuilding(ClassBase target)
        {
            if (canAttack)
            {                                             // agregar condicion de que esten uno al lado del otro
                target.Damaged(power);
                canAttack = false;
            }
        }
        public void ActivateSkill(ClassCharacter target = null)
        {
            Random random = new Random();
            if(cooldown == 0)
            {
                if(name == "Vaermina")
                {
                    owner.opponent.asleep += 2;                           //agregar el efecto 
                    cooldown = 7;
                    return;
                }
                if(name == "Hermaeus Mora")
                {
                    owner.canActivateTrapps = false;
                    cooldown = 5;
                    skillDuration = 3;
                    return;
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
                    return;
                }
                if (name == "Mehrunes Dagon")
                {
                    BuffPower(baseHealth - health);
                    cooldown = 5;
                    skillDuration = 3;
                    return;
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
                    return;
                }
                if(name == "Boethiah")
                {
                    ActivateBoethiahSkill(target);
                    return;
                }
            }
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
            if(poisoned != 0)
            {
                Damaged(poisoned);
                poisoned--;
            }
            if(inactiveTime != 0)
            {
                inactiveTime--;
            }
            if(cooldown != 0)
            {
                cooldown--;
            }
            if(skillDuration != 0)
            {
                skillDuration--;
            }
        }


    }
}
