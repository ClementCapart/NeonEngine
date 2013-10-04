using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace NeonEngine
{
    public class Feedback : Entity
    {
        public SpriteSheet animation;
        float graphicTimer = 0f;
        float graphicDelay = 1f;

        public bool notAnimated;

        public Feedback(string asset, Vector2 Position, float Layer, Side Direction, float opacity, World containerWorld, bool notAnimated = false)
            : base(containerWorld)
        {
            this.notAnimated = notAnimated;
            transform.Position = Position;
            if (notAnimated)
            {               
                //this.graphic = (Graphic)AddComponent(new Graphic(asset, 0f, this));
            }
            else
            {
                animation = (SpriteSheet)AddComponent(new SpriteSheet(this));
                animation.SpriteSheetTag = asset;
                animation.DrawLayer = Layer;
                animation.Init();
                animation.spriteEffects = Direction == Side.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                animation.IsLooped = false;
            }
            if (opacity != 1f)
            {
                animation.opacity = opacity;
            }
            

        }

        public override void Update(GameTime time)
        {
            if (!notAnimated)
            {
                if (!animation.isPlaying)
                    Destroy();
            }
            else
            {
                if (graphicTimer > graphicDelay)
                    Destroy();
                else
                    graphicTimer += (float)time.ElapsedGameTime.TotalSeconds;
            }
                
            base.Update(time);
        }
    }
}
