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
        public static NeonPool<AnimatedSpecialEffect> EffectsPool = new NeonPool<AnimatedSpecialEffect>(() => new AnimatedSpecialEffect());

        static public AnimatedSpecialEffect GetEffect(SpriteSheetInfo spriteSheetInfo, Side side, Vector2 Position, float Rotation, Vector2 Offset, float layer)
        {
            AnimatedSpecialEffect animatedSpecialEffect = EffectsPool.GetAvailableItem();
            animatedSpecialEffect.containerWorld = Neon.world;
            animatedSpecialEffect.transform.Scale = 2.0f;

            SpriteSheet spriteSheet;

            if (animatedSpecialEffect.Components.Count > 1)
            {
                spriteSheet = animatedSpecialEffect.spriteSheet;
            }
            else
            {
                spriteSheet = new SpriteSheet(animatedSpecialEffect);
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
            spriteSheet.Tint = true;
            spriteSheet.Init();

            animatedSpecialEffect.transform.Position = Position;
            animatedSpecialEffect.transform.Rotation = Rotation;
            animatedSpecialEffect.transform.Position += side == Side.Right ? Offset : -Offset;

            Neon.world.SpecialEffects.Add(animatedSpecialEffect);
            return animatedSpecialEffect;
        }
    }
}
