using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeonStarLibrary.Entities;
using NeonEngine;

namespace NeonStarEditor
{
    public class AddEntity : Tool
    {
        Entity entity;

        public AddEntity(Entity entity, EditorScreen currentWorld)
            :base(currentWorld)
        {
            this.entity = entity;
            currentWorld.entityList.Add(entity);
        }

        public override void Update(GameTime gameTime)
        {
            entity.transform.Position = Neon.Input.MousePosition;
            if (Neon.Input.MousePressed(MouseButton.LeftButton))
            {
                Rigidbody rb = entity.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.body.Position = CoordinateConversion.screenToWorld(Neon.Input.MousePosition);
                currentWorld.Pause = false;
                currentWorld.CurrentTool = null;
            }
            base.Update(gameTime);
        }
        
    }
}
