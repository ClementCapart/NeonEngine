using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using NeonStarLibrary.Components.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Enemies
{
    public class BombAttack : Component
    {
        #region Properties
        private bool _startTimerOnGround = false;

        public bool StartTimerOnGround
        {
            get { return _startTimerOnGround; }
            set { _startTimerOnGround = value; }
        }

        private float _initialTimerDuration = 2.0f;

        public float InitialTimerDuration
        {
            get { return _initialTimerDuration; }
            set { _initialTimerDuration = value; }
        }

        private float _launchedTimerDuration = 1.0f;

        public float LaunchedTimerDuration
        {
            get { return _launchedTimerDuration; }
            set { _launchedTimerDuration = value; }
        }

        private string _effectiveAttackName = "LiOnUppercut";

        public string EffectiveAttackName
        {
            get { return _effectiveAttackName; }
            set { _effectiveAttackName = value; }
        }

        private string _explosionAttack = "";

        public string ExplosionAttack
        {
            get { return _explosionAttack; }
            set { _explosionAttack = value; }
        }

        private string _idleAnimation = "";

        public string IdleAnimation
        {
            get { return _idleAnimation; }
            set { _idleAnimation = value; }
        }

        private string _firstExplosionAnimation = "";

        public string FirstExplosionAnimation
        {
            get { return _firstExplosionAnimation; }
            set { _firstExplosionAnimation = value; }
        }

        private string _nextExplosionAnimation = "";

        public string NextExplosionAnimation
        {
            get { return _nextExplosionAnimation; }
            set { _nextExplosionAnimation = value; }
        }
        #endregion

        public EnemyCore EnemyComponent;

        private float _currentTimer = 0.0f;

        private bool _touchedGround;
        private bool _hasBeenLaunched = false;

        private Attack _attack;

        public BombAttack(Entity entity)
            :base(entity, "BombAttack")
        {
            RequiredComponents = new Type[] { typeof(EnemyCore), typeof(Rigidbody) };
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<EnemyCore>();

            _currentTimer = _initialTimerDuration;
            if (entity.spritesheets != null)
                entity.spritesheets.ChangeAnimation(_idleAnimation, true, 0, true, false, true);
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!_touchedGround && _startTimerOnGround)
            {
                if (entity.rigidbody != null)
                    if (entity.rigidbody.Sensors)
                    {
                        if (entity.rigidbody.isGrounded)
                            _touchedGround = true;
                    }
            }
            else if(!_startTimerOnGround || _touchedGround)
            {
                if (entity.spritesheets != null && !_hasBeenLaunched)
                {
                    entity.spritesheets.ChangeAnimation(_firstExplosionAnimation, false, 0, true, false, false);
                }
                if (_currentTimer > 0.0f)
                {
                    if (!_hasBeenLaunched)
                    {
                        if (EnemyComponent.LastAttackTook == EffectiveAttackName || EnemyComponent.LastAttackTook == EffectiveAttackName + "Finish")
                        {
                            _hasBeenLaunched = true;
                            if (entity.spritesheets != null)
                                entity.spritesheets.ChangeAnimation(_nextExplosionAnimation, true, 0, true, false, false);
                            _currentTimer = _launchedTimerDuration;
                        }
                        
                    }                 

                    _currentTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_currentTimer <= 0.0f)
                    {
                        Explode();                       
                    }
                }
            }

            if (_attack != null && entity.spritesheets.CurrentSpritesheet.IsFinished)
                entity.Destroy();

            base.Update(gameTime);
        }

        private void Explode()
        {
            entity.rigidbody.Remove();
            if (_hasBeenLaunched)
            {
                if (_explosionAttack != "")
                {
                    _attack = AttacksManager.StartFreeAttack(_explosionAttack, Side.Right, entity.transform.Position, false);
                }
                
            }
            else
            {
                if (_explosionAttack != "")
                {
                    _attack = AttacksManager.StartFreeAttack(_explosionAttack, Side.Right, entity.transform.Position, true);
                }
                
            }
        }


    }
}
