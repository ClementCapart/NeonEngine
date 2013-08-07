using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeonEngine;
using Microsoft.Xna.Framework;

namespace NeonStarLibrary
{
    public class LifeBar : HUDComponent
    {
        SpriteSheet spriteSheet;
        Avatar avatar;

        public LifeBar(Avatar avatar)
            : base(avatar, 0f, "LifeBar")
        {
            spriteSheet = new SpriteSheet(AssetManager.GetSpriteSheet("LifeBar"), Layer, new Vector2(180, 50), entity);
            spriteSheet.Init();
            spriteSheet.isPlaying = false;
            this.avatar = avatar;
        }

        public override void Update(GameTime gameTime)
        {
            if(avatar.LifePoints > 0)
                spriteSheet.SetFrame((int)(7 - avatar.LifePoints));
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteSheet.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
