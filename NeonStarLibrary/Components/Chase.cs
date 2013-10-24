using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class Chase : Component
    {
        public Enemy EnemyComponent;

        private float _speed;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }


        public Chase(Entity entity)
            :base(entity, "Chase")
        {
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<Enemy>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (EnemyComponent.State == EnemyState.Chase)
            {
                if (EnemyComponent._threatArea.EntityFollowed.transform.Position.X < this.entity.transform.Position.X)
                {
                    this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(-_speed, this.entity.rigidbody.body.LinearVelocity.Y);
                }
                else
                {
                    this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(_speed, this.entity.rigidbody.body.LinearVelocity.Y);
                }
            }
            base.Update(gameTime);
        }
    }
}
