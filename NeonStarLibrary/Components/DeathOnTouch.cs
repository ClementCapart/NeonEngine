using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeonStarLibrary.Components.Avatar;
using NeonEngine.Components.CollisionDetection;

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

        AvatarCore _avatar;

        public DeathOnTouch(Entity entity)
            :base(entity, "DeathOnTouch")
        {
            RequiredComponents = new Type[] { typeof(Hitbox) };
        }

        public override void Init()
        {
            Entity e = entity.containerWorld.GetEntityByName(_avatarName);
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
                        _avatar.CurrentHealthPoints = 0.0f;
                }
            }

            base.Update(gameTime);
        }


    }
}
