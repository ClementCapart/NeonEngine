using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Enemies
{
    public abstract class EnemyAttack : Component
    {
        
        #region Properties
        protected bool _canTurn = true;

        public bool CanTurn
        {
            get { return _canTurn; }
            set { _canTurn = value; }
        }

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

        protected string _attackOneDelayAnimation = "";

        public string AttackOneDelayAnimation
        {
            get { return _attackOneDelayAnimation; }
            set { _attackOneDelayAnimation = value; }
        }

        protected string _attackOneDurationAnimation = "";

        public string AttackOneDurationAnimation
        {
            get { return _attackOneDurationAnimation; }
            set { _attackOneDurationAnimation = value; }
        }

        protected string _attackOneCooldownAnimation = "";

        public string AttackOneCooldownAnimation
        {
            get { return _attackOneCooldownAnimation; }
            set { _attackOneCooldownAnimation = value; }
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

        protected string _attackTwoDelayAnimation = "";

        public string AttackTwoDelayAnimation
        {
            get { return _attackTwoDelayAnimation; }
            set { _attackTwoDelayAnimation = value; }
        }

        protected string _attackTwoDurationAnimation = "";

        public string AttackTwoDurationAnimation
        {
            get { return _attackTwoDurationAnimation; }
            set { _attackTwoDurationAnimation = value; }
        }

        protected string _attackTwoCooldownAnimation = "";

        public string AttackTwoCooldownAnimation
        {
            get { return _attackTwoCooldownAnimation; }
            set { _attackTwoCooldownAnimation = value; }
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

        protected string _attackThreeDelayAnimation = "";

        public string AttackThreeDelayAnimation
        {
            get { return _attackThreeDelayAnimation; }
            set { _attackThreeDelayAnimation = value; }
        }

        protected string _attackThreeDurationAnimation = "";

        public string AttackThreeDurationAnimation
        {
            get { return _attackThreeDurationAnimation; }
            set { _attackThreeDurationAnimation = value; }
        }

        protected string _attackThreeCooldownAnimation = "";

        public string AttackThreeCooldownAnimation
        {
            get { return _attackThreeCooldownAnimation; }
            set { _attackThreeCooldownAnimation = value; }
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

        protected string _attackFourDelayAnimation = "";

        public string AttackFourDelayAnimation
        {
            get { return _attackFourDelayAnimation; }
            set { _attackFourDelayAnimation = value; }
        }

        protected string _attackFourDurationAnimation = "";

        public string AttackFourDurationAnimation
        {
            get { return _attackFourDurationAnimation; }
            set { _attackFourDurationAnimation = value; }
        }

        protected string _attackFourCooldownAnimation = "";

        public string AttackFourCooldownAnimation
        {
            get { return _attackFourCooldownAnimation; }
            set { _attackFourCooldownAnimation = value; }
        }
   
        private string _entityToAttackName = "";

        public string EntityToAttackName
        {
            get { return _entityToAttackName; }
            set { _entityToAttackName = value; }
        }
        #endregion

        public EnemyCore EnemyComponent;
        public Attack CurrentAttack;
        
        public Entity EntityToAttack = null;

        public List<Attack> LocalAttacksInCooldown;
        public Dictionary<float, List<string>> _attacks;

        public EnemyAttack(Entity entity)
            :base(entity, "EnemyAttack")
        {
            RequiredComponents = new Type[] { typeof(EnemyCore) };
        }

        public override void Init()
        {
            LocalAttacksInCooldown = new List<Attack>();
            EnemyComponent = entity.GetComponent<EnemyCore>();
            if (_entityToAttackName != "")
                EntityToAttack = entity.GameWorld.GetEntityByName(_entityToAttackName);

            if (EnemyComponent != null)
                EnemyComponent.Attack = this;

            _attacks = new Dictionary<float, List<string>>();

            if (_rangeForAttackOne != 0.0f)
            {
                if (!_attacks.ContainsKey(_rangeForAttackOne))
                    _attacks.Add(_rangeForAttackOne, new List<string>());

                _attacks[_rangeForAttackOne].Add(_attackToLaunchOne);
            }
            if (_rangeForAttackTwo != 0.0f)
            {
                if (!_attacks.ContainsKey(_rangeForAttackTwo))
                    _attacks.Add(_rangeForAttackTwo, new List<string>());

                _attacks[_rangeForAttackTwo].Add(_attackToLaunchTwo);
            }
            if (_rangeForAttackThree != 0.0f)
            {
                if (!_attacks.ContainsKey(_rangeForAttackThree))
                    _attacks.Add(_rangeForAttackThree, new List<string>());

                _attacks[_rangeForAttackThree].Add(_attackToLaunchThree);
            }
            if (_rangeForAttackFour != 0.0f)
            {
                if (!_attacks.ContainsKey(_rangeForAttackFour))
                    _attacks.Add(_rangeForAttackFour, new List<string>());

                _attacks[_rangeForAttackFour].Add(_attackToLaunchFour);
            }

            _attacks = _attacks.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp2 => kvp2.Value);

            base.Init();
        }

        public override void PreUpdate(GameTime gameTime)
        {
            if (EnemyComponent.State != EnemyState.Dying && EnemyComponent.State != EnemyState.Dead)
            {
                for (int i = LocalAttacksInCooldown.Count - 1; i >= 0; i--)
                {
                    LocalAttacksInCooldown[i].LocalCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (LocalAttacksInCooldown[i].LocalCooldown <= 0.0f)
                        LocalAttacksInCooldown.RemoveAt(i);
                }
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {           
            base.Update(gameTime);
        }
    }
}
