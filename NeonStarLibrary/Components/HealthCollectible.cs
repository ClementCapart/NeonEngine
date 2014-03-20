using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.GameplayElements
{
    public class HealthCollectible : Collectible
    {
        #region Properties

        private float _healthValue = 10.0f;

        public float HealthValue
        {
            get { return _healthValue; }
            set { _healthValue = value; }
        }

        private string _idleAnimation = "";

        public string IdleAnimation
        {
            get { return _idleAnimation; }
            set { _idleAnimation = value; }
        }

        private string _outAnimation = "";

        public string OutAnimation
        {
            get { return _outAnimation; }
            set { _outAnimation = value; }
        }

        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        #endregion

        private bool _noGravity = false;
        private AvatarCore _avatar;

        public HealthCollectible(Entity entity)
            :base(entity, "HealthCollectible")
        {
            RequiredComponents = new Type[] { typeof(Hitbox) };
        }

        public override void Init()
        {
            if (_avatarName != "")
            {
                Entity e = entity.GameWorld.GetEntityByName(_avatarName);
                if (e != null)
                    _avatar = e.GetComponent<AvatarCore>();
            }
            if (entity.spritesheets != null)
                entity.spritesheets.ChangeAnimation(_idleAnimation, false, 0, true, false, true);
            
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (entity.hitboxes.Count > 0 && _avatar != null && _avatar.entity.hitboxes[0] != null && State == CollectibleState.Available)
            {
                if (entity.hitboxes[0].hitboxRectangle.Intersects(_avatar.entity.hitboxes[0].hitboxRectangle))
                {
                    _avatar.CurrentHealthPoints = Math.Min(_avatar.CurrentHealthPoints + _healthValue, _avatar.StartingHealthPoints);
                    this.entity.Destroy();
                }
            }
            if (entity.rigidbody != null && (entity.rigidbody.body.LinearVelocity.Y >= 3.0f || _noGravity))
            {
                _noGravity = true; ;
                entity.rigidbody.body.GravityScale = 0.0f;
                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
            }
            base.Update(gameTime);
        }

        
    }
}
