using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using NeonEngine.Private;

namespace NeonEngine
{
    public abstract class World
    {
        public float Alpha = 1.0f;
        bool _change = false;
        public World NextScreen = null;

        public string LevelGroupName = "";
        public string LevelName = "";

        public FarseerPhysics.Dynamics.World PhysicWorld;
        RenderTarget2D _defferedDrawing;

        PolygonRenderer _polygonRenderer;

        public Effect ScreenEffect;

        Vector2 _screenCenter;
        DebugViewXNA _debugView;

        public Camera2D Camera;
        public List<Entity> Entities;
        public List<DrawableComponent> DrawableComponents;
        public List<DrawableComponent> HUDComponents;
        public List<Water> Waterzones;
        public List<LightArea> LightAreas;
        public List<PathNodeList> NodeLists;
        public List<SpawnPoint> SpawnPoints;

        public NeonPool<Hitbox> HitboxPool;
        public NeonPool<Particle> ParticlePool;
        public List<Hitbox> Hitboxes;
        public List<AnimatedSpecialEffect> SpecialEffects;

        public Level LevelMap;
        public string LevelFilePath;

        public bool Pause = false;

        public bool FirstUpdateWorld = true;
        public bool FirstUpdate = true;

        public Game game;

        public World(Game game)
        {
            Console.WriteLine(GetType().Name + " (World) loading...");
            Console.WriteLine("");
            Neon.World = this;
            this.game = game;
            PhysicWorld = new FarseerPhysics.Dynamics.World(new Vector2(0.0f, 9.8f));

            _defferedDrawing = new RenderTarget2D(Neon.GraphicsDevice, Neon.ScreenWidth, Neon.ScreenHeight);

            _debugView = new DebugViewXNA(PhysicWorld);
            _debugView.RemoveFlags(DebugViewFlags.Controllers);
            _debugView.RemoveFlags(DebugViewFlags.Joint);
            _debugView.AppendFlags(DebugViewFlags.DebugPanel);
            _debugView.LoadContent(game.GraphicsDevice, game.Content);

            _screenCenter = new Vector2(this.game.GraphicsDevice.Viewport.Width * 0.5f, this.game.GraphicsDevice.Viewport.Height * 0.5f);

            Camera = new Camera2D();
            Entities = new List<Entity>();
            Waterzones = new List<Water>();
            DrawableComponents = new List<DrawableComponent>();
            HUDComponents = new List<DrawableComponent>();
            LightAreas = new List<LightArea>();
            HitboxPool = new NeonPool<Hitbox>(() => new Hitbox());
            ParticlePool = new NeonPool<Particle>(() => new Particle());
            EffectsManager.Initialize();
            Hitboxes = new List<Hitbox>();
            NodeLists = new List<PathNodeList>();
            SpawnPoints = new List<SpawnPoint>();
            SpecialEffects = new List<AnimatedSpecialEffect>();

            _polygonRenderer = new PolygonRenderer(Neon.GraphicsDevice, Vector2.Zero);
        }

        public void LoadLevel(Level level)
        {
            LevelMap = level;
        }

        public virtual void PreUpdate(GameTime gameTime)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            if (FirstUpdate)
            {
                FirstUpdate = false;
                Console.WriteLine("-----------------------------------------------------------------------------");
                Console.WriteLine("");
                Console.WriteLine("");
            }
        }

