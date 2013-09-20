using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using NeonEngine;
using Microsoft.Xna.Framework;
using NeonStarLibrary.Entities;

namespace NeonStarLibrary
{
    public class Teleport : Cosmetic
    {
        public bool IsActive = false;
        public bool CloseEnough = false;

        SpriteSheet spriteSheetOpening;
        Avatar avatar;

        public float ActivationRange = 100f;
        
        public Teleport(Vector2 Position, World containerWorld, Avatar avatar)
            : base(Position, "Teleporter", 0.6f, containerWorld)
        {
            spriteSheetOpening = new SpriteSheet(AssetManager.GetSpriteSheet("TeleporterOpening"), 0.6f, this);
            this.avatar = avatar;
            spriteSheet.isPlaying = false;
            spriteSheet.IsLooped = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                spriteSheet.isPlaying = true;
            }
            else
            {
                spriteSheet.isPlaying = false;
                spriteSheet.SetFrame(0);
            }
            if (Vector2.Distance(avatar.transform.Position, transform.Position) < ActivationRange)
                CloseEnough = true;
            else
                CloseEnough = false;

            if (CloseEnough && IsActive )
            {
                if (spriteSheet != spriteSheetOpening)
                {
                    spriteSheet.Remove();
                    spriteSheet = spriteSheetOpening;
                    AddComponent(spriteSheet);
                    spriteSheet.IsLooped = false;
                }
                if (Neon.Input.Pressed(Buttons.X))
                    containerWorld.ChangeScreen(new GameScreen(containerWorld.game));
            }

              
            base.Update(gameTime);
        }
    }
}
