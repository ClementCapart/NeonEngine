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

        public EnemyAttack(Entity entity)
            :base(entity, "EnemyAttack")
        {
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<Enemy>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {          
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
                    else
                        CurrentAttack.Update(gameTime);
                }
                else
                {
                    if (_rangeForAttackFive != 0.0f && Vector2.DistanceSquared(entity.transform.Position, EnemyComponent._threatArea.EntityFollowed.transform.Position) < _rangeForAttackFive * _rangeForAttackFive)
                    {
                        CurrentAttack = AttacksManager.GetAttack(_attackToLaunchFive, entity.spritesheets.CurrentSide, entity, true);
                    }
                    else if (_rangeForAttackFour != 0.0f && Vector2.DistanceSquared(entity.transform.Position, EnemyComponent._threatArea.EntityFollowed.transform.Position) < _rangeForAttackFour * _rangeForAttackFour)
                    {
                        CurrentAttack = AttacksManager.GetAttack(_attackToLaunchFour, entity.spritesheets.CurrentSide, entity, true);
                    }
                    else if (_rangeForAttackThree != 0.0f && Vector2.DistanceSquared(entity.transform.Position, EnemyComponent._threatArea.EntityFollowed.transform.Position) < _rangeForAttackThree * _rangeForAttackThree)
                    {
                        CurrentAttack = AttacksManager.GetAttack(_attackToLaunchThree, entity.spritesheets.CurrentSide, entity, true);
                    }
                    else if (_rangeForAttackTwo != 0.0f && Vector2.DistanceSquared(entity.transform.Position, EnemyComponent._threatArea.EntityFollowed.transform.Position) < _rangeForAttackTwo * _rangeForAttackTwo)
                    {
                        CurrentAttack = AttacksManager.GetAttack(_attackToLaunchTwo, entity.spritesheets.CurrentSide, entity, true);
                    }
                    else
                        CurrentAttack = AttacksManager.GetAttack(_attackToLaunchOne, entity.spritesheets.CurrentSide, entity, true);
                }
                    
            }
            else
            {
                if(CurrentAttack != null ) CurrentAttack.CancelAttack();
                CurrentAttack = null;
            }
            base.Update(gameTime);
        }


    }
}
