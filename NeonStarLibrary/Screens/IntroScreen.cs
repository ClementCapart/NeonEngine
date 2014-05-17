using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using NeonEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace NeonStarLibrary
{
    public class IntroScreen : World, IDisposable
    {
        VideoPlayer videoPlayer;
        Video videoIntro;
        int Loop = 0;
        bool Started;


        public IntroScreen(Game game)
            : base(game)
        {
            videoIntro = game.Content.Load<Video>("Trailer");
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
                        videoPlayer.Play(videoIntro);
                        Loop = 1;
                    }

                }
                if (Neon.Input.Pressed(NeonStarInput.Start))
                {
                    if (Loop == 1)
                        videoPlayer.Stop();
                }
            }
            else
                if (Neon.Input.Pressed(NeonStarInput.Start))
                {
                    if(!Started)
                        Started = true;
                }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
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
