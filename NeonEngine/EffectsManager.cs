using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    static public class EffectsManager
    {
        public static NeonPool<AnimatedSpecialEffect> EffectsPool;

        public static void Initialize()
        {
            EffectsPool = new NeonPool<AnimatedSpecialEffect>(() => new AnimatedSpecialEffect());
        }

        static public AnimatedSpecialEffect GetEffect(SpriteSheetInfo spriteSheetInfo, Side side, Vector2 Position, float Rotation, Vector2 Offset, float scale, float layer, Entity entity = null)
        {
            AnimatedSpecialEffect animatedSpecialEffect = EffectsPool.GetAvailableItem();
            animatedSpecialEffect.containerWorld = Neon.World;
            animatedSpecialEffect.transform.Scale = scale;

            SpriteSheet spriteSheet;

            if (animatedSpecialEffect.Components.Count > 1)
            {
                spriteSheet = animatedSpecialEffect.spriteSheet;
                spriteSheet.RotationOffset = Rotation;
            }
            else
            {
                spriteSheet = new SpriteSheet(animatedSpecialEffect);
                spriteSheet.RotationOffset = Rotation;
                animatedSpecialEffect.AddComponent(spriteSheet);
                animatedSpecialEffect.spriteSheet = spriteSheet;
            }
          
            spriteSheet.spriteSheetInfo = spriteSheetInfo;
            spriteSheet.currentFrame = 0;
            spriteSheet.Layer = layer;
            spriteSheet.IsLooped = false;
            spriteSheet.IsFinished = false;
            if(Rotation == 0)
                spriteSheet.spriteEffects = side == Side.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteSheet.Active = true;
            //spriteSheet.Tint = true;
            //priteSheet.Offset = new Vector2(side == Side.Right ? Offset.X : -Offset.X, Offset.Y);
            spriteSheet.Init();
            spriteSheet.isPlaying = true;

            animatedSpecialEffect.transform.Position = Position + new Vector2(side == Side.Right ? Offset.X : -Offset.X, Offset.Y);
            animatedSpecialEffect.transform.Rotation = Rotation;

            if (entity != null)
            {
                FollowEntity followEntity = animatedSpecialEffect.GetComponent<FollowEntity>();
                if (followEntity != null)
                {
                    followEntity.EntityToFollow = entity;
                }
                else
                {
                    followEntity = new FollowEntity(animatedSpecialEffect);
                    followEntity.EntityToFollow = entity;
                    animatedSpecialEffect.AddComponent(followEntity);
                }
            }
            else
            {
                FollowEntity followEntity = animatedSpecialEffect.GetComponent<FollowEntity>();
                if (followEntity != null)
                    followEntity.Remove();
            }

            Neon.World.SpecialEffects.Add(animatedSpecialEffect);
            return animatedSpecialEffect;
        }
    }
}
