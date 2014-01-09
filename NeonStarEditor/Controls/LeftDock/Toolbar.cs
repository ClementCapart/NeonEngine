using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NeonEngine;
using System.Reflection;
using NeonEngine.Private;
using Component = NeonEngine.Component;
using NeonStarLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace NeonStarEditor.Controls.LeftDock
{
    public partial class Toolbar : UserControl
    {
        public EditorScreen GameWorld;

        ComponentResourceManager resources = new ComponentResourceManager(typeof(Toolbar));

        public Toolbar()
        {
            InitializeComponent();
            PausePlayButton.BackgroundImage = PausePlayButton.BackgroundImage = ((Image)(resources.GetObject("PauseButton")));
        }

        protected override void OnLoad(EventArgs e)
        {
            if (GameWorld == null)
            {
                Console.WriteLine("GameWorld is missing in Toolbar.cs");
            }
            base.OnLoad(e);
        }

        private void SaveCurrentMap_Click(object sender, EventArgs e)
        {
            DataManager.SaveLevel(GameWorld, "LiOn");
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            GameWorld.ReloadLevel();
        }

        private void PausePlayButton_Click(object sender, EventArgs e)
        {
            if (GameWorld.Pause)
            {
                GameWorld.Pause = false;
                PausePlayButton.BackgroundImage = ((Image)(resources.GetObject("PauseButton")));
            }
            else
            {
                GameWorld.Pause = true;
                PausePlayButton.BackgroundImage = ((Image)(resources.GetObject("PlayButton")));
            }
        }

        private void Selection_Click(object sender, EventArgs e)
        {
            GameWorld.CurrentTool = new Selection(GameWorld);
        }

        private void ReloadScript_Click(object sender, EventArgs e)
        {
            Neon.NeonScripting.CompileScripts();
            if (Neon.Scripts != null)
                foreach (Entity ent in GameWorld.Entities)
                    for (int i = ent.Components.Count - 1; i >= 0; i--)
                    {
                        ScriptComponent sc = ent.Components[i] is ScriptComponent ? (ScriptComponent)ent.Components[i] : null;
                        if (sc != null)
                        {
                            sc.Remove();
                            foreach (Type t in Neon.Scripts)
                                if (sc.GetType().Name == t.Name)
                                    ent.AddComponent((Component)Activator.CreateInstance(t, ent));
                        }
                    }

            List<Type> Components = new List<Type>(Neon.Utils.GetTypesInNamespace(Assembly.GetAssembly(typeof(Neon)), "NeonEngine").Where(t => t.IsSubclassOf(typeof(Component)) && !(t.IsAbstract)));
            if (Neon.Scripts != null)
                Components.AddRange(Neon.Scripts);
            Components.Add(typeof(ScriptComponent));
            GameWorld.RightDockControl.InspectorControl.ComponentList.DataSource = Components;
            GameWorld.RightDockControl.InspectorControl.ComponentList.DisplayMember = "Name";
        }

        private void CreateRectangle_Click(object sender, EventArgs e)
        {
            GameWorld.CurrentTool = new CreateRectangle(GameWorld);
        }

        private void ReloadAssetsButton_Click(object sender, EventArgs e)
        {
            //AssetManager.LoadAssets(GameWorld.game.GraphicsDevice);
        }

        private void AttackManagerButton_Click(object sender, EventArgs e)
        {
            GameWorld.ToggleAttackManager();
        }

        private void ToggleBoundsButton_Click(object sender, EventArgs e)
        {
            GameWorld._boundsGizmoToggled = !GameWorld._boundsGizmoToggled;
        }

        private void ToggleLightButton_Click(object sender, EventArgs e)
        {
            GameWorld._lightGizmoToggled = !GameWorld._lightGizmoToggled;
        }

        private void PathNodeTool_Click(object sender, EventArgs e)
        {
            GameWorld.TogglePathNodeManager();
        }

        private void magnetCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            GameWorld.MagnetismActivated = (sender as CheckBox).Checked;
        }

        private void SaveHitboxes_Click(object sender, EventArgs e)
        {
            Dictionary<RenderTarget2D, Vector2> renderTargets = new Dictionary<RenderTarget2D, Vector2>();

            float StartWidth = 10000.0f;
            float EndWidth = -10000.0f;
            float StartHeight = 10000.0f;
            float EndHeight = -10000.0f; 

            foreach (Hitbox hb in GameWorld.Hitboxes)
            {
                if (hb.Type != HitboxType.Trigger && hb.Type != HitboxType.Hit && hb.Type != HitboxType.Bullet)
                {
                    if (StartWidth > hb.hitboxRectangle.Left)
                        StartWidth = hb.hitboxRectangle.Left;
                    if (EndWidth < hb.hitboxRectangle.Right)
                        EndWidth = hb.hitboxRectangle.Right;
                    if (StartHeight > hb.hitboxRectangle.Top)
                        StartHeight = hb.hitboxRectangle.Top;
                    if (EndHeight < hb.hitboxRectangle.Bottom)
                        EndHeight = hb.hitboxRectangle.Bottom;
                }
            }
            StartWidth -= 10;
            EndWidth += 10;
            StartHeight -= 10;
            EndWidth += 10;

            int Width = (int)Math.Abs(EndWidth - StartWidth);
            int Height = (int)Math.Abs(EndHeight -StartHeight);

            Vector2 CurrentOffset = new Vector2(StartWidth, StartHeight);

            Console.WriteLine("Width -> " + Width);
            Console.WriteLine("Height -> " + Height);
            Console.WriteLine("...");

            while (Height > 4096)
            {
                while (Width > 4096)
                {
                    renderTargets.Add(new RenderTarget2D(Neon.GraphicsDevice, 4096, 4096), new Vector2(CurrentOffset.X, CurrentOffset.Y));
                    CurrentOffset += new Vector2(4096, 0);
                    Width -= 4096;
                }

                if(Width > 0)
                {
                    renderTargets.Add(new RenderTarget2D(Neon.GraphicsDevice, Width, 4096), new Vector2(CurrentOffset.X, CurrentOffset.Y));
                    CurrentOffset += new Vector2(Width, 0);
                }

                CurrentOffset += new Vector2(0, 4096);
                Height -= 4096;
            }

            while (Width > 4096)
            {
                renderTargets.Add(new RenderTarget2D(Neon.GraphicsDevice, 4096, Height), new Vector2(CurrentOffset.X, CurrentOffset.Y));
                CurrentOffset += new Vector2(4096, 0);
                Width -= 4096;
            }

            if (Width > 0)
            {
                renderTargets.Add(new RenderTarget2D(Neon.GraphicsDevice, Width, Height), new Vector2(CurrentOffset.X, CurrentOffset.Y));
                CurrentOffset += new Vector2(Width, 0);
            }

            PolygonRenderer _polygonRenderer = new PolygonRenderer(Neon.GraphicsDevice, Vector2.Zero);
            float formerZoom = Neon.World.Camera.Zoom;
            Neon.World.Camera.Zoom = 1.0f;
            foreach (KeyValuePair<RenderTarget2D, Vector2> kvp in renderTargets)
            {
                RenderTarget2D rt = kvp.Key;
                Neon.GraphicsDevice.SetRenderTarget(rt);
                Neon.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Transparent);
                Neon.World.Camera.Position = kvp.Value + new Vector2(rt.Width / 2, rt.Height / 2);
                Console.WriteLine(kvp.Value);
                Neon.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Neon.World.Camera.get_transformation(Neon.GraphicsDevice));

                foreach (Hitbox hb in Neon.World.Hitboxes)
                {
                    switch (hb.Type)
                    {
                        case HitboxType.Main:
                            _polygonRenderer.Color = Microsoft.Xna.Framework.Color.Green;
                            _polygonRenderer.vectors = hb.vectors;
                            _polygonRenderer.Position = hb.Center;
                            _polygonRenderer.Draw(Neon.SpriteBatch);
                            break;

                        case HitboxType.Hit:
                            _polygonRenderer.Color = Microsoft.Xna.Framework.Color.Red;
                            break;

                        case HitboxType.None:
                            _polygonRenderer.Color = Microsoft.Xna.Framework.Color.White;
                            break;

                        case HitboxType.Bullet:
                            _polygonRenderer.Color = Microsoft.Xna.Framework.Color.LightPink;
                            break;

                        case HitboxType.Invincible:
                            _polygonRenderer.Color = Microsoft.Xna.Framework.Color.LightBlue;
                            break;

                        case HitboxType.Solid:
                            _polygonRenderer.Color = Microsoft.Xna.Framework.Color.Violet;
                            _polygonRenderer.vectors = hb.vectors;
                            _polygonRenderer.Position = hb.Center;
                            _polygonRenderer.Draw(Neon.SpriteBatch);
                            break;

                        case HitboxType.OneWay:
                            _polygonRenderer.Color = Microsoft.Xna.Framework.Color.Purple;
                            _polygonRenderer.vectors = hb.vectors;
                            _polygonRenderer.Position = hb.Center;
                            _polygonRenderer.Draw(Neon.SpriteBatch);
                            break;

                        case HitboxType.Trigger:
                            _polygonRenderer.Color = Microsoft.Xna.Framework.Color.SkyBlue;
                            break;
                    }
                }

                Neon.SpriteBatch.End();
                Neon.GraphicsDevice.SetRenderTarget(null);
                Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Level0_" + kvp.Value.X + "_" + kvp.Value.Y + ".png");
                Stream stream = File.OpenWrite(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Level0_" + kvp.Value.X + "_" + kvp.Value.Y + ".png");
                rt.SaveAsPng(stream, rt.Width, rt.Height);
                stream.Close();
                Neon.World.Camera.Zoom = formerZoom;
            }
        }

        private void frameByFrame_Click(object sender, EventArgs e)
        {
            if (!Neon.World.Pause)
                Neon.World.Pause = true;
            else
            {
                GameWorld.UnpauseTillNextFrame = true;
                Neon.World.Pause = false;
            }
        }

        private void respawnPanel_Click(object sender, EventArgs e)
        {
            GameWorld.ToggleSpawnPointManager();
        }
    }
}
