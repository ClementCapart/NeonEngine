using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine.Components.Graphics2D;
using NeonEngine.Components.Utils;
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

        static public AnimatedSpecialEffect GetEffect(SpriteSheetInfo spriteSheetInfo, Side side, Vector2 Position, float Rotation, Vector2 Offset, float scale, float layer, Entity entity = null, float duration = 0.0f, bool loop = false, bool HUD = false)
        {
            AnimatedSpecialEffect animatedSpecialEffect = EffectsPool.GetAvailableItem();
            animatedSpecialEffect.GameWorld = Neon.World;
            animatedSpecialEffect.transform.Scale = scale;
            animatedSpecialEffect.Duration = duration;

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
            spriteSheet.IsLooped = loop;
            spriteSheet.IsFinished = false;
            spriteSheet.CurrentSide = side;
            spriteSheet.Active = true;
            //spriteSheet.Tint = true;
            //priteSheet.Offset = new Vector2(side == Side.Right ? Offset.X : -Offset.X, Offset.Y);
            spriteSheet.Init();
            spriteSheet.isPlaying = true;

            animatedSpecialEffect.transform.Position = Position + new Vector2(side == Side.Right ? Offset.X : -Offset.X, Offset.Y);
            //animatedSpecialEffect.transform.Rotation = Rotation;

            spriteSheet.Remove();
            spriteSheet.IsHUD = HUD;
            animatedSpecialEffect.AddComponent(spriteSheet);
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
