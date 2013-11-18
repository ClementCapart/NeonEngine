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
        public float alpha = 1.0f;
        bool change = false;
        public World nextScreen = null;

        public FarseerPhysics.Dynamics.World physicWorld;
        public LightingSystem lightingSystem;
        RenderTarget2D defferedDrawing;

        public RenderTarget2D ShadowCasters;
        ShadowmapResolver shadowmapResolver;
        QuadRenderComponent quadRender;


        PolygonRenderer _polygonRenderer;

        public Effect ScreenEffect;

        public RenderTarget2D screenShadows = new RenderTarget2D(Neon.graphicsDevice, Neon.ScreenWidth, Neon.ScreenHeight);

        Effect lightingEffect;

        public Texture2D graphicDressing;

        Vector2 _screenCenter;
        DebugViewXNA debugView;

        public Camera2D camera;
        public List<Entity> entities;
        public List<DrawableComponent> DrawableComponents;
        public List<DrawableComponent> HUDComponents;
        public List<Water> waterzones;
        public List<LightArea> lightAreas;
        public List<PathNodeList> NodeLists;

        public NeonPool<Hitbox> HitboxPool;
        public NeonPool<Particle> ParticlePool;
        public List<Hitbox> Hitboxes;
        public List<AnimatedSpecialEffect> SpecialEffects;

        public Level levelMap;
        public string levelFilePath;

        public bool Pause = false;

        public bool FirstUpdateWorld = true;
        public bool FirstUpdate = true;

        public Game game;

        public World(Game game)
        {
            Console.WriteLine(GetType().Name + " (World) loading...");
            Console.WriteLine("");
            Neon.world = this;
            this.game = game;
            physicWorld = new FarseerPhysics.Dynamics.World(new Vector2(0.0f, 9.8f));
            lightingSystem = new LightingSystem();
            debugView = new DebugViewXNA(physicWorld);
            debugView.RemoveFlags(DebugViewFlags.Controllers);
            debugView.RemoveFlags(DebugViewFlags.Joint);
            debugView.AppendFlags(DebugViewFlags.DebugPanel);
            debugView.LoadContent(game.GraphicsDevice, game.Content);

            defferedDrawing = new RenderTarget2D(Neon.graphicsDevice, Neon.ScreenWidth, Neon.ScreenHeight);
            ShadowCasters = new RenderTarget2D(Neon.graphicsDevice, Neon.ScreenWidth, Neon.ScreenHeight);
            lightingEffect = game.Content.Load<Effect>(@"Shaders\Lighting");

            _screenCenter = new Vector2(this.game.GraphicsDevice.Viewport.Width * 0.5f, this.game.GraphicsDevice.Viewport.Height * 0.5f);

            camera = new Camera2D();
            entities = new List<Entity>();
            waterzones = new List<Water>();
            DrawableComponents = new List<DrawableComponent>();
            HUDComponents = new List<DrawableComponent>();
            lightAreas = new List<LightArea>();
            HitboxPool = new NeonPool<Hitbox>(() => new Hitbox());
            ParticlePool = new NeonPool<Particle>(() => new Particle());
            EffectsManager.Initialize();
            Hitboxes = new List<Hitbox>();
            NodeLists = new List<PathNodeList>();
            SpecialEffects = new List<AnimatedSpecialEffect>();


            _polygonRenderer = new PolygonRenderer(Neon.graphicsDevice, Vector2.Zero);
            quadRender = new QuadRenderComponent(this.game);
            shadowmapResolver = new ShadowmapResolver(quadRender, ShadowmapSize.Size2048, ShadowmapSize.Size2048);
            shadowmapResolver.LoadContent(this.game.Content);
        }

        public void LoadLevel(Level level)
        {
            levelMap = level;
            levelFilePath = level.levelFilePath;
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
            Neon.Input.Update(camera);
  
            Neon.elapsedTime = gameTime.ElapsedGameTime.Milliseconds;
            if (!Pause)
            {
                
                foreach (Water w in waterzones)
                    w.Update(gameTime);
                for (int i = entities.Count - 1; i >= 0; i--)
                    entities[i].PreUpdate(gameTime);

                
                for (int i = entities.Count - 1; i >= 0; i--)
                    entities[i].Update(gameTime);

                for(int i = SpecialEffects.Count - 1; i >= 0; i--)
                    SpecialEffects[i].Update(gameTime);

                physicWorld.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
                              
                for (int i = entities.Count - 1; i >= 0; i--)
                    entities[i].PostUpdate(gameTime);
            }

            lightingSystem.ComposeLightMask(Neon.spriteBatch, this);
           


            DeferredDrawGame(Neon.spriteBatch);

            if (!change)
                alpha = Math.Max(alpha - 0.01f, 0.0f);
            else
            {
                if (alpha >= 1.0f)
                    Neon.world = nextScreen;
                alpha = Math.Min(alpha + 0.035f, 1.0f);
            }
            
            InputEngine();

            PreUpdate(gameTime);

            Update(gameTime);
            
            PostUpdate(gameTime);

            //Console.WriteLine((1000.0f / gameTime.ElapsedGameTime.TotalMilliseconds) + "FPS");
            Neon.Input.LastFrameState();
        }

        public virtual void PostUpdate(GameTime gameTime)
        {
        }

        public void DeferredDrawGame(SpriteBatch spriteBatch)
        {
            Neon.graphicsDevice.SetRenderTarget(defferedDrawing);
            Neon.graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            ManualDrawBackHUD(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.get_transformation(Neon.graphicsDevice));

            foreach (DrawableComponent dc in DrawableComponents)
                dc.Draw(spriteBatch);
            
            spriteBatch.End();
            foreach (Water w in waterzones)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                w.Draw(spriteBatch);
                spriteBatch.End();
            }
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.get_transformation(Neon.graphicsDevice));
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
            Neon.graphicsDevice.SetRenderTarget(null);

