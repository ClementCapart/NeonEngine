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

        public Feedback(string asset, Vector2 Position, float Layer, SideDirection Direction, float opacity, World containerWorld, bool notAnimated = false)
            : base(containerWorld)
        {
            this.notAnimated = notAnimated;
            this.transform.Position = Position;
            if (notAnimated)
            {               
                //this.graphic = (Graphic)AddComponent(new Graphic(asset, 0f, this));
            }
            else
            {
                this.animation = (SpriteSheet)AddComponent(new SpriteSheet(this));
                animation.SpriteSheetTag = asset;
                animation.DrawLayer = Layer;
                this.animation.Init();
                this.animation.spriteEffects = Direction == SideDirection.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                this.animation.IsLooped = false;
            }
            if (opacity != 1f)
            {
                this.animation.opacity = opacity;
            }
            

        }

        public override void Update(GameTime time)
        {
            if (!notAnimated)
            {
                if (!animation.isPlaying)
                    this.Destroy();
            }
            else
            {
                if (graphicTimer > graphicDelay)
                    this.Destroy();
                else
                    graphicTimer += (float)time.ElapsedGameTime.TotalSeconds;
            }
                
            base.Update(time);
        }
    }
}
