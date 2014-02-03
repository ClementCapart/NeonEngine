using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace NeonStarEditor
{
    class Selection : Tool
    {
        private XElement TransformState = null;

        public Selection(EditorScreen currentWorld)
            :base(currentWorld)
        {
        }

        public override void Update(GameTime gameTime)
        {
            Select();
            Move();
            base.Update(gameTime);
        }

        private void Select()
        {
            Fixture fixture;
            Vector2 position = CoordinateConversion.screenToWorld(Neon.Input.MousePosition);
            if (Neon.Input.MousePressed(MouseButton.LeftButton) && currentWorld.IsActiveForm)
            {
                fixture = currentWorld.PhysicWorld.TestPoint(position);
                if (fixture != null)
                {   
                    currentWorld.SelectedEntity = Neon.Utils.GetEntityByBody(fixture.Body);
                    if (Neon.Utils.GetEntityByBody(fixture.Body) != null)
                    {
                        currentWorld.BottomDockControl.entityListControl.SelectEntityNode(Neon.Utils.GetEntityByBody(fixture.Body));
                    }
                    currentWorld.FocusedTextBox = null;
                    currentWorld.FocusedNumericUpDown = null;
                    currentWorld.ToggleGraphicPicker();
                    currentWorld.ToggleSpritesheetPicker();

                }
                else
                {
                    bool NoSelection = true;

                    foreach (Entity e in currentWorld.Entities)
                    {
                        if (Neon.Input.MousePosition.X >= e.transform.Position.X - 30 && Neon.Input.MousePosition.X <= e.transform.Position.X + 30
                            && Neon.Input.MousePosition.Y >= e.transform.Position.Y - 30 && Neon.Input.MousePosition.Y <= e.transform.Position.Y + 30)
                        {
                            currentWorld.SelectedEntity = e;
                            currentWorld.BottomDockControl.entityListControl.SelectEntityNode(e);
                            currentWorld.FocusedTextBox = null;
                            currentWorld.FocusedNumericUpDown = null;
                            NoSelection = false;
                            currentWorld.ToggleGraphicPicker();
                            currentWorld.ToggleSpritesheetPicker();
                            break;
                        }

                    }

                    if (NoSelection)
                    {
                        currentWorld.SelectedEntity = null;
                        currentWorld.BottomDockControl.entityListControl.EntityListBox.SelectedNode = null;
                        currentWorld.RefreshInspector(null);
                        currentWorld.FocusedTextBox = null;
                        currentWorld.FocusedNumericUpDown = null;
                        currentWorld.ToggleGraphicPicker();
                        currentWorld.ToggleSpritesheetPicker();
                    }
                }
            } 
        }

        private void Move()
        {
            if (Neon.Input.MousePressed(MouseButton.RightButton))
            {
                if(currentWorld.SelectedEntity != null)
                    TransformState = DataManager.SaveComponentParameters(currentWorld.SelectedEntity.transform);
            }
            if (Neon.Input.MouseCheck(MouseButton.RightButton) && currentWorld.IsActiveForm)
            {
                if (currentWorld.SelectedEntity != null)
                {
                    currentWorld.SelectedEntity.transform.Position += Neon.Input.DeltaMouse / currentWorld.Camera.Zoom;
                }
                else if (currentWorld.BottomDockControl.entityListControl.EntityListBox.SelectedNode != null && currentWorld.BottomDockControl.entityListControl.EntityListBox.SelectedNode.Parent == null)
                {
                    string layer = currentWorld.BottomDockControl.entityListControl.EntityListBox.SelectedNode.Text;
                    if(layer == "NoLayer")
                        layer = "";
                    foreach(Entity entity in currentWorld.Entities.Where(e => e.Layer == layer))
                    {
                        entity.transform.Position += Neon.Input.DeltaMouse / currentWorld.Camera.Zoom;
                    }
                }
                    
            }
            if (Neon.Input.MouseReleased(MouseButton.RightButton))
            {
                if (currentWorld.SelectedEntity != null)
                {
                    if (currentWorld.MagnetismActivated)
                    {
                        currentWorld.SelectedEntity.transform.Position = new Vector2((float)Math.Round((double)currentWorld.SelectedEntity.transform.Position.X), (float)Math.Round((double)currentWorld.SelectedEntity.transform.Position.Y));
                        if (currentWorld.MagnetismValue != 1)
                        {
                            int restX = (int)currentWorld.SelectedEntity.transform.Position.X % (int)currentWorld.MagnetismValue;
                            int restY = (int)currentWorld.SelectedEntity.transform.Position.Y % (int)currentWorld.MagnetismValue;

                            currentWorld.SelectedEntity.transform.Position += new Vector2((restX < currentWorld.MagnetismValue / 2 ? -restX : currentWorld.MagnetismValue - restX), (restY < currentWorld.MagnetismValue / 2 ? -restY : currentWorld.MagnetismValue - restY));
                        }
                    }

                    //ActionManager.SaveAction(ActionType.ChangeEntityParameters, new object[2] { currentWorld.SelectedEntity.transform, TransformState });
                    TransformState = null;
                }
                else if (currentWorld.BottomDockControl.entityListControl.EntityListBox.SelectedNode != null && currentWorld.BottomDockControl.entityListControl.EntityListBox.SelectedNode.Parent == null)
                {

                    string layer = currentWorld.BottomDockControl.entityListControl.EntityListBox.SelectedNode.Text;
                    if (layer == "NoLayer")
                        layer = "";

                    if(currentWorld.MagnetismActivated)
                    {
                        foreach (Entity entity in currentWorld.Entities.Where(e => e.Layer == layer))
                        {
                            entity.transform.Position = new Vector2((float)Math.Round((double)entity.transform.Position.X), (float)Math.Round((double)entity.transform.Position.Y));
                            if (currentWorld.MagnetismValue != 1)
                            {
                                int restX = (int)entity.transform.Position.X % (int)currentWorld.MagnetismValue;
                                int restY = (int)entity.transform.Position.Y % (int)currentWorld.MagnetismValue;

                                entity.transform.Position += new Vector2((restX < currentWorld.MagnetismValue / 2 ? -restX : currentWorld.MagnetismValue - restX), (restY < currentWorld.MagnetismValue / 2 ? -restY : currentWorld.MagnetismValue - restY));
                            }
                        }
                    }
                    

                }
                
            }
        }
    }
}
