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

        static public AnimatedSpecialEffect GetEffect(SpriteSheetInfo spriteSheetInfo, Side side, Vector2 Position, float layer)
        {
            AnimatedSpecialEffect animatedSpecialEffect = EffectsPool.GetAvailableItem();
            animatedSpecialEffect.containerWorld = Neon.world;

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
            spriteSheet.spriteEffects = side == Side.Right ? SpriteEffects.None : SpriteEffects.FlipVertically;
            spriteSheet.Active = true;
            spriteSheet.Init();

            animatedSpecialEffect.transform.Position = Position;

            Neon.world.SpecialEffects.Add(animatedSpecialEffect);
            return animatedSpecialEffect;
        }
    }
}
