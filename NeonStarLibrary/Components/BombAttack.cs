﻿using NeonEngine;
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
        #endregion

        public EnemyCore EnemyComponent;

        private float _currentTimer = 0.0f;

        private bool _touchedGround;
        private bool _hasBeenLaunched = false;

        public BombAttack(Entity entity)
            :base(entity, "BombAttack")
        {
            RequiredComponents = new Type[] { typeof(EnemyCore), typeof(Rigidbody) };
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<EnemyCore>();

            _currentTimer = _initialTimerDuration;
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
                if (_currentTimer > 0.0f)
                {
                    if (!_hasBeenLaunched)
                    {
                        if (EnemyComponent.LastAttackTook == EffectiveAttackName || EnemyComponent.LastAttackTook == EffectiveAttackName + "Finish")
                        {
                            _hasBeenLaunched = true;
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
            base.Update(gameTime);
        }

        private void Explode()
        {
            if (_hasBeenLaunched)
            {
                Console.WriteLine("Good Boom ! ");

                if (_explosionAttack != "")
                {
                    Attack a = AttacksManager.StartFreeAttack(_explosionAttack, Side.Right, entity.transform.Position);
                    a.FromEnemy = false;
                }
                
            }
            else
            {
                Console.WriteLine("Bad Boom ! ");
                if (_explosionAttack != "")
                {
                    Attack a = AttacksManager.StartFreeAttack(_explosionAttack, Side.Right, entity.transform.Position);
                    a.FromEnemy = true;
                }
                
            }

            entity.Destroy();
        }


    }
}
