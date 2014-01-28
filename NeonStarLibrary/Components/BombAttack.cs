using NeonEngine;
using NeonEngine.Components.CollisionDetection;
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

        #endregion

        private float _currentTimer = 0.0f;

        private bool _touchedGround;
        private bool _startedTimer = false;

        public BombAttack(Entity entity)
            :base(entity, "BombAttack")
        {
            RequiredComponents = new Type[] { typeof(Rigidbody) };
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
                if (_currentTimer < _initialTimerDuration)
                {
                    //if()
                    _currentTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_currentTimer >= _initialTimerDuration)
                    {
                        Console.WriteLine("Boom ! ");
                        entity.Destroy();
                    }
                }
            }
            base.Update(gameTime);
        }


    }
}
