using Microsoft.Xna.Framework;
using NeonEngine;
using NeonStarLibrary.Components.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Utils
{
    public class RotateSpritesheet : Component
    {
        public EnemyCore EnemyComponent = null;

        public RotateSpritesheet(Entity entity)
            :base(entity, "RotateSpritesheet")
        {

        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<EnemyCore>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(EnemyComponent != null)
            {
                if (EnemyComponent.State == EnemyState.Attacking || EnemyComponent.State == EnemyState.Chase)
                {
                    if (EnemyComponent.Attack != null)
                    {
                        if (EnemyComponent.Attack.EntityToAttack != null)
                        {
                            entity.spritesheets.RotationOffset = MathHelper.ToRadians((int)(MathHelper.ToDegrees(Neon.Utils.AngleBetween(EnemyComponent.Attack.EntityToAttack.transform.Position, entity.transform.Position)) - 180) % 360);
                            if (entity.spritesheets.RotationOffset < -Math.PI / 2)
                                entity.spritesheets.ChangeSide(Side.Left);
                            else
                                entity.spritesheets.ChangeSide(Side.Right);
                            if (entity.spritesheets.CurrentSide == Side.Left)
                            {
                                entity.spritesheets.RotationOffset += (float)Math.PI;
                            }
                        }
                    }
                }
            }
                
            base.Update(gameTime);
        }
    }
}
