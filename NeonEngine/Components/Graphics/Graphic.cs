using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NeonEngine
{
    public class Graphic : DrawableComponent
    {
        private Texture2D texture;
        public float opacity = 1f;

        public string graphicTag;
        public string GraphicTag
        {
            get
            {
                return graphicTag;
            }
            set
            {
                graphicTag = value;
                this.texture = AssetManager.GetTexture(value);
            }
        }

        public DrawLayer drawLayer
        {
            get
            {
                return DrawType;
            }
            set
            {
                ChangeLayer(value);
            }
        }


        public Graphic(Entity entity)
            : base(DrawLayer.None, entity, "Graphic")
        {
        }

        public Graphic(Texture2D texture, DrawLayer DrawType, Entity entity)
            : base(DrawType, entity, "Graphic")
        {
            this.texture = texture;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (entity != null)
                spriteBatch.Draw(texture, entity.transform.Position, null, Color.White, entity.transform.rotation, new Vector2(texture.Width / 2, texture.Height / 2), entity.transform.Scale, SpriteEffects.None, 0);
        }
    }
}
