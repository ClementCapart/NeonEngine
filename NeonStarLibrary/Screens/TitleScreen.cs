using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NeonEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace NeonStarLibrary
{
    public class TitleScreen : World
    {
        float StartOpacity = 1f;
        bool OpacityUp = false;

        public TitleScreen(Game game)
            :base(game)
        {
            MediaPlayer.Play(SoundManager.GetSong("TitleMusic"));
        }

        public override void Update(GameTime gameTime)
        {
            if (Neon.Input.Pressed(Buttons.Start))
            {
                ChangeScreen(new HoloRoom(game));
                MediaPlayer.Stop();
            }

            if (OpacityUp)
                if (StartOpacity < 1f)
                    StartOpacity += 0.01f;
                else
                    OpacityUp = false;
            else
                if (StartOpacity > 0.2f)
                    StartOpacity -= 0.01f;
                else
                    OpacityUp = true;


            base.Update(gameTime);
        }

        public override void ManualDrawHUD(SpriteBatch sb)
        {
            sb.Draw(AssetManager.GetTexture("TitleScreen"), Vector2.Zero, Color.White);
            sb.Draw(AssetManager.GetTexture("PressStart"), Vector2.Zero, Color.Lerp(Color.Transparent, Color.White, StartOpacity));
            base.ManualDrawHUD(sb);
        }
    }
}