#region lightning & shadow system
            if (lightingSystem.LightingEnabled)
            {
                Neon.graphicsDevice.SetRenderTarget(ShadowCasters);
                Neon.graphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.get_transformation(Neon.graphicsDevice));

                foreach (DrawableComponent dc in DrawableComponents)
                    dc.Draw(spriteBatch);

                spriteBatch.End();

                foreach (LightArea la in lightAreas)
                {
                    la.BeginDrawingShadowCasters();
                    DrawCaster(la);
                    la.EndDrawingShadowCasters();
                    shadowmapResolver.ResolveShadows(la.RenderTarget, la.RenderTarget, la.LightPosition);
                }

                Neon.graphicsDevice.SetRenderTarget(screenShadows);
                Neon.graphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, camera.get_transformation(Neon.graphicsDevice));
                
                foreach (LightArea la in lightAreas)
                    spriteBatch.Draw(la.RenderTarget, la.LightPosition - la.LightAreaSize * 0.5f, la.color);

                spriteBatch.End();
                Neon.graphicsDevice.SetRenderTarget(null);
            }
#endregion
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (ScreenEffect != null)
            {
                foreach (EffectPass p in ScreenEffect.CurrentTechnique.Passes)
                {
                    p.Apply();
                    spriteBatch.Draw(defferedDrawing, Vector2.Zero, Color.White);
                }
            }
            else
                spriteBatch.Draw(defferedDrawing, Vector2.Zero, Color.White);


            spriteBatch.End();



            if (lightingSystem.LightingEnabled)
            {
                BlendState blendState = new BlendState();
                blendState.ColorSourceBlend = Blend.DestinationColor;
                blendState.ColorDestinationBlend = Blend.SourceColor;

                spriteBatch.Begin(SpriteSortMode.Immediate, blendState);
                spriteBatch.Draw(screenShadows, Vector2.Zero, Color.White);
                spriteBatch.End();

                spriteBatch.Begin();
                spriteBatch.Draw(ShadowCasters, Vector2.Zero, Color.White);
                spriteBatch.End();
            }
            

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            
            foreach (DrawableComponent hudc in HUDComponents)
                hudc.Draw(spriteBatch);
            
            ManualDrawHUD(spriteBatch);
            DrawDebugView(Neon.graphicsDevice);
            spriteBatch.Draw(AssetManager.GetTexture("neon_screen"), Vector2.Zero, Color.Lerp(Color.Transparent, Neon.fadeColor, alpha));
            spriteBatch.End();
        }

        private void DrawCaster(LightArea lightArea)
        {
            Neon.spriteBatch.Begin();
            Neon.spriteBatch.Draw(ShadowCasters, lightArea.ToRelativePosition(camera.Position - Neon.HalfScreen), Color.Black);
            Neon.spriteBatch.End();
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
            entities.Add(newEntity);
            Console.WriteLine("Add : " + newEntity.Name);
        }

        public virtual void RemoveEntity(Entity entity)
        {
            if(entities.Contains(entity))
                entities.Remove(entity);
            Console.WriteLine("Remove : " + entity.Name);
        }

        public void ChangeScreen(World nextScreen)
        {
            change = true;
            this.nextScreen = nextScreen;
        }

        private void DrawDebugView(GraphicsDevice device)
        {
            if (Neon.DebugViewEnabled)
            {
                Matrix projection = Matrix.CreateOrthographicOffCenter(
               0f,
               CoordinateConversion.screenToWorld(game.GraphicsDevice.Viewport.Width / camera.Zoom),
               CoordinateConversion.screenToWorld(game.GraphicsDevice.Viewport.Height / camera.Zoom), 0f, 0f,
               1f);
                Matrix view = Matrix.CreateTranslation(camera.Position.X / -100f, camera.Position.Y / -100f, 0f) * Matrix.CreateTranslation(_screenCenter.X / camera.Zoom / 100f, _screenCenter.Y / camera.Zoom / 100f, 0f);
                debugView.RenderDebugData(ref projection, ref view);
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
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                if (entities[i].Name == name)
                    return entities[i];
            }

            return null;
        }
    }
}
