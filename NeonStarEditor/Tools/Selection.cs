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
        private Vector2 _previousPosition;

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
            if (Neon.Input.MousePressed(MouseButton.LeftButton) && !(Neon.Input.Check(Microsoft.Xna.Framework.Input.Keys.LeftControl) || Neon.Input.Check(Microsoft.Xna.Framework.Input.Keys.RightControl)) && currentWorld.IsActiveForm)
            {
                currentWorld.OtherSelectedEntities = new List<Entity>();
                bool NoSelection = true;
                if (currentWorld.DisplayEntityCenter)
                {
                    foreach (Entity e in currentWorld.Entities)
                    {
                        if (!currentWorld.Layers.Contains(e.Layer))
                            continue;
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
                }
                else
                {
                    fixture = currentWorld.PhysicWorld.TestPoint(position);
                    if (fixture != null)
                    {   
                        currentWorld.SelectedEntity = Neon.Utils.GetEntityByBody(fixture.Body);
                        if (Neon.Utils.GetEntityByBody(fixture.Body) != null)
                        {
                            currentWorld.BottomDockControl.entityListControl.SelectEntityNode(Neon.Utils.GetEntityByBody(fixture.Body));
                            NoSelection = false;
                        }
                        currentWorld.FocusedTextBox = null;
                        currentWorld.FocusedNumericUpDown = null;
                        currentWorld.ToggleGraphicPicker();
                        currentWorld.ToggleSpritesheetPicker();
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
            else if (Neon.Input.MousePressed(MouseButton.LeftButton) && (Neon.Input.Check(Microsoft.Xna.Framework.Input.Keys.LeftControl) || Neon.Input.Check(Microsoft.Xna.Framework.Input.Keys.RightControl)) && currentWorld.IsActiveForm && currentWorld.SelectedEntity != null)
            {
                if (currentWorld.DisplayEntityCenter)
                {
                    foreach (Entity e in currentWorld.Entities)
                    {
                        if (Neon.Input.MousePosition.X >= e.transform.Position.X - 30 && Neon.Input.MousePosition.X <= e.transform.Position.X + 30
                            && Neon.Input.MousePosition.Y >= e.transform.Position.Y - 30 && Neon.Input.MousePosition.Y <= e.transform.Position.Y + 30)
                        {
                            if (!currentWorld.OtherSelectedEntities.Contains(e) && currentWorld.SelectedEntity != e)
                                currentWorld.OtherSelectedEntities.Add(e);
                            else if (currentWorld.SelectedEntity != e)
                                currentWorld.OtherSelectedEntities.Remove(e);
                            currentWorld.ToggleGraphicPicker();
                            currentWorld.ToggleSpritesheetPicker();
                            break;
                        }
                    }
                }
                else
                {
                    fixture = currentWorld.PhysicWorld.TestPoint(position);
                    if (fixture != null)
                    {
                        Entity e = Neon.Utils.GetEntityByBody(fixture.Body);
                        if (e != null)
                        {
                            if (!currentWorld.OtherSelectedEntities.Contains(e) && currentWorld.SelectedEntity != e)
                                currentWorld.OtherSelectedEntities.Add(e);
                            else if(currentWorld.SelectedEntity != e)
                                currentWorld.OtherSelectedEntities.Remove(e);
                        }
                        currentWorld.ToggleGraphicPicker();
                        currentWorld.ToggleSpritesheetPicker();
                    }
                }
            }
        }

        private void Move()
        {
            List<Vector2> _previousPositions = new List<Vector2>();
            if (Neon.Input.MousePressed(MouseButton.RightButton))
            {
                if(currentWorld.SelectedEntity != null)
                    _previousPosition = new Vector2(currentWorld.SelectedEntity.transform.Position.X, currentWorld.SelectedEntity.transform.Position.Y);

                foreach (Entity e in currentWorld.OtherSelectedEntities)
                    _previousPositions.Add(e.transform.Position);
            }
            if (Neon.Input.MouseCheck(MouseButton.RightButton) && currentWorld.IsActiveForm)
            {
                if (currentWorld.SelectedEntity != null)
                {
                    currentWorld.SelectedEntity.transform.Position += Neon.Input.DeltaMouse / currentWorld.Camera.Zoom;

                    foreach (Entity e in currentWorld.OtherSelectedEntities)
                        e.transform.Position += Neon.Input.DeltaMouse / currentWorld.Camera.Zoom;
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
                        foreach (Entity e in currentWorld.OtherSelectedEntities)
                        {
                            e.transform.Position = new Vector2((float)Math.Round((double)e.transform.Position.X), (float)Math.Round((double)e.transform.Position.Y));
                            if (currentWorld.MagnetismValue != 1)
                            {
                                int restX = (int)e.transform.Position.X % (int)currentWorld.MagnetismValue;
                                int restY = (int)e.transform.Position.Y % (int)currentWorld.MagnetismValue;

                                e.transform.Position += new Vector2((restX < currentWorld.MagnetismValue / 2 ? -restX : currentWorld.MagnetismValue - restX), (restY < currentWorld.MagnetismValue / 2 ? -restY : currentWorld.MagnetismValue - restY));
                            }
                            
                        }

                        ActionManager.SaveAction(ActionType.MovedEntity, new object[2] { currentWorld.SelectedEntity, new Vector2(_previousPosition.X, _previousPosition.Y) });
                    }
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
