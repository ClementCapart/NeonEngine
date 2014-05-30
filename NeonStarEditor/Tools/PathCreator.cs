using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using NeonEngine.Components.Graphics2D;
using NeonStarLibrary.Components.EnergyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using World = NeonEngine.World;

namespace NeonStarEditor
{
    public enum PartType
    {
        TopRightCorner,
        BottomRightCorner,
        LeftTopCorner,
        LeftBottomCorner,
        Vertical,
        Horizontal
    }


    public class PathPart
    {
        public PartType Type;
        public float Size;
        public Vector2 Position;
    }

    public class PathCreator : Tool
    {
        public List<PathPart> PathToCreate;
        public Vector2 NextPointPosition;

        public PathCreator(World currentWorld)
            :base((EditorScreen)currentWorld)
        {
            pixel = new Texture2D(Neon.GraphicsDevice, 1, 1);
            Color[] pixels = new Color[1];
            pixels[0] = Color.White;
            pixel.SetData<Color>(pixels);
            Color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            if (Neon.Input.MousePressed(MouseButton.LeftButton))
            {
                if(PathToCreate == null || PathToCreate.Count == 0)
                {
                    PathToCreate = new List<PathPart>();
                    PathPart pp = new PathPart();
                    pp.Position = Neon.Input.MousePosition;
                    PathToCreate.Add(pp);
                    pp = new PathPart();
                    pp.Position = Neon.Input.MousePosition;
                    PathToCreate.Add(pp);
                }
                else if (PathToCreate.Count == 2)
                {
                    PathPart pp = new PathPart();
                    pp.Position = Neon.Input.MousePosition;
                    PathToCreate.Add(pp);
                }
                else if (PathToCreate.Count > 2)
                {
                    PathPart pp = new PathPart();
                    pp.Position = Neon.Input.MousePosition;
                    PathToCreate.Add(pp);
                }
            }

            if (Neon.Input.MousePressed(MouseButton.RightButton) && PathToCreate != null && PathToCreate.Count > 1)
            {
                CreatePath();
            }

            if(PathToCreate != null && PathToCreate.Count > 1)
            {
                Vector2 lastPointPosition = PathToCreate[PathToCreate.Count - 2].Position;
                float angleInDegree = MathHelper.ToDegrees((float)Math.Atan2(Neon.Input.MousePosition.Y - lastPointPosition.Y, Neon.Input.MousePosition.X - lastPointPosition.X));

                if ((Math.Abs(angleInDegree) >= 0 && Math.Abs(angleInDegree) < 45) || (Math.Abs(angleInDegree) >= 135 && Math.Abs(angleInDegree) < 180))
                    NextPointPosition = new Vector2(Neon.Input.MousePosition.X, lastPointPosition.Y);
                else
                    NextPointPosition = new Vector2(lastPointPosition.X, Neon.Input.MousePosition.Y);

                PathToCreate.Last().Position = NextPointPosition;
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (PathToCreate == null)
                return;

            for (int i = 0; i < PathToCreate.Count; i++)
            {
                spriteBatch.Draw(pixel,
                    PathToCreate[i].Position - new Vector2(4, 4),
                    null,
                    Color,
                    0.0f,
                    new Vector2(0, 0),
                    new Vector2(8, 8),
                    SpriteEffects.None,
                    0);

                if(i == 0)
                    continue;
                Vector2 vector1 = PathToCreate[i - 1].Position;
                Vector2 vector2 = PathToCreate[i].Position;

                float distance = Vector2.Distance(vector1, vector2);
                float angle = (float)Math.Atan2((double)(vector2.Y - vector1.Y),
                    (double)(vector2.X - vector1.X));

                spriteBatch.Draw(pixel,
                    Position + vector1 - Vector2.Transform(new Vector2(0, 1.5f), Matrix.CreateRotationZ(angle)),
                    null,
                    Color,
                    angle,
                    new Vector2(0, 0),
                    new Vector2(distance, 3),
                    SpriteEffects.None,
                    0);
            }
            base.Draw(spriteBatch);
        }

        public void CreatePath()
        {
            PathToCreate.RemoveAt(PathToCreate.Count - 1);
            Entity e = new Entity(currentWorld);
            e.Name = "Link";
            e.transform.Position = PathToCreate[0].Position;
            e.transform.Scale = 1.0f;

            AnimatedEnergyObject aeo = new AnimatedEnergyObject(e);
            aeo.UnpoweredAnimation = "Off";
            aeo.PoweredAnimation = "On";
            e.AddComponent(aeo);

            for(int i = 1; i < PathToCreate.Count; i ++)
            {
                TilableSpritesheetManager tss = new TilableSpritesheetManager(e);
                Vector2 vector1 = PathToCreate[i - 1].Position;
                Vector2 vector2 = PathToCreate[i].Position;
                float angle = MathHelper.ToDegrees((float)Math.Atan2((double)(vector2.Y - vector1.Y),
                    (double)(vector2.X - vector1.X)));

                if (i + 1 < PathToCreate.Count)
                {
                    Vector2 nextDirection = PathToCreate[i + 1].Position;
                    float nextAngle = MathHelper.ToDegrees((float)Math.Atan2((double)(nextDirection.Y - vector2.Y),
                    (double)(nextDirection.X - vector2.X)));

                    Side cornerSide = Side.Right;
                    string spritesheetTag = "";
                    string spritesheetTagOff = "";

                    if (angle == 0)
                    {
                        if (nextAngle == 90)
                        {
                            spritesheetTag = "LinkOnCorner2";
                            spritesheetTagOff = "LinkOffCorner2";
                            cornerSide = Side.Left;
                        }
                        else if (nextAngle == -90)
                        {
                            spritesheetTag = "LinkOnCorner";
                            spritesheetTagOff = "LinkOffCorner";
                            cornerSide = Side.Left;
                        }
                    }
                    else if (angle == 180)
                    {
                        if(nextAngle == 90)
                        {
                            spritesheetTag = "LinkOnCorner2";
                            spritesheetTagOff = "LinkOffCorner2";
                        }
                        else if (nextAngle == -90)
                        {
                            spritesheetTag = "LinkOnCorner";
                            spritesheetTagOff = "LinkOffCorner";
                        }
                    }
                    else if (angle == 90)
                    {
                        if (nextAngle == 0)
                        {
                            spritesheetTag = "LinkOnCorner";
                            spritesheetTagOff = "LinkOffCorner";
                        }
                        else if (nextAngle == 180)
                        {
                            spritesheetTag = "LinkOnCorner";
                            spritesheetTagOff = "LinkOffCorner";
                            cornerSide = Side.Left;
                        }
                    }
                    else if (angle == -90)
                    {
                        if (nextAngle == 0)
                        {
                            spritesheetTag = "LinkOnCorner2";
                            spritesheetTagOff = "LinkOffCorner2";
                        }
                        else if (nextAngle == 180)
                        {
                            spritesheetTag = "LinkOnCorner2";
                            spritesheetTagOff = "LinkOffCorner2";
                            cornerSide = Side.Left;
                        }
                    }

                    SpritesheetManager ss = new SpritesheetManager(e);
                    Dictionary<string, SpriteSheetInfo> spritesheets = new Dictionary<string,SpriteSheetInfo>();

                    spritesheets.Add("On", AssetManager.GetSpriteSheet(spritesheetTag));
                    spritesheets.Add("Off", AssetManager.GetSpriteSheet(spritesheetTagOff));

                    ss.SpritesheetList = spritesheets;
                    ss.CurrentSide = cornerSide;
                    ss.Offset = ((vector2 - PathToCreate[0].Position) / e.transform.Scale);
                    if (ss.CurrentSide == Side.Left)
                    {
                        ss.Offset = new Vector2(ss.Offset.X * -1, ss.Offset.Y);
                    }
                    ss.Layer = 0.92f;

                    e.AddComponent(ss);
                }              

                if (angle == 0 || angle == 180)
                {
                    Dictionary<string, SpriteSheetInfo> spritesheets = new Dictionary<string, SpriteSheetInfo>();
                    spritesheets.Add("On", AssetManager.GetSpriteSheet("LinkOnHorizontal"));
                    spritesheets.Add("Off", AssetManager.GetSpriteSheet("LinkOffHorizontal"));
                    tss.SpritesheetList = spritesheets;
                    tss.UseTextureHeight = true;
                    tss.UseTextureWidth = false;
                    tss.TilingWidth = Vector2.Distance(vector2, vector1) - 10 * e.transform.Scale;
                    if (vector1.X > vector2.X)
                    {
                        Vector2 temp = vector1;
                        vector1 = vector2;
                        vector2 = temp;
                    }

                    tss.Offset = new Vector2(10, 0);
                }
                else
                {
                    Dictionary<string, SpriteSheetInfo> spritesheets = new Dictionary<string, SpriteSheetInfo>();
                    spritesheets.Add("On", AssetManager.GetSpriteSheet("LinkOnVertical"));
                    spritesheets.Add("Off", AssetManager.GetSpriteSheet("LinkOffVertical"));
                    tss.SpritesheetList = spritesheets;
                    tss.UseTextureHeight = false;
                    tss.UseTextureWidth = true;
                    tss.TilingHeight = Vector2.Distance(vector2, vector1) - 10 * e.transform.Scale;
                    if (vector1.Y > vector2.Y)
                    {
                        Vector2 temp = vector1;
                        vector1 = vector2;
                        vector2 = temp;
                    }

                    tss.Offset = new Vector2(0, 10);
                }

                tss.Offset += (vector1 - PathToCreate[0].Position) / e.transform.Scale;
                tss.DrawLayer = 0.91f;
                tss.Init();
                e.AddComponent(tss);
            }

            aeo.Init();
            currentWorld.AddEntity(e);
            PathToCreate.Clear();
        }
    }
}
