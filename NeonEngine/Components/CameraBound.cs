using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class CameraBound : Component
    {
        Side _boundSide;

        public Side BoundSide
        {
            get { return _boundSide; }
            set { _boundSide = value; }
        }

        public CameraBound(Entity entity)
            :base(entity, "CameraBound")
        {
        }

        public override void Init()
        {
            entity.containerWorld.camera.CameraBounds.Add(this);
            base.Init();
        }

        public override void Remove()
        {
            entity.containerWorld.camera.CameraBounds.Remove(this);
            base.Remove();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
