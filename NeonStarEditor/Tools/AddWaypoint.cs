using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Private;

namespace NeonStarEditor
{
    public class AddWaypoint : Tool
    {
        public AddWaypoint(EditorScreen currentWorld)
            : base(currentWorld)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(currentWorld.SelectedEntity == null)
            {
                currentWorld.CurrentTool = null;
            }
            if (Neon.Input.MousePressed(MouseButton.LeftButton) && currentWorld.XNAWindow.Focused)
            {
                Waypoints w = currentWorld.SelectedEntity.GetComponent<Waypoints>();
                if (w != null)
                {
                    if (w.waypoints.Count == 0)
                        w.AddWaypoint(w.entity.transform.Position);
                    w.AddWaypoint(new Vector2(Neon.Input.MousePosition.X, w.waypoints[w.waypoints.Count - 1].Y));    
                }
            }
            base.Update(gameTime);
        }
    }
}
