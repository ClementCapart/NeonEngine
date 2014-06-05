using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Utils
{
    public class RotateEntity : Component
    {
        private float _rotationSpeed = 60.0f;

        public float RotationSpeed
        {
            get { return _rotationSpeed; }
            set { _rotationSpeed = value; }
        }

        public RotateEntity(Entity entity)
            :base(entity, "RotateEntity")
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            entity.transform.Rotation += _rotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }


    }
}