        public void UpdateWorld(GameTime gameTime)
        {
            if (FirstUpdateWorld)
            {
                FirstUpdateWorld = false;
                Console.WriteLine("");
                Console.WriteLine(GetType().Name + " (World) loaded !");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            Neon.Input.Update(Camera);
  
            Neon.ElapsedTime = gameTime.ElapsedGameTime.Milliseconds;

            if(!Pause)
                for (int i = Entities.Count - 1; i >= 0; i--)
                    Entities[i].PreUpdate(gameTime);

            PreUpdate(gameTime);

            if(!Pause)
                for (int i = Entities.Count - 1; i >= 0; i--)
                    Entities[i].Update(gameTime);

            Update(gameTime);

            if(!Pause)
                for(int i = SpecialEffects.Count - 1; i >= 0; i--)
                    SpecialEffects[i].Update(gameTime);

            if(!Pause)
                PhysicWorld.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
                    
            if(!Pause)
                for (int i = Entities.Count - 1; i >= 0; i--)
                    Entities[i].PostUpdate(gameTime);

            PostUpdate(gameTime);

            if (!Pause)
                for (int i = Entities.Count - 1; i >= 0; i--)
                    Entities[i].FinalUpdate(gameTime);

            FinalUpdate(gameTime);

            DeferredDrawGame(Neon.SpriteBatch);

            if (!_change)
                Alpha = Math.Max(Alpha - 0.01f, 0.0f);
            else
            {
                if (Alpha >= 1.0f)
                    Neon.World = NextScreen;
                Alpha = Math.Min(Alpha + 0.035f, 1.0f);
            }
            
            
            InputEngine();

            //Console.WriteLine((1000.0f / gameTime.ElapsedGameTime.TotalMilliseconds) + "FPS");
            Neon.Input.LastFrameState();
        }

        public virtual void PostUpdate(GameTime gameTime)
        {
        }

        public virtual void FinalUpdate(GameTime gameTime)
        {
        }

        public void DeferredDrawGame(SpriteBatch spriteBatch)
        {
            Neon.GraphicsDevice.SetRenderTarget(_defferedDrawing);
            Neon.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            ManualDrawBackHUD(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.get_transformation(Neon.GraphicsDevice));

            foreach (DrawableComponent dc in DrawableComponents.OrderBy(dc => dc.Layer))
                dc.Draw(spriteBatch);
            
            spriteBatch.End();
            foreach (Water w in Waterzones)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                w.Draw(spriteBatch);
                spriteBatch.End();
            }
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Camera.get_transformation(Neon.GraphicsDevice));
            if (Neon.DrawHitboxes)
            {
                foreach (Hitbox hb in Hitboxes)
                {
                    switch (hb.Type)
                    {
                        case HitboxType.Main:
                            _polygonRenderer.Color = Color.Green;
                            break;

                        case HitboxType.Hit:
                            _polygonRenderer.Color = Color.Red;
                            break;

                        case HitboxType.None:
                            _polygonRenderer.Color = Color.White;
                            break;

                        case HitboxType.Bullet:
                            _polygonRenderer.Color = Color.LightPink;
                            break;

                        case HitboxType.Invincible:
                            _polygonRenderer.Color = Color.LightBlue;
                            break;

                        case HitboxType.Solid:
                            _polygonRenderer.Color = Color.Violet;
                            break;

                        case HitboxType.OneWay:
                            _polygonRenderer.Color = Color.Purple;
                            break;

                        case HitboxType.Trigger:
                            _polygonRenderer.Color = Color.SkyBlue;
                            break;
                    }
                    _polygonRenderer.vectors = hb.vectors;
                    _polygonRenderer.Position = hb.Center;
                    _polygonRenderer.Draw(spriteBatch);
                }
            }
            
            ManualDrawGame(spriteBatch);
            spriteBatch.End();
            Neon.GraphicsDevice.SetRenderTarget(null);
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (ScreenEffect != null)
            {
                foreach (EffectPass p in ScreenEffect.CurrentTechnique.Passes)
                {
                    p.Apply();
                    spriteBatch.Draw(_defferedDrawing, Vector2.Zero, Color.White);
                }
            }
            else
                spriteBatch.Draw(_defferedDrawing, Vector2.Zero, Color.White);


            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
            
            foreach (DrawableComponent hudc in HUDComponents)
                hudc.Draw(spriteBatch);
            
            ManualDrawHUD(spriteBatch);
            DrawDebugView(Neon.GraphicsDevice);
            
            spriteBatch.Draw(AssetManager.GetTexture("neon_screen"), Vector2.Zero, Color.Lerp(Color.Transparent, Neon.FadeColor, Alpha));
            spriteBatch.End();
        }

        public virtual void ManualDrawBackHUD(SpriteBatch sb)
        {
        }
        public virtual void ManualDrawHUD(SpriteBatch sb)
        {
        }

        public virtual void ManualDrawGame(SpriteBatch spriteBatch)
        {
        }

        public virtual void AddEntity(Entity newEntity)
        {
            Entities.Add(newEntity);
            Console.WriteLine("Add : " + newEntity.Name);
        }

        public virtual void RemoveEntity(Entity entity)
        {
            if(Entities.Contains(entity))
                Entities.Remove(entity);
            Console.WriteLine("Remove : " + entity.Name);
        }

        public virtual void ChangeScreen(World nextScreen)
        {
            _change = true;
            this.NextScreen = nextScreen;
        }

        private void DrawDebugView(GraphicsDevice device)
        {
            if (Neon.DebugViewEnabled)
            {
                Matrix projection = Matrix.CreateOrthographicOffCenter(
               0f,
               CoordinateConversion.screenToWorld(game.GraphicsDevice.Viewport.Width / Camera.Zoom),
               CoordinateConversion.screenToWorld(game.GraphicsDevice.Viewport.Height / Camera.Zoom), 0f, 0f,
               1f);
                Matrix view = Matrix.CreateTranslation(Camera.Position.X / -100f, Camera.Position.Y / -100f, 0f) * Matrix.CreateTranslation(_screenCenter.X / Camera.Zoom / 100f, _screenCenter.Y / Camera.Zoom / 100f, 0f);
                _debugView.RenderDebugData(ref projection, ref view);
            }
        }

        private void InputEngine()
        {
            if (Neon.Input.Pressed(Keys.F1))
                Neon.DebugViewEnabled = !Neon.DebugViewEnabled;
            if (Neon.Input.Pressed(Keys.F2))
                Neon.DrawHitboxes = !Neon.DrawHitboxes;
        }

        public Entity GetEntityByName(string name)
        {
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                if (Entities[i].Name == name)
                    return Entities[i];
            }

            return null;
        }
    }
}
