using System;
using System.Collections.Generic;
using System.Linq;
using NeonEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NeonStarEditor;
using NeonStarLibrary;

namespace NeonStar
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = true;
            IsFixedTimeStep = false;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            
            Neon.Start(this, graphics, spriteBatch, 1280, 720);
            Neon.Input.AssignCustomControls(typeof(NeonStarInput));
            Neon.NeonScripting.AddAssembly("NeonStarLibrary.dll");
            Neon.NeonScripting.CompileScripts();

            Neon.clearColor = Color.Black;
            #if DEBUG
            Neon.world = new EditorScreen(this, Neon.GraphicsDeviceManager);
            #else
            Neon.world = new GameScreen(this);
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            #endif
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {   

            Neon.world.UpdateWorld(gameTime);
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
                GraphicsDevice.Clear(Neon.clearColor);
                Neon.world.Draw(spriteBatch);
                base.Draw(gameTime);
        }
    }
}
