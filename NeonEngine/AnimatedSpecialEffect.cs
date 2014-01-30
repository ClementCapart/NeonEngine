using NeonEngine.Components.Graphics2D;
using NeonEngine.Components.VisualFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class AnimatedSpecialEffect : Entity
    {
        public SpriteSheet spriteSheet;
        public ParticleEmitter particleEmitter;

        public AnimatedSpecialEffect(World containerWorld)
            :base(containerWorld)
        {
        }

        public AnimatedSpecialEffect()
            :base(null)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (spriteSheet.IsFinished)
            {
                this.Destroy();
            }
        }

        public override void Destroy()
        {
            EffectsManager.EffectsPool.FlagAvailableItem(this);
            spriteSheet.Active = false;
            GameWorld.SpecialEffects.Remove(this);
        }
    }
}
