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
            if (Neon.Input.MousePressed(MouseButton.LeftButton) && currentWorld.XNAWindow.Focused)
            {
                fixture = currentWorld.physicWorld.TestPoint(position);
                if (fixture != null)
                {
                    if (currentWorld.SelectedEntity != Neon.utils.GetEntityByBody(fixture.Body))
                    {
                        currentWorld.SelectedEntity = Neon.utils.GetEntityByBody(fixture.Body);
                        currentWorld.BottomDockControl.entityListControl.EntityListBox.SelectedItem = currentWorld.SelectedEntity;
                    }
                }
                else
                {
                    currentWorld.SelectedEntity = null;
                    currentWorld.BottomDockControl.entityListControl.EntityListBox.SelectedItem = null;
                }
            } 
        }

        private void Move()
        {
            if (Neon.Input.MousePressed(MouseButton.RightButton))
            {
                TransformState = DataManager.SaveComponentParameters(currentWorld.SelectedEntity.transform);
            }
            if (Neon.Input.MouseCheck(MouseButton.RightButton))
            {
                if (currentWorld.SelectedEntity != null)
                    currentWorld.SelectedEntity.transform.Position += Neon.Input.DeltaMouse / currentWorld.camera.Zoom ;
            }
            if (Neon.Input.MouseReleased(MouseButton.RightButton))
            {
                ActionManager.SaveAction(ActionType.ChangeEntityParameters, new object[2] { currentWorld.SelectedEntity.transform, TransformState });
                TransformState = null;
            }
        }
    }
}
