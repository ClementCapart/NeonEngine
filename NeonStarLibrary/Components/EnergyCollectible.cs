using NeonEngine;
using NeonStarLibrary.Components.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public class EnergyCollectible : Component
    {
        #region Properties

        private float _energyValue = 10.0f;

        public float EnergyValue
        {
            get { return _energyValue; }
            set { _energyValue = value; }
        }

        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        #endregion

        private AvatarCore _avatar;
        private bool _gathered = false;

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
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (entity.hitboxes[0] != null && _avatar != null && _avatar.entity.hitboxes[0] != null && !_gathered)
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
            _gathered = true;
            _avatar.EnergySystem.CurrentEnergyStock += _energyValue;
        }
    }
}
