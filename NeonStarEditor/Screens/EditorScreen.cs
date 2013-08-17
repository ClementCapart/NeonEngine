using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework.Graphics;
using NeonStarLibrary;
using System.Windows.Forms;
using NeonStarLibrary.Entities;
using System.Reflection;
using System.ComponentModel;
using Microsoft.Xna.Framework.Input;
using NeonEngine;

namespace NeonStarEditor
{
    public class EditorScreen : GameScreen, IDisposable
    {
        public MainEditor editorForm;

        public bool EditorVisible = true;
        public Form GameAsForm;
        public BottomDock BottomDockControl;

        public Tool CurrentTool;
        public Entity SelectedEntity;

        public bool FocusEntity = false;

        public BindingList<Entity> entityList = new BindingList<Entity>();

        public Form XNAWindow;
        public GraphicsDeviceManager graphics;


        public EditorScreen(Game game, GraphicsDeviceManager graphics)
            : base(game)
        {
            this.GameAsForm = Control.FromHandle(this.game.Window.Handle) as Form;
            this.graphics = graphics;
            entityList = new BindingList<Entity>(entities);
            game.IsMouseVisible = true;
            CurrentTool = new Selection(this);

            GameAsForm.Controls.Add(new NeonStarToolstrip());
            
            editorForm = new MainEditor(this);
            editorForm.Show();

            BottomDockControl = new BottomDock(this);
            
            GameAsForm.Controls.Add(BottomDockControl);

            XNAWindow = (Form)Form.FromHandle(this.game.Window.Handle);
        }

        public override void Update(GameTime gameTime)
        {
            if (this.CurrentTool != null && Neon.graphicsDevice.Viewport.Bounds.Contains((int)Neon.Input.ScreenMousePosition.X, (int)Neon.Input.ScreenMousePosition.Y))
                this.CurrentTool.Update(gameTime);
            
            if (Neon.Input.MouseCheck(MouseButton.LeftButton) && Neon.graphicsDevice.Viewport.Bounds.Contains((int)Neon.Input.ScreenMousePosition.X, (int)Neon.Input.ScreenMousePosition.Y))
                this.camera.Position += new Vector2(-Neon.Input.DeltaMouse.X, -Neon.Input.DeltaMouse.Y);

            if (Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Keys.F))
                FocusEntity = !FocusEntity;

            if (Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Keys.H))
            {
                if (EditorVisible)
                {
                    foreach (Control c in GameAsForm.Controls)
                        c.Hide();
                    EditorVisible = false;
                }
                else
                {
                    foreach (Control c in GameAsForm.Controls)
                        c.Show();
                    EditorVisible = true;
                }
            }

            if (Neon.Input.MouseWheel() > 0)
                this.camera.Zoom += 0.1f;
            else if (Neon.Input.MouseWheel() < 0)
                this.camera.Zoom -= 0.1f;

            if (Neon.Input.MousePressed(MouseButton.MiddleButton))
                this.camera.Zoom = 1f;

            if (FocusEntity && SelectedEntity != null)
                this.camera.SmoothFollow(SelectedEntity);

            base.Update(gameTime);
            ManageInspector();
        }

        public override void ManualDrawGame(SpriteBatch spriteBatch)
        {
            if (FocusEntity)
            {
                if (SelectedEntity != null)
                {
                    DrawableComponent dc = SelectedEntity.GetComponent<DrawableComponent>();
                    if (dc != null)
                    {
                        spriteBatch.Draw(AssetManager.GetTexture("neon_screen"), Vector2.Zero, Color.Lerp(Color.Transparent, Color.White, 0.7f));
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, camera.get_transformation(graphics.GraphicsDevice));
                        dc.Draw(spriteBatch);
                    }
                }
            }        
            base.ManualDrawGame(spriteBatch);
        }

        public override void ManualDrawHUD(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, camera.get_transformation(Neon.graphicsDevice));
            if (this.CurrentTool != null)
                this.CurrentTool.Draw(sb);
            sb.End();
            sb.Begin();
            base.ManualDrawHUD(sb);
        }

        public void ManageInspector()
        {
            if (SelectedEntity != null)
            {
                if (editorForm.entityList.SelectedEntity != null)
                {
                    foreach (PropertyComponentControl pcc in editorForm.entityList.PropertyControlList)
                    {
                        if (pcc.ctrl.Name == "X")
                        {
                            Vector2 v = (Vector2)pcc.pi.GetValue(pcc.c, null);
                            (pcc.ctrl as NumericUpDown).Value = (decimal)v.X;
                        }
                        else if(pcc.ctrl.Name == "Y")
                        {
                            Vector2 v = (Vector2)pcc.pi.GetValue(pcc.c, null);
                            (pcc.ctrl as NumericUpDown).Value = (decimal)v.Y;
                        }
                        else if (pcc.pi.PropertyType.IsEnum)
                        {
                            (pcc.ctrl as ComboBox).SelectedValue = (Enum)(pcc.pi.GetValue(pcc.c, null));
                        }
                        else if (pcc.pi.PropertyType == typeof(float))
                        {
                            float value = (float)pcc.pi.GetValue(pcc.c, null);
                            (pcc.ctrl as NumericUpDown).Value = (decimal)value;
                        }
                    }
                }

                if (Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Keys.Delete))
                {
                    SelectedEntity.Destroy();
                    SelectedEntity = null;
                }
            }
        }

        public override void RemoveEntity(Entity newEntity)
        {
            entityList.Remove(newEntity);
            //base.RemoveEntity(newEntity);
        }

        public override void ReloadLevel()
        {
            editorForm.Close();
            ChangeScreen(new EditorScreen(game, this.graphics));
        }

        public void Dispose()
        {
            editorForm.Dispose();
        }
    }
}
