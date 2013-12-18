using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class FollowEntity : Component
    {
        private Entity _entityToFollow = null;
        private SpritesheetManager _spritesheetManager = null;


        public Entity EntityToFollow
        {
            get { return _entityToFollow; }
            set { _entityToFollow = value; }
        }
        
        public FollowEntity(Entity entity)
            :base(entity, "FollowEntity")
        {
        }

        public override void Init()
        {
            _spritesheetManager = entity.GetComponent <SpritesheetManager>();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            this.entity.transform.Position = _entityToFollow.transform.Position;
            base.Update(gameTime);
        }

    }
}
