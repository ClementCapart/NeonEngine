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
            Console.WriteLine(currentWorld.IsActiveForm);
            if (Neon.Input.MousePressed(MouseButton.LeftButton) && currentWorld.IsActiveForm)
            {
                fixture = currentWorld.physicWorld.TestPoint(position);
                if (fixture != null)
                {
                    if (currentWorld.SelectedEntity != Neon.utils.GetEntityByBody(fixture.Body))
                    {
                        currentWorld.SelectedEntity = Neon.utils.GetEntityByBody(fixture.Body);
                        currentWorld.BottomDockControl.entityListControl.EntityListBox.SelectedItem = Neon.utils.GetEntityByBody(fixture.Body);
                        currentWorld.FocusedTextBox = null;
                        currentWorld.FocusedNumericUpDown = null;
                    }
                }
                else
                {
                    bool NoSelection = true;

                    foreach (Entity e in currentWorld.entities)
                    {
                        if (Neon.Input.MousePosition.X >= e.transform.Position.X - 30 && Neon.Input.MousePosition.X <= e.transform.Position.X + 30
                            && Neon.Input.MousePosition.Y >= e.transform.Position.Y - 30 && Neon.Input.MousePosition.Y <= e.transform.Position.Y + 30)
                        {
                            currentWorld.SelectedEntity = e;
                            currentWorld.BottomDockControl.entityListControl.EntityListBox.SelectedItem = e;
                            currentWorld.FocusedTextBox = null;
                            currentWorld.FocusedNumericUpDown = null;
                            NoSelection = false;
                            break;
                        }

                    }

                    if (NoSelection)
                    {
                        currentWorld.SelectedEntity = null;
                        currentWorld.BottomDockControl.entityListControl.EntityListBox.SelectedItem = null;
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
                    currentWorld.SelectedEntity.transform.Position += Neon.Input.DeltaMouse / currentWorld.camera.Zoom ;
            }
            if (Neon.Input.MouseReleased(MouseButton.RightButton))
            {
                if (currentWorld.SelectedEntity != null)
                {
                    ActionManager.SaveAction(ActionType.ChangeEntityParameters, new object[2] { currentWorld.SelectedEntity.transform, TransformState });
                    TransformState = null;
                }
                
            }
        }
    }
}
