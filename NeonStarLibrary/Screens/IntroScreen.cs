using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeonEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace NeonStarLibrary
{
    public class IntroScreen : World, IDisposable
    {
        VideoPlayer videoPlayer;
        Video videoEngine;
        Video videoTeam;
        Video videoIntro;
        int Loop = 0;
        bool Started;


        public IntroScreen(Game game)
            : base(game)
        {
            videoEngine = game.Content.Load<Video>("NeonEngineLogo");
            videoTeam = game.Content.Load<Video>("ToxicRacoonLogo");
            videoIntro = game.Content.Load<Video>("Intro");
            videoPlayer = new VideoPlayer();
        }

        public override void Update(GameTime gameTime)
        {
            if (Started)
            {
                if (videoPlayer.State == MediaState.Stopped)
                {
                    if (Loop == 0)
                    {
                        videoPlayer.IsLooped = false;
                        videoPlayer.Play(videoEngine);
                        Loop = 1;
                    }
                    else if (Loop == 1)
                    {
                        videoPlayer.Play(videoTeam);
                        Loop = 2;
                    }
                    else if (Loop == 2)
                    {
                        videoPlayer.Play(videoIntro);
                        Loop = 3;
                    }
                    else
                        ChangeScreen(new TitleScreen(game));

                }
            }
            else
                if (Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Buttons.Start))
                    Started = true;

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Texture2D videoTexture = null;

            if (videoPlayer.State != MediaState.Stopped)
                videoTexture = videoPlayer.GetTexture();

            if (videoTexture != null)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(videoTexture, Vector2.Zero, Color.White);
                spriteBatch.End();
            }
            base.Draw(spriteBatch);
        }

        public void Dispose()
        {
            videoPlayer.Dispose();
        }
    }
}
