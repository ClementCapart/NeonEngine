using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public abstract class EnemyAttack : Component
    {
        
        #region Properties
        protected float _rangeForAttackOne = 0.0f;

        public float RangeForAttackOne
        {
            get { return _rangeForAttackOne; }
            set { _rangeForAttackOne = value; }
        }

        protected string _attackToLaunchOne = "";

        public string AttackToLaunchOne
        {
            get { return _attackToLaunchOne; }
            set { _attackToLaunchOne = value; }
        }

        protected float _rangeForAttackTwo = 0.0f;

        public float RangeForAttackTwo
        {
            get { return _rangeForAttackTwo; }
            set { _rangeForAttackTwo = value; }
        }

        protected string _attackToLaunchTwo = "";

        public string AttackToLaunchTwo
        {
            get { return _attackToLaunchTwo; }
            set { _attackToLaunchTwo = value; }
        }

        protected float _rangeForAttackThree = 0.0f;

        public float RangeForAttackThree
        {
            get { return _rangeForAttackThree; }
            set { _rangeForAttackThree = value; }
        }

        protected string _attackToLaunchThree = "";

        public string AttackToLaunchThree
        {
            get { return _attackToLaunchThree; }
            set { _attackToLaunchThree = value; }
        }

        protected float _rangeForAttackFour = 0.0f;

        public float RangeForAttackFour
        {
            get { return _rangeForAttackFour; }
            set { _rangeForAttackFour = value; }
        }

        protected string _attackToLaunchFour = "";

        public string AttackToLaunchFour
        {
            get { return _attackToLaunchFour; }
            set { _attackToLaunchFour = value; }
        }

        protected float _rangeForAttackFive = 0.0f;

        public float RangeForAttackFive
        {
            get { return _rangeForAttackFive; }
            set { _rangeForAttackFive = value; }
        }

        protected string _attackToLaunchFive = "";

        public string AttackToLaunchFive
        {
            get { return _attackToLaunchFive; }
            set { _attackToLaunchFive = value; }
        }

        private string _entityToAttackName = "";

        public string EntityToAttackName
        {
            get { return _entityToAttackName; }
            set { _entityToAttackName = value; }
        }
        #endregion

        public Enemy EnemyComponent;
        public Attack CurrentAttack;
        
        public Entity EntityToAttack = null;

        public List<Attack> LocalAttacksInCooldown;

        public EnemyAttack(Entity entity)
            :base(entity, "EnemyAttack")
        {
        }

        public override void Init()
        {
            LocalAttacksInCooldown = new List<Attack>();
            EnemyComponent = entity.GetComponent<Enemy>();
            if (_entityToAttackName != "")
                EntityToAttack = Neon.world.GetEntityByName(_entityToAttackName);

            if (EnemyComponent != null)
                EnemyComponent.Attack = this;
            base.Init();
        }

        public override void PreUpdate(GameTime gameTime)
        {          
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {           
            base.Update(gameTime);
        }
    }
}
