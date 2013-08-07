using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NeonStarEditor
{
    class Selection : Tool
    {

        public Selection(EditorScreen currentWorld)
            :base(currentWorld)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
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
                        currentWorld.editorForm.entityList.EntityListBox.SelectedItem = currentWorld.SelectedEntity;
                    }
                }
                else
                {
                    currentWorld.SelectedEntity = null;
                    currentWorld.editorForm.entityList.EntityListBox.SelectedItem = null;
                }
            }
            if (Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Keys.Delete))
            {
                if (currentWorld.SelectedEntity != null)
                    currentWorld.SelectedEntity.Destroy();
            } 
        }

        private void Move()
        {
            if (Neon.Input.MouseCheck(MouseButton.RightButton))
            {
                if (currentWorld.SelectedEntity != null)
                    currentWorld.SelectedEntity.transform.Position += Neon.Input.DeltaMouse / currentWorld.camera.Zoom ;
            }
        }
    }
}
