using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeonEngine
{
    public enum ParticlePattern
    {
        Linear,
        Sinusoidal,
        Curve,
    }

    public class Particle : DrawableComponent
    {
        public Vector2 Direction;

        public float StartingSpeed;
        public float EndingSpeed;
        private float _speed;

        public float AngularVelocity;
        
        public Vector2 Position;
        public float Angle;

        public float Duration;
        private float _duration;

        public float FadeInDelay;
        public float FadeOutDelay;

        public Texture2D Texture;
        public ParticlePattern ParticleMovement;
        
        public Color StartingColor;
        public Color EndingColor;

        public float StartingBrightness;
        public float EndingBrightness;
        private float _brightness;
        
        public float StartingOpacity;
        public float EndingOpacity;
        private float _opacity;
       

        public float StartingScale;
        public float EndingScale;

        public float Scale;

        public ParticleEmitter Emitter;

        public Particle()
            : base(1.0f ,null, "Particle")
        {  
        }

        public Particle(Entity entity)
            : base(1.0f, entity, "Particle")
        {
        }

        public override void Init()
        {
            _duration = 0;

            StartingBrightness = 0f;
            EndingBrightness = 0.2f;

            StartingColor = Color.Red;
            EndingColor = Color.Red;

            StartingOpacity = 0.7f;
            EndingOpacity = 1f;

            StartingScale = 1f;
            EndingScale = 2f;

            if (FadeInDelay > 0f)
            {
                _opacity = 0.0f;
            }

            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            if (_duration < Duration)
            {
                ManageOpacity(gameTime);
                ManageColor(gameTime);
                ManageBrightness(gameTime);
                ManageScale(gameTime);
                ManageSpeed(gameTime);

                ManageMovement(gameTime);

                _duration += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                this.Remove();
            }
            base.Update(gameTime);
        }

        private void ManageOpacity(GameTime gameTime)
        {
            if (FadeInDelay > 0f && _duration < FadeInDelay)
            {
                _opacity = (_duration / FadeInDelay) * StartingOpacity;
            }
            else if (_duration > Duration - FadeOutDelay)
            {
                _opacity = ((_duration - (Duration - FadeOutDelay) / FadeOutDelay) - 1) * -1 * EndingOpacity;
            }
            else
            {
                _opacity = StartingOpacity + ((_duration - FadeInDelay) / (Duration - FadeOutDelay - FadeInDelay)) * (EndingOpacity - StartingOpacity);
            }
        }

        private void ManageColor(GameTime gameTime)
        {
            TintColor = Color.Lerp(StartingColor, EndingColor, _duration / Duration);
        }

        private void ManageBrightness(GameTime gameTime)
        {
            _brightness = MathHelper.Lerp(StartingBrightness, EndingBrightness, _duration / Duration);
            TintColor = Color.Lerp(Color.White, TintColor, _brightness);
        }

        private void ManageScale(GameTime gameTime)
        {
            Scale = MathHelper.Lerp(StartingScale, EndingScale, _duration / Duration);
        }

        private void ManageSpeed(GameTime gameTime)
        {
            _speed = MathHelper.Lerp(StartingSpeed, EndingSpeed, _duration / Duration);
        }

        private void ManageMovement(GameTime gameTime)
        {
            switch(ParticleMovement)
            {
                case ParticlePattern.Linear:
                    Position += Direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;

                case ParticlePattern.Curve:
                    break;

                case ParticlePattern.Sinusoidal:
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, TintColor, Angle, new Vector2(Texture.Width / 2, Texture.Height / 2), Scale, SpriteEffects.None, Layer);
            base.Draw(spriteBatch);
        }

        public override void Remove()
        {
            Emitter.particles.Remove(this);
            Neon.world.ParticlePool.FlagAvailableItem(this);
            base.Remove();
        }
    }
}
