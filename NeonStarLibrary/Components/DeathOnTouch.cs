using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeonStarLibrary.Components.Avatar;
using NeonEngine.Components.CollisionDetection;
using Microsoft.Xna.Framework;

namespace NeonStarLibrary.Components.GameplayElements
{
    public class DeathOnTouch : Component
    {
        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        private float _healthPointsLoss = 15.0f;

        public float HealthPointsLoss
        {
            get { return _healthPointsLoss; }
            set { _healthPointsLoss = value; }
        }

        private float _cameraChaseStrengthReset = 0.0f;

        public float CameraChaseStrengthReset
        {
            get { return _cameraChaseStrengthReset; }
            set { _cameraChaseStrengthReset = value; }
        }

        private float _cameraChaseStrengtheningRate = 0.05f;

        public float CameraChaseStrengtheningRate
        {
            get { return _cameraChaseStrengtheningRate; }
            set { _cameraChaseStrengtheningRate = value; }
        }

        AvatarCore _avatar;

        public DeathOnTouch(Entity entity)
            :base(entity, "DeathOnTouch")
        {
            RequiredComponents = new Type[] { typeof(Hitbox) };
        }

        public override void Init()
        {
            Entity e = entity.GameWorld.GetEntityByName(_avatarName);
            if (e != null)
                _avatar = e.GetComponent<AvatarCore>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_avatar != null)
            {
                if (_avatar.entity.hitboxes[0] != null && entity.hitboxes[0] != null)
                {
                    if (_avatar.entity.hitboxes[0].hitboxRectangle.Intersects(entity.hitboxes[0].hitboxRectangle))
                    {
                        _avatar.CurrentHealthPoints -= _healthPointsLoss;
                        if (_avatar.CurrentHealthPoints > 0.0f)
                        {
                            _avatar.State = AvatarState.Respawning;
                            entity.GameWorld.Camera.ChaseStrength = CameraChaseStrengthReset;
                            entity.GameWorld.Camera.ChaseStrengtheningRate = CameraChaseStrengtheningRate;
                            if ((entity.GameWorld as GameScreen).InstantRespawnPoint != new Vector2(float.MaxValue, float.MaxValue))
                                _avatar.entity.transform.Position = (entity.GameWorld as GameScreen).InstantRespawnPoint;
                        }                    
                    }
                }
            }

            base.Update(gameTime);
        }


    }
}
