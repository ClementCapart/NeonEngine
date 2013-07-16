using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            : base(Position, "Teleporter", NeonEngine.DrawLayer.Middleground2, containerWorld)
        {
            this.spriteSheetOpening = new SpriteSheet(AssetManager.GetSpriteSheet("TeleporterOpening"), DrawLayer.Middleground2, this);
            this.avatar = avatar;
            this.spriteSheet.isPlaying = false;
            this.spriteSheet.IsLooped = false;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (IsActive)
            {
                this.spriteSheet.isPlaying = true;
            }
            else
            {
                this.spriteSheet.isPlaying = false;
                this.spriteSheet.SetFrame(0);
            }
            if (Vector2.Distance(avatar.transform.Position, this.transform.Position) < ActivationRange)
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
                if (Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Buttons.X))
                    containerWorld.ChangeScreen(new GameScreen(containerWorld.game));
            }

              
            base.Update(gameTime);
        }
    }
}
