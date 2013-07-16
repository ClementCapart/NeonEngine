using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarEditor
{
    public abstract class Tool
    {
        protected EditorScreen currentWorld;

        public Color Color;
        public Vector2 Position;
        public Texture2D pixel;

        public Tool(EditorScreen currentWorld)
        {
            this.currentWorld = currentWorld;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
           
        }
    }


}
