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
            
            base.Update(gameTime);
        }
    }
}
