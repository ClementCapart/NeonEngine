using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;

namespace NeonEngine.Components.VisualFX
{
    public class ParticleEmitter : DrawableComponent
    {
        private Random random;

        public List<Particle> particles;

        private MovePattern _movementType = MovePattern.Linear;

        public MovePattern MovementType
        {
            get { return _movementType; }
            set { _movementType = value; }
        }

        private float _minimumDispersionAngle = -90;

        public float MinimumDispersionAngle
        {
            get { return _minimumDispersionAngle; }
            set { _minimumDispersionAngle = value; }
        }

        private float _maximumDispersionAngle = 90;

        public float MaximumDispersionAngle
        {
            get { return _maximumDispersionAngle; }
            set { _maximumDispersionAngle = value; }
        }

        private float _minimumStartingSpeed = 100;

        public float MininimumStartingSpeed
        {
            get { return _minimumStartingSpeed; }
            set { _minimumStartingSpeed = value; }
        }

        private float _maximumStartingSpeed = 100;

        public float MaximumStartingSpeed
        {
            get { return _maximumEndingSpeed; }
            set { _maximumEndingSpeed = value; }
        }

        private float _minimumEndingSpeed = 100;

        public float MinimumEndingSpeed
        {
            get { return _minimumEndingSpeed; }
            set { _minimumEndingSpeed = value; }
        }

        private float _maximumEndingSpeed = 100;

        public float MaximumEndingSpeed
        {
            get { return _maximumEndingSpeed; }
            set { _maximumEndingSpeed = value; }
        }     

        private float _minimumDuration = 2.0f;

        public float MinimumDuration
        {
            get { return _minimumDuration; }
            set { _minimumDuration = value; }
        }

        private float _maximumDuration = 3.0f;

        public float MaximumDuration
        {
            get { return _maximumDuration; }
            set { _maximumDuration = value; }
        }

        private float _maximumSpawnPerTick = 3.0f;

        public float MaximumSpawnPerTick
        {
            get { return _maximumSpawnPerTick; }
            set { _maximumSpawnPerTick = value; }
        }

        private float _minimumSpawnPerTick = 1.0f;

        public float MinimumSpawnPerTick
        {
            get { return _minimumSpawnPerTick; }
            set { _minimumSpawnPerTick = value; }
        }

        private float _minimumFadeInTime = 0.4f;

        public float MinimumFadeInTime
        {
            get { return _minimumFadeInTime; }
            set { _minimumFadeInTime = value; }
        }

        private float _maximumFadeInTime = 0.4f;

        public float MaximumFadeInTime
        {
            get { return _maximumFadeInTime; }
            set { _maximumFadeInTime = value; }
        }

        private float _minimumFadeOutTime = 0.3f;

        public float MinimumFadeOutTime
        {
            get { return _minimumFadeOutTime; }
            set { _minimumFadeOutTime = value; }
        }

        private float _maximumFadeOutTime = 0.3f;

        public float MaximumFadeOutTime
        {
            get { return _maximumFadeOutTime; }
            set { _maximumFadeOutTime = value; }
        }

        private float _minimumSpawnRate = 0.6f;

        public float MinimumSpawnRate
        {
            get { return _minimumSpawnRate; }
            set { _minimumSpawnRate = value; }
        }

        private float _maximumSpawnRate = 1.0f;

        public float MaximumSpawnRate
        {
            get { return _maximumSpawnRate; }
            set { _maximumSpawnRate = value; }
        }

        private float _minimumOpacity = 0.7f;

        public float MinimumOpacity
        {
            get { return _minimumOpacity; }
            set { _minimumOpacity = value; }
        }

        private float _maximumOpacity = 1f;

        public float MaximumOpacity
        {
            get { return _maximumOpacity; }
            set { _maximumOpacity = value; }
        }

        private float _minimumStartingBrightness = 1f;

