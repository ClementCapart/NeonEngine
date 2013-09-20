using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeonEngine.Private
{
    public class Particle : Entity
    {
        public Vector2 Velocity;
        public float AngularVelocity;
        public float TimeToLive;

        public Particle(Texture2D texture, Vector2 Position, Vector2 Velocity, float Rotation, float AngularVelocity, float Scale, float TimeToLive, World containerWorld)
            : base(containerWorld)
        {
            transform.Position = Position;
            transform.Rotation = Rotation;
            transform.Scale = Scale;
            
            this.Velocity = Velocity;
            this.AngularVelocity = AngularVelocity;          
            this.TimeToLive = TimeToLive;
            
            graphic = AddComponent(new Graphic(texture, 0, this));
        }

        public Particle(SpriteSheetInfo spriteSheetInfo, Vector2 Position, Vector2 Velocity, float Rotation, float AngularVelocity, float Scale, float TimeToLive, World containerWorld)
            : base(containerWorld)
        {
             transform.Position = Position;
            transform.Rotation = Rotation;
            transform.Scale = Scale;
            
            this.Velocity = Velocity;
            this.AngularVelocity = AngularVelocity;          
            this.TimeToLive = TimeToLive;
            spritesheet = AddComponent(new SpriteSheet(spriteSheetInfo, 0, this));
        }

        public override void Update(GameTime gameTime)
        {
            TimeToLive -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            transform.Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            transform.Rotation += AngularVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }
}
