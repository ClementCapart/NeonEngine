using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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


        private ParticlePattern MovementType;

        public ParticlePattern MovementType1
        {
            get { return MovementType; }
            set { MovementType = value; }
        }

        private float _minimumDispersionAngle;

        public float MinimumDispersionAngle
        {
            get { return _minimumDispersionAngle; }
            set { _minimumDispersionAngle = value; }
        }

        private float _maximumDispersionAngle;

        public float MaximumDispersionAngle
        {
            get { return _maximumDispersionAngle; }
            set { _maximumDispersionAngle = value; }
        }

        private float _mininimumSpeed;

        public float MininimumSpeed
        {
            get { return _mininimumSpeed; }
            set { _mininimumSpeed = value; }
        }

        private float _maximumSpeed;

        public float MaximumSpeed
        {
            get { return _maximumSpeed; }
            set { _maximumSpeed = value; }
        }

        private float _minimumDuration;

        public float MinimumDuration
        {
            get { return _minimumDuration; }
            set { _minimumDuration = value; }
        }

        private float _maximumDuration;

        public float MaximumDuration
        {
            get { return _maximumDuration; }
            set { _maximumDuration = value; }
        }

        private int _maximumSpawnPerTick;

        public int MaximumSpawnPerTick
        {
            get { return _maximumSpawnPerTick; }
            set { _maximumSpawnPerTick = value; }
        }

        private int _minimumSpawnPerTick;

        public int MinimumSpawnPerTick
        {
            get { return _minimumSpawnPerTick; }
            set { _minimumSpawnPerTick = value; }
        }

        private float _minimumFadeInTime;

        public float MinimumFadeInTime
        {
            get { return _minimumFadeInTime; }
            set { _minimumFadeInTime = value; }
        }

        private float _maximumFadeInTime;

        public float MaximumFadeInTime
        {
            get { return _maximumFadeInTime; }
            set { _maximumFadeInTime = value; }
        }

        private float _minimumFadeOutTime;

        public float MinimumFadeOutTime
        {
            get { return _minimumFadeOutTime; }
            set { _minimumFadeOutTime = value; }
        }

        private float _maximumFadeOutTime;

        public float MaximumFadeOutTime
        {
            get { return _maximumFadeOutTime; }
            set { _maximumFadeOutTime = value; }
        }

        private float _spawnRate;

        public float SpawnRate
        {
            get { return _spawnRate; }
            set { _spawnRate = value; }
        }

        private float _minimumAngle;

        public float MinimumAngle
        {
            get { return _minimumAngle; }
            set { _minimumAngle = value; }
        }

        private float _maximumAngle;

        public float MaximumAngle
        {
            get { return _maximumAngle; }
            set { _maximumAngle = value; }
        }

        private float _minimumOpacity;

        public float MinimumOpacity
        {
            get { return _minimumOpacity; }
            set { _minimumOpacity = value; }
        }

        private float _maximumOpacity;

        public float MaximumOpacity
        {
            get { return _maximumOpacity; }
            set { _maximumOpacity = value; }
        }

        Texture2D _particleTexture;
        
        private string _graphicTag;

        public string GraphicTag
        {
            get { return _graphicTag; }
            set 
            { 
                _graphicTag = value;
                _particleTexture = AssetManager.GetTexture(value);
            }
        }

        public bool Active = true;

        public ParticleEmitter(Entity entity)
            :base(0, entity, "ParticleEmitter")
        {
            particles = new List<Particle>();
           
            random = new Random();
            Init();
        }

        public override void Init()
        {
            particles.Clear();
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            InstantiateParticle();
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Remove()
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Remove();
                particles.RemoveAt(1);
            }
            base.Remove();
        }

        private Particle InstantiateParticle()
        {
            Particle p = Neon.world.ParticlePool.GetAvailableItem();

            p.Texture = _particleTexture;
            float randomAngle = random.Next((int)_minimumDispersionAngle, (int)_maximumDispersionAngle);
            p.Direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(randomAngle)), -(float)(Math.Sin(randomAngle)));
            //p.Duration = (float)random.NextDouble() * 
            p.ParticleMovement = MovementType;
            p.FadeInDelay = 1f;
            p.FadeOutDelay = 1f;
            p.Position = Vector2.Zero;

            p.StartingBrightness = 0f;
            p.EndingBrightness = 0.2f;

            p.StartingColor = Color.Red;
            p.EndingColor = Color.Red;

            p.StartingOpacity = 0.7f;
            p.EndingOpacity = 1f;

            p.StartingScale = 1f;
            p.EndingScale = 2f;

            p.Init();

            return p;
        }
    }
}
