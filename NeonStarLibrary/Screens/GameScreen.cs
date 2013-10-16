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
        public bool MustFollowAvatar = true;

        public GameScreen(Game game)
            : base(game)
        {
            enemies = new List<Enemy>();
            
            LoadLevel(new Level(@"..\Data\Levels\Level_0-0", this, true));

            AttacksManager.LoadAttacks();
            camera.Bounded = true;
        }

        public override void Update(GameTime gameTime)
        {
            if(MustFollowAvatar)
                camera.SmoothFollow(entities.Where(e => e.Name == "LiOn").First());

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
