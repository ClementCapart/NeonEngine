using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarEditor
{
    public class CreateRectangle : Tool
    {
        public List<Vector2> vectors = new List<Vector2>();

        public CreateRectangle(World currentWorld)
            :base((EditorScreen)currentWorld)
        {
            pixel = new Texture2D(Neon.graphicsDevice, 1, 1);
            Color[] pixels = new Color[1];
            pixels[0] = Color.White;
            pixel.SetData<Color>(pixels);
            Color = Color.White;
        }

        public void AddVector(Vector2 vector)
        {
            vectors.Add(vector);
        }

        public void RemoveVector(Vector2 vector)
        {
            vectors.Remove(vector);
        }

        public void RemoveVector(int index)
        {
            vectors.RemoveAt(index);
        }

        public void ClearVectors()
        {
            vectors.Clear();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (vectors.Count < 4)
            {
                if (Neon.Input.MousePressed(MouseButton.LeftButton))
                {
                    vectors.Clear();
                    for (int i = 0; i < 5; i++)
                        AddVector(Neon.Input.MousePosition);
                }
            }
            else
            {
                vectors[1] = new Vector2(Neon.Input.MousePosition.X, vectors[1].Y);
                vectors[2] = Neon.Input.MousePosition;
                vectors[3] = new Vector2(vectors[3].X, Neon.Input.MousePosition.Y);
                if (Neon.Input.MousePressed(MouseButton.LeftButton))
                {
                    FarseerPhysics.Common.Vertices vertices = new FarseerPhysics.Common.Vertices();
                    foreach(Vector2 v in vectors)
                        vertices.Add(v - vectors[0]);

                    if ((int)Math.Abs(vertices[1].X) == 0 || (int)Math.Abs(vertices[2].Y) == 0)
                        return;

                    Entity e = new Entity(currentWorld);
                    e.Name = "Level Geometry";
                    e.transform.Position = vectors[0] + new Vector2(vertices[2].X / 2, vertices[2].Y / 2);
                    
                    Hitbox hb = new Hitbox(e);
                    hb.Width = (int)Math.Abs(vertices[1].X);
                    hb.Height = (int)Math.Abs(vertices[2].Y);
                    hb.Init();
                    e.AddComponent(hb);

                    Rigidbody rg = new Rigidbody(e);
                    rg.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
                    rg.Init();
                    e.AddComponent(rg);
                    rg.Hitbox = hb;
                    currentWorld.AddEntity(e);
                    currentWorld.CurrentTool = new CreateRectangle(currentWorld);
                }
 
            }
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (vectors.Count < 2)
                return;

            for (int i = 1; i < vectors.Count; i++)
            {
                Vector2 vector1 = vectors[i - 1];
                Vector2 vector2 = vectors[i];

                float distance = Vector2.Distance(vector1, vector2);
                float angle = (float)Math.Atan2((double)(vector2.Y - vector1.Y),
                    (double)(vector2.X - vector1.X));

                spriteBatch.Draw(pixel,
                    Position + vector1,
                    null,
                    Color,
                    angle,
                    Vector2.Zero,
                    new Vector2(distance, 3),
                    SpriteEffects.None,
                    0);
            }
            base.Draw(spriteBatch);
        }
    }
}