        public float MinimumStartingBrightness
        {
            get { return _minimumStartingBrightness; }
            set { _minimumStartingBrightness = value; }
        }
        private float _maximumStartingBrightness = 1f;

        public float MaximumStartingBrightness
        {
            get { return _maximumStartingBrightness; }
            set { _maximumStartingBrightness = value; }
        }
        private float _minimumEndingBrightness = 1f;

        public float MinimumEndingBrightness
        {
            get { return _minimumEndingBrightness; }
            set { _minimumEndingBrightness = value; }
        }
        private float _maximumEndingBrightness = 1f;

        public float MaximumEndingBrightness
        {
            get { return _maximumEndingBrightness; }
            set { _maximumEndingBrightness = value; }
        }

        private Color _startingColor = Color.White;

        public Color StartingColor
        {
            get { return _startingColor; }
            set { _startingColor = value; }
        }
        private Color _endingColor = Color.White;

        public Color EndingColor
        {
            get { return _endingColor; }
            set { _endingColor = value; }
        }

        private float _minimumStartingOpacity = 0.5f;

        public float MinimumStartingOpacity
        {
            get { return _minimumStartingOpacity; }
            set { _minimumStartingOpacity = value; }
        }

        private float _maximumStartingOpacity = 0.5f;

        public float MaximumStartingOpacity
        {
            get { return _maximumStartingOpacity; }
            set { _maximumStartingOpacity = value; }
        }

        private float _minimumEndingOpacity = 1.0f;

        public float MinimumEndingOpacity
        {
            get { return _minimumEndingOpacity; }
            set { _minimumEndingOpacity = value; }
        }
        private float _maximumEndingOpacity = 1.0f;

        public float MaximumEndingOpacity
        {
            get { return _maximumEndingOpacity; }
            set { _maximumEndingOpacity = value; }
        }

        private float _minimumStartingScale = 0.7f;

        public float MinimumStartingScale
        {
            get { return _minimumStartingScale; }
            set { _minimumStartingScale = value; }
        }
        private float _maximumStartingScale = 1.0f;

        public float MaximumStartingScale
        {
            get { return _maximumStartingScale; }
            set { _maximumStartingScale = value; }
        }

        private float _minimumEndingScale = 1.0f;

        public float MinimumEndingScale
        {
            get { return _minimumEndingScale; }
            set { _minimumEndingScale = value; }
        }

        private float _maximumEndingScale = 1.0f;

        public float MaximumEndingScale
        {
            get { return _maximumEndingScale; }
            set { _maximumEndingScale = value; }
        }

        public float ParticleLayer
        {
            get { return this.Layer; }
            set { Layer = value; }
        }

        Texture2D _particleTexture;
        
        private string _graphicTag = "";

        public string GraphicTag
        {
            get { return _graphicTag; }
            set 
            { 
                _graphicTag = value;
                _particleTexture = AssetManager.GetTexture(value);
            }
        }

        SpriteSheetInfo _spriteSheetInfo;

        private string _spriteSheetTag = "";

        public string SpriteSheetTag
        {
            get { return _spriteSheetTag; }
            set 
            {
                _spriteSheetTag = value;
                _spriteSheetInfo = AssetManager.GetSpriteSheet(SpriteSheetTag);
            }
        }

        private bool _useSpriteSheet;

        public bool UseSpriteSheet
        {
            get { return _useSpriteSheet; }
            set 
            { 
                _useSpriteSheet = value; 
            }
        }
        private bool _loop;

        public bool Loop
        {
            get { return _loop; }
            set { _loop = value; }
        }
        private bool _startAtRandomFrame;

        public bool StartAtRandomFrame
        {
            get { return _startAtRandomFrame; }
            set { _startAtRandomFrame = value; }
        }

        public bool Active = true;
        private float _currentSpawnRate = 0.0f;

        private float _SpawnTimer = 0.0f;

