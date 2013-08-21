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
    public class EditorScreen : GameScreen
    {
        public bool EditorVisible = true;
        public Form GameAsForm;
        public BottomDock BottomDockControl;
        public RightDock RightDockControl;
        public LeftDock LeftDockControl;

        public Tool CurrentTool;
        public Entity SelectedEntity;

        public TextBox FocusedTextBox = null;
        public NumericUpDown FocusedNumericUpDown = null;

        public bool MouseInGameWindow = false;

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

            //GameAsForm.Controls.Add(new NeonStarToolstrip());
        

            LeftDockControl = new LeftDock(this);
            BottomDockControl = new BottomDock(this);
            RightDockControl = new RightDock(this);
            

            GameAsForm.Controls.Add(BottomDockControl);
            GameAsForm.Controls.Add(RightDockControl);
            GameAsForm.Controls.Add(LeftDockControl);
            GameAsForm.MouseEnter += GameAsForm_MouseEnter;
            GameAsForm.MouseLeave += GameAsForm_MouseLeave;
            GameAsForm.KeyPreview = true;

            XNAWindow = (Form)Form.FromHandle(this.game.Window.Handle);
        }

        void GameAsForm_MouseLeave(object sender, EventArgs e)
        {
            this.MouseInGameWindow = false;
        }

        void GameAsForm_MouseEnter(object sender, EventArgs e)
        {
            this.MouseInGameWindow = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.CurrentTool != null && this.MouseInGameWindow)
                this.CurrentTool.Update(gameTime);

            if (Neon.Input.MouseCheck(MouseButton.LeftButton) && this.MouseInGameWindow)
                this.camera.Position += new Vector2(-Neon.Input.DeltaMouse.X, -Neon.Input.DeltaMouse.Y);

            if (Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Keys.F) && FocusedTextBox == null && FocusedNumericUpDown == null)
                FocusEntity = !FocusEntity;

            if (Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Keys.H) && FocusedTextBox == null && FocusedNumericUpDown == null)
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

            if (((Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Keys.LeftControl) || Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Keys.RightControl))&& Neon.Input.Check(Microsoft.Xna.Framework.Input.Keys.Z))
                || (Neon.Input.Check(Microsoft.Xna.Framework.Input.Keys.LeftControl) || Neon.Input.Check(Microsoft.Xna.Framework.Input.Keys.RightControl))&& Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Keys.Z))
                ActionManager.Undo();

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

        public void RefreshInspector(Entity SelectedEntity)
        {
            if (SelectedEntity != null)
            {
                this.SelectedEntity = SelectedEntity;
                RightDockControl.InspectorControl.InstantiateProperties(SelectedEntity);
            }
            else
            {
                RightDockControl.InspectorControl.ClearInspector();
            }
        }

        public void ManageInspector()
        {
            if (SelectedEntity != null)
            {
                foreach (PropertyComponentControl pcc in this.RightDockControl.InspectorControl.PropertyControlList)
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

                if (Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Keys.Delete) && FocusedTextBox == null && FocusedNumericUpDown == null)
                {
                    ActionManager.SaveAction(ActionType.DeleteEntity, new object[2] { DataManager.SavePrefab(this.SelectedEntity), this });
                    SelectedEntity.Destroy();
                    SelectedEntity = null;
                    RightDockControl.InspectorControl.ClearInspector();
                    
                }
                if (FocusedTextBox != null)
                    ManageText();
                if (FocusedNumericUpDown != null)
                    ManageNumber();
            }
        }

        public void ManageText()
        {
            if (FocusedTextBox != null)
            {
                 for (int i = 0; i < Neon.Input.KeysPressed.Length; i++)
                     if (Neon.Input.Pressed(Neon.Input.KeysPressed[i]) && Neon.Input.KeysPressed[i].ToString().Length == 1)
                     {

                         int LastSelectionStart = FocusedTextBox.SelectionStart;
                         FocusedTextBox.Text = FocusedTextBox.Text.Insert(FocusedTextBox.SelectionStart,
                             Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Keys.LeftShift) || Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Keys.RightShift) ? Neon.Input.KeysPressed[i].ToString().ToUpper() : Neon.Input.KeysPressed[i].ToString().ToLower());
                         FocusedTextBox.SelectionStart = ++LastSelectionStart;
                     }
                     else
                     {
                         if (Neon.Input.KeysPressed[i] == Microsoft.Xna.Framework.Input.Keys.Enter)
                             FocusedTextBox.Parent.Focus();
                     }
            }
        }

        public void ManageNumber()
        {
            if (FocusedNumericUpDown != null)
            {
                for (int i = 0; i < Neon.Input.KeysPressed.Length; i++)
                {
                    if (Neon.Input.Pressed(Neon.Input.KeysPressed[i]) && !(Neon.Input.Check(Microsoft.Xna.Framework.Input.Keys.LeftShift) || Neon.Input.Check(Microsoft.Xna.Framework.Input.Keys.RightShift)))
                    {
                        int LastSelectionStart = (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart;

                        switch(Neon.Input.KeysPressed[i])
                        {
                            case Microsoft.Xna.Framework.Input.Keys.D0:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "0");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.D1:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "1");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.D2:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "2");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.D3:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "3");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.D4:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "4");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.D5:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "5");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.D6:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "6");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.D7:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "7");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.D8:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "8");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.D9:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "9");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.NumPad0:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "0");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.NumPad1:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "1");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.NumPad2:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "2");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.NumPad3:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "3");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.NumPad4:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "4");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.NumPad5:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "5");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.NumPad6:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "6");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.NumPad7:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "7");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.NumPad8:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "8");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.NumPad9:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "9");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.OemPeriod:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                ",");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.OemComma:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                ",");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.Subtract:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "-");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Microsoft.Xna.Framework.Input.Keys.Enter:
                            FocusedNumericUpDown.Parent.Focus();
                            break;
                        }                       
                    }
                    else if (Neon.Input.Pressed(Neon.Input.KeysPressed[i]))
                    {
                        int LastSelectionStart = (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart;
                        if (Neon.Input.KeysPressed[i] == Microsoft.Xna.Framework.Input.Keys.D6)
                        {
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "-");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                        }
                    }
                }
            }
        }

        public override void AddEntity(Entity newEntity)
        {
            base.AddEntity(newEntity);
            entityList.ResetBindings();
        }

        public override void RemoveEntity(Entity newEntity)
        {
            base.RemoveEntity(newEntity);
            entityList.ResetBindings();
        }

        public override void ReloadLevel()
        {
            this.LeftDockControl.Dispose();
            this.BottomDockControl.Dispose();
            this.RightDockControl.Dispose();
            ChangeScreen(new EditorScreen(game, this.graphics));
        }
    }
}
