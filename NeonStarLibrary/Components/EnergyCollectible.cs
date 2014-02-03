using NeonEngine;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public class EnergyCollectible : Collectible
    {
        #region Properties

        private float _energyValue = 10.0f;

        public float EnergyValue
        {
            get { return _energyValue; }
            set { _energyValue = value; }
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

        private AvatarCore _avatar;

        public EnergyCollectible(Entity entity)
            :base(entity, "EnergyCollectible")
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
                    GatherEnergy();
                }
            }
            base.Update(gameTime);
        }

        private void GatherEnergy()
        {
            State = CollectibleState.Used;
            _avatar.EnergySystem.CurrentEnergyStock += _energyValue;
            if(entity.spritesheets != null)
            entity.spritesheets.ChangeAnimation(_outAnimation, true, 0, true, false, false);
        }
    }
}