        public ParticleEmitter(Entity entity)
            :base(0, entity, "ParticleEmitter")
        {
            particles = new List<Particle>();
            random = Neon.Utils.CommonRandom;
        }

        public override void Init()
        {
            particles.Clear();
            _currentSpawnRate = (float)Neon.Utils.GetRandomNumber(_minimumSpawnRate, _maximumSpawnRate);
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            if(Active)
            {
                if (_SpawnTimer < _currentSpawnRate)
                {
                    _SpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    _SpawnTimer = 0.0f;

                    int particlesToSpawn = random.Next((int)_minimumSpawnPerTick, (int)_maximumSpawnPerTick);
                    for (int i = 0; i < particlesToSpawn; i++)
                    {
                        particles.Add(InstantiateParticle());
                    }
                }
            }

            for (int i = particles.Count - 1; i >= 0; i-- )
            {
                particles[i].Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in particles)
            {
                p.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }

        public override void Remove()
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles.RemoveAt(i);
            }
            base.Remove();
        }

        private Particle InstantiateParticle()
        {
            Particle p = entity.GameWorld.ParticlePool.GetAvailableItem();

            if (UseSpriteSheet)
            {
                p.spriteSheet = new SpriteSheet(_spriteSheetInfo, Layer, p);
                if(this.StartAtRandomFrame)
                    p.spriteSheet.SetFrame(random.Next(0, _spriteSheetInfo.FrameCount));
                p.spriteSheet.IsLooped = this.Loop;
                p.spriteSheet.Init();
            }
            else
            {
                p.Texture = _particleTexture;
            }
            
            float randomAngle = (float)Neon.Utils.GetRandomNumber(_minimumDispersionAngle, _maximumDispersionAngle, random);
            p.Direction = new Vector2((float)Math.Cos(randomAngle), -(float)(Math.Sin(randomAngle)));
            p.Duration = _minimumDuration + (float)random.NextDouble() * (_maximumDuration - _minimumDuration);
            p.ParticleMovement = _movementType;
            p.FadeInDelay = _minimumFadeInTime + (float)random.NextDouble() * (_maximumFadeInTime - _minimumFadeInTime);
            p.FadeOutDelay = _minimumFadeOutTime + (float)random.NextDouble() * (_maximumFadeOutTime - _minimumFadeOutTime);
            p.Position = entity.transform.Position;
            p.Layer = ParticleLayer;

            p.StartingBrightness = _minimumStartingBrightness + (float)random.NextDouble() * (_maximumStartingBrightness - _minimumStartingBrightness);
            p.EndingBrightness = _minimumEndingBrightness + (float)random.NextDouble() * (_maximumEndingBrightness - _maximumEndingBrightness);

            p.StartingColor = _startingColor;
            p.EndingColor = _endingColor;

            p.StartingOpacity = _minimumStartingOpacity + (float)random.NextDouble() * (_maximumStartingOpacity - _minimumStartingOpacity);
            p.EndingOpacity = _minimumEndingOpacity + (float)random.NextDouble() * (_maximumEndingOpacity - _minimumEndingOpacity);
            p.StartingSpeed = _minimumStartingSpeed + (float)random.NextDouble() * (_maximumStartingSpeed - _maximumStartingSpeed);
            p.EndingSpeed = _minimumEndingSpeed + (float)random.NextDouble() * (_maximumEndingSpeed - _maximumEndingSpeed);
            p.StartingScale = _minimumStartingScale + (float)random.NextDouble() * (_maximumStartingScale - _minimumStartingScale);
            p.EndingScale = _minimumEndingScale + (float)random.NextDouble() * (_maximumEndingScale - _minimumEndingScale);
            p.entity = this.entity;
            p.Emitter = this;
            p.Init();

            _currentSpawnRate = (float)Neon.Utils.GetRandomNumber(_minimumSpawnRate, _maximumSpawnRate);

            return p;
        }
    }
}
