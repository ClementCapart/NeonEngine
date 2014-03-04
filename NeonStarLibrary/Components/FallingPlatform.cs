using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.GameplayElements
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

        private string _fallingAnim = "";

        public string FallingAnim
        {
            get { return _fallingAnim; }
            set { _fallingAnim = value; }
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
            _avatar = entity.GameWorld.GetEntityByName(_avatarName);         

            _currentFallingTimer = _fallingTimer;
            if(entity.spritesheets != null )
                entity.spritesheets.ChangeAnimation(_fallingAnim, false, 0, false, false, true);
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
                            entity.spritesheets.ChangeAnimation(_fallingAnim, true, 0, false, true, false);
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
                    Rigidbody rb = _avatar.rigidbody.beacon.CheckGround(Vector2.Zero);
                    if (_avatar != null && rb != null && rb.entity == this.entity)
                    {
                        _startToCrumble = true;
                        entity.spritesheets.ChangeAnimation(_fallingAnim, true, 0, true, true, true);
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}
