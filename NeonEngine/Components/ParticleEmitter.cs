using Microsoft.Xna.Framework;
using NeonEngine.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class ParticleEmitter : DrawableComponent
    {
        private Random random;
        private List<Particle> particles;
        private List<string> textures;
        private List<string> spritesheets;

        public float DelaySpawn = 0.0f, TimerSpawn = 0f;

        public ParticleEmitter(Entity entity)
            :base(DrawLayer.None, entity, "ParticleEmitter")
        {
            this.particles = new List<Particle>();
            this.spritesheets = new List<string>();
            this.textures = new List<string>();
            random = new Random();
            Init();
        }

        public override void Init()
        {
            spritesheets.Add("BulletEffect");

            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (DelaySpawn <= TimerSpawn)
            {
                int total = 3;
                for (int i = 0; i < total; i++)
                {
                    Particle p = InstantiateParticle();
                    entity.containerWorld.AddEntity(p);
                    particles.Add(p);
                }
                TimerSpawn = 0f;
            }
            else
            {
                TimerSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if(particles.Count > 0)
                for(int j = particles.Count - 1; j >= 0; j--)
                    if (particles[j].TimeToLive <= 0)
                    {
                        particles[j].Destroy();
                        particles.RemoveAt(j);
                    }
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private Particle InstantiateParticle()
        {
            SpriteSheetInfo ssi = AssetManager.GetSpriteSheet(spritesheets[random.Next(textures.Count)]);
            Vector2 velocity = new Vector2(
                100f * (float)(random.NextDouble() * 2 - 1),
                100f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 10f * (float)(random.NextDouble() * 2 - 1);

            float Scale = (float)random.NextDouble();
            float TimeToLive = 0.5f + random.Next(2);

            return new Particle(ssi, entity.transform.Position, velocity, angle, angularVelocity, Scale, TimeToLive, entity.containerWorld);
        }
    }
}
