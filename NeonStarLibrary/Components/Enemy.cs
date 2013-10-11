using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class Enemy : Component
    {
        private bool _debug;
        public bool Debug
        {
            get { return _debug; }
            set { _debug = value; }
        }
        private float _startingHealthPoints = 10.0f;
        public float StartingHealthPoints
        {
            get { return _startingHealthPoints; }
            set { _startingHealthPoints = value; }
        }

        private float _currentHealthPoints;
        private float _airLockDuration = 0.0f;

        public Enemy(Entity entity)
            :base(entity, "Enemy")
        {
        }

        public override void Init()
        {
            _currentHealthPoints = _startingHealthPoints;
            base.Init();
        }

        public void ChangeHealthPoints(float value)
        {
            _currentHealthPoints += value;
            if (Debug)
            {
                Console.WriteLine(entity.Name + " have lost " + value + " HP(s) -> Now at " + _currentHealthPoints + " HP(s).");
            }
        }

        public void AirLock(float duration)
        {
            _airLockDuration = duration;
            if(_airLockDuration > 0)
                entity.rigidbody.body.LinearVelocity = Vector2.Zero;           
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_airLockDuration > 0.0f)
            {
                _airLockDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                entity.rigidbody.GravityScale = 0.0f;
            }
            else
            {
                entity.rigidbody.GravityScale = entity.rigidbody.InitialGravityScale;
                _airLockDuration = 0.0f;
            }
            base.Update(gameTime);
        }
    }
}
