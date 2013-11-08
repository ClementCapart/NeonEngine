using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class EnemyAttack : Component
    {
        public Enemy EnemyComponent;
        public Attack CurrentAttack;

        private string _attackToLaunchOne = "";

        public string AttackToLaunchOne
        {
            get { return _attackToLaunchOne; }
            set { _attackToLaunchOne = value; }
        }

        private float _rangeForAttackTwo = 0.0f;

        public float RangeForAttackTwo
        {
            get { return _rangeForAttackTwo; }
            set { _rangeForAttackTwo = value; }
        }

        private string _attackToLaunchTwo = "";

        public string AttackToLaunchTwo
        {
            get { return _attackToLaunchTwo; }
            set { _attackToLaunchTwo = value; }
        }

        private float _rangeForAttackThree = 0.0f;

        public float RangeForAttackThree
        {
            get { return _rangeForAttackThree; }
            set { _rangeForAttackThree = value; }
        }

        private string _attackToLaunchThree = "";

        public string AttackToLaunchThree
        {
            get { return _attackToLaunchThree; }
            set { _attackToLaunchThree = value; }
        }

        private float _rangeForAttackFour = 0.0f;

        public float RangeForAttackFour
        {
            get { return _rangeForAttackFour; }
            set { _rangeForAttackFour = value; }
        }

        private string _attackToLaunchFour = "";

        public string AttackToLaunchFour
        {
            get { return _attackToLaunchFour; }
            set { _attackToLaunchFour = value; }
        }

        private float _rangeForAttackFive = 0.0f;

        public float RangeForAttackFive
        {
            get { return _rangeForAttackFive; }
            set { _rangeForAttackFive = value; }
        }

        private string _attackToLaunchFive = "";

        public string AttackToLaunchFive
        {
            get { return _attackToLaunchFive; }
            set { _attackToLaunchFive = value; }
        }

        public List<Attack> LocalAttacksInCooldown;

        public EnemyAttack(Entity entity)
            :base(entity, "EnemyAttack")
        {
        }

        public override void Init()
        {
            LocalAttacksInCooldown = new List<Attack>();
            EnemyComponent = entity.GetComponent<Enemy>();
            if (EnemyComponent != null)
                EnemyComponent._attack = this;
            base.Init();
        }

        public override void PreUpdate(GameTime gameTime)
        {
            for (int i = LocalAttacksInCooldown.Count - 1; i >= 0; i--)
            {
                LocalAttacksInCooldown[i].LocalCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (LocalAttacksInCooldown[i].LocalCooldown <= 0.0f)
                    LocalAttacksInCooldown.RemoveAt(i);
            }

            if (CurrentAttack == null && EnemyComponent.State == EnemyState.Attack)
            {
                if (_rangeForAttackFive != 0.0f && Vector2.DistanceSquared(entity.transform.Position, EnemyComponent._threatArea.EntityFollowed.transform.Position) < _rangeForAttackFive * _rangeForAttackFive)
                {
                    if (_attackToLaunchFive == "Chase")
                    {
                        EnemyComponent.State = EnemyState.Chase;
                    }
                    else
                    {
                        bool inLocalCooldown = false;
                        foreach (Attack a in LocalAttacksInCooldown)
                        {
                            if (a.Name == _attackToLaunchFive)
                                inLocalCooldown = true;
                        }

                        if(!inLocalCooldown)
                            CurrentAttack = AttacksManager.GetAttack(_attackToLaunchFive, entity.spritesheets.CurrentSide, entity, EnemyComponent._threatArea.EntityFollowed, true);
                    }

                    if (CurrentAttack == null)
                    {
                        EnemyComponent.State = EnemyState.Chase;
                    }
                }
                else if (_rangeForAttackFour != 0.0f && Vector2.DistanceSquared(entity.transform.Position, EnemyComponent._threatArea.EntityFollowed.transform.Position) < _rangeForAttackFour * _rangeForAttackFour)
                {
                    if (_attackToLaunchFour == "Chase")
                    {
                        EnemyComponent.State = EnemyState.Chase;
                    }
                    else
                    {
                        bool inLocalCooldown = false;
                        foreach (Attack a in LocalAttacksInCooldown)
                        {
                            if (a.Name == _attackToLaunchFour)
                                inLocalCooldown = true;
                        }

                        if (!inLocalCooldown)
                            CurrentAttack = AttacksManager.GetAttack(_attackToLaunchFour, entity.spritesheets.CurrentSide, entity, EnemyComponent._threatArea.EntityFollowed, true);
                    }

                    if (CurrentAttack == null)
                    {
                        EnemyComponent.State = EnemyState.Chase;
                    }
                }
                else if (_rangeForAttackThree != 0.0f && Vector2.DistanceSquared(entity.transform.Position, EnemyComponent._threatArea.EntityFollowed.transform.Position) < _rangeForAttackThree * _rangeForAttackThree)
                {
                    if (_attackToLaunchThree == "Chase")
                    {
                        EnemyComponent.State = EnemyState.Chase;
                    }
                    else
                    {
                        bool inLocalCooldown = false;
                        foreach (Attack a in LocalAttacksInCooldown)
                        {
                            if (a.Name == _attackToLaunchThree)
                                inLocalCooldown = true;
                        }

                        if (!inLocalCooldown)
                            CurrentAttack = AttacksManager.GetAttack(_attackToLaunchThree, entity.spritesheets.CurrentSide, entity, EnemyComponent._threatArea.EntityFollowed, true);
                    }

                    if (CurrentAttack == null)
                    {
                        EnemyComponent.State = EnemyState.Chase;
                    }
                }
                else if (_rangeForAttackTwo != 0.0f && Vector2.DistanceSquared(entity.transform.Position, EnemyComponent._threatArea.EntityFollowed.transform.Position) < _rangeForAttackTwo * _rangeForAttackTwo)
                {
                    if (_attackToLaunchTwo == "Chase")
                    {
                        EnemyComponent.State = EnemyState.Chase;
                    }
                    else
                    {
                        bool inLocalCooldown = false;
                        foreach (Attack a in LocalAttacksInCooldown)
                        {
                            if (a.Name == _attackToLaunchTwo)
                                inLocalCooldown = true;
                        }

                        if (!inLocalCooldown)
                            CurrentAttack = AttacksManager.GetAttack(_attackToLaunchTwo, entity.spritesheets.CurrentSide, entity, EnemyComponent._threatArea.EntityFollowed, true);
                    }

                    if (CurrentAttack == null)
                    {
                        EnemyComponent.State = EnemyState.Chase;
                    }
                }
                else
                {
                    if (_attackToLaunchOne == "Chase")
                    {
                        EnemyComponent.State = EnemyState.Chase;
                    }
                    else
                    {
                        bool inLocalCooldown = false;
                        foreach (Attack a in LocalAttacksInCooldown)
                        {
                            if (a.Name == _attackToLaunchOne)
                                inLocalCooldown = true;
                        }

                        if(!inLocalCooldown)
                            CurrentAttack = AttacksManager.GetAttack(_attackToLaunchOne, entity.spritesheets.CurrentSide, entity, EnemyComponent._threatArea.EntityFollowed, true);
                    }
                    
                    if (CurrentAttack == null)
                    {
                        EnemyComponent.State = EnemyState.Chase;
                    }
                }
            }

            if (EnemyComponent.State == EnemyState.Attack)
            {
                if (CurrentAttack != null)
                {
                    if (CurrentAttack.CooldownFinished && CurrentAttack.AirLockFinished)
                    {
                        CurrentAttack = null;
                        if (entity.spritesheets != null && (entity.spritesheets.CurrentSpritesheet.IsLooped || !entity.spritesheets.CurrentSpritesheet.IsLooped && entity.spritesheets.IsFinished()))
                            entity.spritesheets.CurrentPriority = 0;
                    }
                }
            }
            else
            {
                if (CurrentAttack != null) CurrentAttack.CancelAttack();
                CurrentAttack = null;
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {          
            if (EnemyComponent.State == EnemyState.Attack)
            {
                if (CurrentAttack != null)
                {
                        CurrentAttack.Update(gameTime);
                }                   
            }
            base.Update(gameTime);
        }


    }
}
