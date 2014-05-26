using NeonEngine;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.GameplayElements
{
    public class SlotCollectible : Collectible
    {
        #region Properties
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

        private AvatarCore _avatar;

        public SlotCollectible(Entity entity)
            :base(entity, "SlotCollectible")
        {
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
            if (State == CollectibleState.Used)
            {
                if (entity.spritesheets != null)
                    entity.spritesheets.ChangeAnimation(_outAnimation, true, 0, true, false, false);
            }
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (entity.hitboxes[0] != null && _avatar != null && _avatar.entity.hitboxes[0] != null && State == CollectibleState.Available)
            {
                if (entity.hitboxes[0].hitboxRectangle.Intersects(_avatar.entity.hitboxes[0].hitboxRectangle))
                {
                    Gather();
                }
            }
            base.Update(gameTime);
        }

        private void Gather()
        {
            State = CollectibleState.Used;
            if (_avatar != null) _avatar.ElementSystem.ChangeMaxLevel((int)_avatar.ElementSystem.MaxLevel + 1);
            if (entity.spritesheets != null)
                entity.spritesheets.ChangeAnimation(_outAnimation, true, 0, true, false, false);
        }

    }
}
