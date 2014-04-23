using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using World = NeonEngine.World;

namespace NeonStarEditor
{
    public class AreaSelection : Tool
    {
        public List<Vector2> vectors = new List<Vector2>();

        public AreaSelection(World currentWorld)
            : base((EditorScreen)currentWorld)
        {
            pixel = new Texture2D(Neon.GraphicsDevice, 1, 1);
            Color[] pixels = new Color[1];
            pixels[0] = Color.White;
            pixel.SetData<Color>(pixels);
            Color = Color.SkyBlue;
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

        public override void Update(GameTime gameTime)
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
                currentWorld.SelectedEntity = null;
                currentWorld.OtherSelectedEntities.Clear();
                vectors[1] = new Vector2(Neon.Input.MousePosition.X, vectors[1].Y);
                vectors[2] = Neon.Input.MousePosition;
                vectors[3] = new Vector2(vectors[3].X, Neon.Input.MousePosition.Y);
                
                if (Neon.Input.MouseReleased(MouseButton.LeftButton))
                {
                    Vector2 position = new Vector2(float.MaxValue, float.MaxValue);
                    int width = int.MinValue;
                    int height = int.MinValue;

                    foreach (Vector2 v in vectors)
                    {
                        if (v.X < position.X)
                            position.X = v.X;
                        if (v.Y < position.Y)
                            position.Y = v.Y;

                        if (Math.Abs(v.X - position.X) > width)
                            width = (int)Math.Abs(v.X - position.X);
                        if (Math.Abs(v.Y - position.Y) > height)
                            height = (int)Math.Abs(v.Y - position.Y);
                    }

                    Rectangle r = new Rectangle((int)position.X, (int)position.Y, width, height);

                    vectors.Clear();

                    foreach (Entity e in currentWorld.Entities)
                    {
                        if (r.Contains((int)e.transform.Position.X, (int)e.transform.Position.Y))
                        {
                            if (currentWorld.SelectedEntity == null)
                                currentWorld.SelectedEntity = e;
                            else
                                currentWorld.OtherSelectedEntities.Add(e);
                        }
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
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
                    new Vector2(distance, 1 / currentWorld.Camera.Zoom),
                    SpriteEffects.None,
                    0);
            }
            base.Draw(spriteBatch);
        }
    }
}
