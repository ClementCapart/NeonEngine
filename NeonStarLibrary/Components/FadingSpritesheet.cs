using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Graphics2D
{
    public class FadingSpritesheet : SpriteSheet
    {
        #region Properties
        private float _fadingSpeed = 1.0f;

        public float FadingSpeed
        {
            get { return _fadingSpeed; }
            set { _fadingSpeed = value; }
        }

        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }
        #endregion

        private Entity _avatarEntity;

        public FadingSpritesheet(Entity entity)
            :base(entity)
        {
            Name = "FadingSpritesheet";
            RequiredComponents = new Type[] { typeof(Hitbox) };
        }

        public override void Init()
        {
            this.Opacity = 0.0f;
            _avatarEntity = entity.GameWorld.GetEntityByName(_avatarName);
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_avatarEntity != null && _avatarEntity.hitboxes.Count > 0)
            {
                if (_avatarEntity.hitboxes[0].hitboxRectangle.Intersects(entity.hitboxes[0].hitboxRectangle))
                    this.Opacity = Math.Min(1.0f, Opacity + (float)gameTime.ElapsedGameTime.TotalSeconds * _fadingSpeed);
                else
                    this.Opacity = Math.Max(0.0f, Opacity - (float)gameTime.ElapsedGameTime.TotalSeconds * _fadingSpeed);
            }
            base.Update(gameTime);
        }


    }
}
