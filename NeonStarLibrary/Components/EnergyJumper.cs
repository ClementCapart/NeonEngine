using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public class EnergyJumper : EnergyComponent
    {
        #region Properties
        private Vector2 _jumperImpulse;

        public Vector2 JumperImpulse
        {
            get { return _jumperImpulse; }
            set { _jumperImpulse = value; }
        }

        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }
        #endregion

        private Entity _avatarEntity;

        public EnergyJumper(Entity entity)
            :base(entity)
        {
            Name = "EnergyJumper";
            RequiredComponents = new Type[] { typeof(Hitbox) };
        }

        public override void Init()
        {
            _avatarEntity = entity.GameWorld.GetEntityByName(_avatarName);
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            if(_powered)
            {
                if (_avatarEntity != null && _avatarEntity.hitboxes.Count > 0)
                {
                    if (_avatarEntity.hitboxes[0].hitboxRectangle.Intersects(entity.hitboxes[0].hitboxRectangle))
                    {
                        if (_avatarEntity.rigidbody != null)
                        {
                            _avatarEntity.rigidbody.body.LinearVelocity = Vector2.Zero;
                            _avatarEntity.rigidbody.body.ApplyLinearImpulse(_jumperImpulse);
                        
                        }
                    }
                }
            }
            base.Update(gameTime);
        }

    }
}
