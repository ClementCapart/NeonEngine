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
                texture = AssetManager.GetTexture(value);
            }
        }

        public float DrawLayer
        {
            get
            {
                return Layer;
            }
            set
            {
                Layer = value;
            }
        }


        public Graphic(Entity entity)
            : base(0, entity, "Graphic")
        {
        }

        public Graphic(Texture2D texture, float Layer, Entity entity)
            : base(Layer, entity, "Graphic")
        {
            this.texture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (entity != null && texture != null)
                spriteBatch.Draw(texture, entity.transform.Position + this._parallaxPosition + Offset, null, Color.White, entity.transform.rotation, new Vector2(texture.Width / 2, texture.Height / 2), entity.transform.Scale, SpriteEffects.None, Layer);
        }
    }
}
