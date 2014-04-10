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

        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        private string _healingFeedbackSpriteSheetTag = "";

        public string HealingFeedbackSpriteSheetTag
        {
            get { return _healingFeedbackSpriteSheetTag; }
            set { _healingFeedbackSpriteSheetTag = value; }
        }


        #endregion

        private bool _noGravity = false;
        private AvatarCore _avatar;

        private SpriteSheetInfo _healingFeedback;

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

            _healingFeedback = AssetManager.GetSpriteSheet(_healingFeedbackSpriteSheetTag);
            
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (entity.hitboxes.Count > 0 && _avatar != null && _avatar.entity.hitboxes[0] != null && State == CollectibleState.Available)
            {
                if (entity.hitboxes[0].hitboxRectangle.Intersects(_avatar.entity.hitboxes[0].hitboxRectangle))
                {
                    _avatar.CurrentHealthPoints = Math.Min(_avatar.CurrentHealthPoints + _healthValue, _avatar.StartingHealthPoints);
                    EffectsManager.GetEffect(_healingFeedback, _avatar.CurrentSide, _avatar.entity.transform.Position, 0.0f, Vector2.Zero, 2.0f, 0.52f, _avatar.entity);
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
