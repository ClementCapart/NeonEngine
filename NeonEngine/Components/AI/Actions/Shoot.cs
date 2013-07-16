using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.AI
{
    public class Shoot : AIAction
    {
        float _Delay;
        public float Delay
        {
            get { return _Delay; }
            set { _Delay = value; }
        }

        float _BulletSpeed;
        public float BulletSpeed
        {
            get { return _BulletSpeed; }
            set { _BulletSpeed = value; }
        }

        public Shoot()
            :base("Shoot")
        {

        }

        public override void Act(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }

    }
}
