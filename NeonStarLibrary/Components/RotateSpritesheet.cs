using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class RotateSpritesheet : Component
    {
        public Enemy EnemyComponent = null;

        public RotateSpritesheet(Entity entity)
            :base(entity, "RotateSpritesheet")
        {
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<Enemy>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(EnemyComponent != null)
            {
                if (EnemyComponent.State == EnemyState.Attack || EnemyComponent.State == EnemyState.Chase)
                {
                    if (EnemyComponent._threatArea.EntityFollowed != null)
                        entity.spritesheets.RotationOffset = Neon.utils.AngleBetween(EnemyComponent._threatArea.EntityFollowed.transform.Position, entity.transform.Position);
                }
            }
                
            base.Update(gameTime);
        }
    }
}
