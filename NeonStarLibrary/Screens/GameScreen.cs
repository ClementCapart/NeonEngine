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

namespace NeonStarLibrary
{
    public class GameScreen : World
    {
        public List<Enemy> enemies;


        public GameScreen(Game game)
            : base(game)
        {
            enemies = new List<Enemy>();
            
            LoadLevel(new Level(@"..\Data\Levels\Level_0-0", this, true));

            AttacksManager.LoadAttacks();

            camera.Position = new Vector2(-100, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (Neon.Input.Check(Buttons.RightThumbstickLeft))
                camera.Position -= new Vector2(10, 0);
            if (Neon.Input.Check(Buttons.RightThumbstickRight))
                camera.Position += new Vector2(10, 0);

            if (Neon.Input.Pressed(Buttons.Start))
                Pause = !Pause;
            base.Update(gameTime);
        }

        public virtual void ReloadLevel()
        {
            ChangeScreen(new GameScreen(game));
        }

        public override void ManualDrawBackHUD(SpriteBatch sb)
        {
            base.ManualDrawBackHUD(sb);
        }
    }
}
