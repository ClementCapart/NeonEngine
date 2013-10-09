using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework.Graphics;
using NeonStarLibrary;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
using Microsoft.Xna.Framework.Input;
using NeonEngine;
using Keys = Microsoft.Xna.Framework.Input.Keys;

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
        float PressedDelay = 0.0f;

        public bool MouseInGameWindow = false;

        public bool FocusEntity = false;

        public BindingList<Entity> entityList = new BindingList<Entity>();

        public GraphicsDeviceManager graphics;
        public bool IsActiveForm = false;

        public EditorScreen(Game game, GraphicsDeviceManager graphics)
            : base(game)
        {
            GameAsForm = Control.FromHandle(this.game.Window.Handle) as Form;
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

        }

        void GameAsForm_MouseLeave(object sender, EventArgs e)
        {
            MouseInGameWindow = false;
        }

        void GameAsForm_MouseEnter(object sender, EventArgs e)
        {
            MouseInGameWindow = true;
        }

        public override void Update(GameTime gameTime)
        {
            this.IsActiveForm = System.Windows.Forms.Form.ActiveForm == this.GameAsForm;

            if (CurrentTool != null && MouseInGameWindow)
                CurrentTool.Update(gameTime);

            if (Neon.Input.MouseCheck(MouseButton.LeftButton) && MouseInGameWindow)
                camera.Position += new Vector2(-Neon.Input.DeltaMouse.X, -Neon.Input.DeltaMouse.Y);

            if (Neon.Input.Pressed(Keys.F) && FocusedTextBox == null && FocusedNumericUpDown == null)
                FocusEntity = !FocusEntity;

            if (Neon.Input.Pressed(Keys.H) && FocusedTextBox == null && FocusedNumericUpDown == null)
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
                camera.Zoom += 0.1f;
            else if (Neon.Input.MouseWheel() < 0)
                camera.Zoom -= 0.1f;

            if (Neon.Input.MousePressed(MouseButton.MiddleButton))
                camera.Zoom = 1f;

            if (FocusEntity && SelectedEntity != null)
                camera.SmoothFollow(SelectedEntity);

            if (((Neon.Input.Pressed(Keys.LeftControl) || Neon.Input.Pressed(Keys.RightControl))&& Neon.Input.Check(Keys.Z))
                || (Neon.Input.Check(Keys.LeftControl) || Neon.Input.Check(Keys.RightControl))&& Neon.Input.Pressed(Keys.Z))
                ActionManager.Undo();

            if (PressedDelay > 0.0f)
            {
                PressedDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
                PressedDelay = 0.0f;

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
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);
                        spriteBatch.Draw(AssetManager.GetTexture("neon_screen"), Vector2.Zero, Color.Lerp(Color.Transparent, Color.White, 0.7f));
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, camera.get_transformation(graphics.GraphicsDevice));
                        dc.Draw(spriteBatch);
                    }
                }
            }        
            base.ManualDrawGame(spriteBatch);
        }

        public override void ManualDrawHUD(SpriteBatch sb)
        {
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, camera.get_transformation(Neon.graphicsDevice));
            if (CurrentTool != null)
                CurrentTool.Draw(sb);
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
                foreach (PropertyComponentControl pcc in RightDockControl.InspectorControl.PropertyControlList)
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

                if (Neon.Input.Pressed(Keys.Delete) && FocusedTextBox == null && FocusedNumericUpDown == null)
                {
                    ActionManager.SaveAction(ActionType.DeleteEntity, new object[2] { DataManager.SavePrefab(SelectedEntity), this });
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
                int LastSelectionStart = FocusedTextBox.SelectionStart;

                 for (int i = 0; i < Neon.Input.KeysPressed.Length; i++)
                     if (Neon.Input.Pressed(Neon.Input.KeysPressed[i]) && Neon.Input.KeysPressed[i].ToString().Length == 1)
                     {

                         
                         FocusedTextBox.Text = FocusedTextBox.Text.Insert(FocusedTextBox.SelectionStart,
                             Neon.Input.Check(Keys.LeftShift) || Neon.Input.Check(Keys.RightShift) ? Neon.Input.KeysPressed[i].ToString().ToUpper() : Neon.Input.KeysPressed[i].ToString().ToLower());
                         FocusedTextBox.SelectionStart = ++LastSelectionStart;
                     }
                     else
                     {
                         if (Neon.Input.KeysPressed[i] == Keys.Enter)
                         {
                             FocusedTextBox.Parent.Focus();
                             FocusedTextBox = null;
                         }
                         else if (Neon.Input.KeysPressed[i] == Keys.Delete && PressedDelay <= 0f)
                         {
                             if (FocusedTextBox.SelectionStart < FocusedTextBox.Text.Length)
                             {
                                 PressedDelay = 0.5f;
                                 if (FocusedTextBox.SelectionLength > 0)
                                 {
                                     FocusedTextBox.Text = FocusedTextBox.Text.Remove(FocusedTextBox.SelectionStart, FocusedTextBox.SelectionLength);
                                     FocusedTextBox.SelectionStart = LastSelectionStart;
                                 }
                                 else
                                 {
                                     FocusedTextBox.Text = FocusedTextBox.Text.Remove(FocusedTextBox.SelectionStart, 1);
                                     FocusedTextBox.SelectionStart = LastSelectionStart;
                                 }
                             }                                             
                         }
                     }
            }
        }

        public void ManageNumber()
        {
            if (FocusedNumericUpDown != null)
            {
                for (int i = 0; i < Neon.Input.KeysPressed.Length; i++)
                {
                    if (Neon.Input.Pressed(Neon.Input.KeysPressed[i]) && !(Neon.Input.Check(Keys.LeftShift) || Neon.Input.Check(Keys.RightShift)))
                    {
                        int LastSelectionStart = (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart;

                        switch(Neon.Input.KeysPressed[i])
                        {
                            case Keys.D0:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "0");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Keys.D1:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "1");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Keys.D2:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "2");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Keys.D3:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "3");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Keys.D4:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "4");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Keys.D5:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "5");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Keys.D6:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "6");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Keys.D7:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "7");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Keys.D8:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "8");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Keys.D9:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "9");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Keys.NumPad0:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "0");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Keys.NumPad1:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "1");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Keys.NumPad2:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "2");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Keys.NumPad3:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "3");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Keys.NumPad4:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "4");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Keys.NumPad5:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "5");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Keys.NumPad6:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "6");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Keys.NumPad7:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "7");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Keys.NumPad8:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "8");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Keys.NumPad9:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "9");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Keys.OemPeriod:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                ",");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart; 
                            break;

                            case Keys.OemComma:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                ",");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Keys.Subtract:
                            FocusedNumericUpDown.Controls[1].Text = FocusedNumericUpDown.Controls[1].Text.Insert((FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart,
                                "-");
                            (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart = ++LastSelectionStart;
                            break;

                            case Keys.Enter:
                            FocusedNumericUpDown.Parent.Focus();
                            break;

                            case Keys.Delete:
                            if(PressedDelay <= 0F)
                            {
                            
                            }
                            break;
                             
                        }                       
                    }
                    else if (Neon.Input.Pressed(Neon.Input.KeysPressed[i]))
                    {
                        int LastSelectionStart = (FocusedNumericUpDown.Controls[1] as TextBox).SelectionStart;
                        if (Neon.Input.KeysPressed[i] == Keys.D6)
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
            LeftDockControl.Dispose();
            BottomDockControl.Dispose();
            RightDockControl.Dispose();
            ChangeScreen(new EditorScreen(game, graphics));
        }
    }
}
