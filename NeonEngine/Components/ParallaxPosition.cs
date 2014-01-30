using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Utils
{
    public class ParallaxPosition : Component
    {
        private Vector2 _parallaxForce;
        private Vector2 _startingPosition;


        public Vector2 ParallaxForce
        {
            get { return _parallaxForce; }
            set { _parallaxForce = value; }
        }

        public ParallaxPosition(Entity entity)
            :base(entity, "ParallaxPosition")
        {
        }

        public override void Init()
        {
            _startingPosition = entity.transform.Position;
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            entity.transform.Position = _startingPosition + entity.GameWorld.Camera.Position * _parallaxForce ;
            base.Update(gameTime);
        }
    }
}
