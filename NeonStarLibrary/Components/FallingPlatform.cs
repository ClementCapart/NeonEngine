﻿using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    class FallingPlatform : Component
    {
        #region Properties
        private float _fallingTimer = 1.0f;

        public float FallingTimer
        {
            get { return _fallingTimer; }
            set { _fallingTimer = value; }
        }

        private float _fallSpeed = 50.0f;

        public float FallSpeed
        {
            get { return _fallSpeed; }
            set { _fallSpeed = value; }
        }

        private float _maxDistanceBeforeDestroying = 2000.0f;

        public float MaxDistanceBeforeDestroying
        {
            get { return _maxDistanceBeforeDestroying; }
            set { _maxDistanceBeforeDestroying = value; }
        }

        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }
        #endregion

        private float _currentFallingTimer = 1.0f;
        private Entity _avatar;

        private bool _startToCrumble = false;
        private Vector2 _startPosition;

        public FallingPlatform(Entity entity)
            :base(entity, "FallingPlatform")
        {
        }

        public override void Init()
        {
            if (this.entity.rigidbody != null)
            {
                this.entity.rigidbody.BodyType = FarseerPhysics.Dynamics.BodyType.Kinematic;
                this.entity.rigidbody.Init();
            }

            _startPosition = this.entity.transform.Position;
            _avatar = entity.containerWorld.GetEntityByName(_avatarName);         

            _currentFallingTimer = _fallingTimer;
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_startToCrumble)
            {
                if (_currentFallingTimer > 0.0f)
                {
                    _currentFallingTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_currentFallingTimer <= 0.0f)
                    {
                        _currentFallingTimer = 0.0f;
                        if (this.entity.rigidbody != null)
                        {
                            this.entity.rigidbody.body.LinearVelocity = new Vector2(0, FallSpeed);
                        }
                    }
                }
                else
                {
                    if (Vector2.Distance(this.entity.transform.Position, this._startPosition) > _maxDistanceBeforeDestroying)
                        this.entity.Destroy();
                }
            }
            else
            {
                if (_avatar != null && entity.hitboxes[0] != null)
                {
                    Rigidbody rb = _avatar.rigidbody.beacon.CheckGround();
                    if (_avatar != null && rb != null && rb.entity == this.entity)
                    {
                        _startToCrumble = true;
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}