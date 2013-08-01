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
using NeonStarLibrary.Entities;

namespace NeonStarLibrary
{
    public class GameScreen : World
    {
        public Avatar avatar;
        public List<Enemy> enemies;


        public GameScreen(Game game)
            : base(game)
        {
            this.enemies = new List<Enemy>();
            this.LoadLevel(new Level(@"Levels\Level_0-0", this, true));
            avatar = new Avatar(this, new Vector2(-550, 200));
            avatar.Name = "Li-On";
            AddEntity(avatar);
            this.camera.Position = new Vector2(-100, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (Neon.Input.Check(Buttons.RightThumbstickLeft))
                this.camera.Position -= new Vector2(10, 0);
            if (Neon.Input.Check(Buttons.RightThumbstickRight))
                this.camera.Position += new Vector2(10, 0);

            if (Neon.Input.Pressed(Buttons.Start))
                Pause = !Pause;
            if (avatar.LifePoints <= 0)
            {
                avatar.LifePoints = 7;
                ReloadLevel();
            }
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
