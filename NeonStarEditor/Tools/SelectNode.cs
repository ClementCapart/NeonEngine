using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarEditor
{
    class SelectNode : Tool
    {
        PathNodeList pathNodeList;

        public SelectNode(PathNodeList pnl, EditorScreen GameWorld)
            :base(GameWorld)
        {
            this.pathNodeList = pnl;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Neon.Input.MousePressed(MouseButton.LeftButton))
            {
                foreach (Node n in pathNodeList.Nodes)
                {
                    if (new Rectangle((int)n.Position.X - 30, (int)n.Position.Y - 30, 60, 60).Contains(new Point((int)Neon.Input.MousePosition.X, (int)Neon.Input.MousePosition.Y)))
                    {
                        currentWorld.PathNodePanel.CurrentNodeSelected = n;
                        currentWorld.PathNodePanel.InitializeNodeData();
                    }
                }
            }

            if (Neon.Input.MouseCheck(MouseButton.RightButton))
            {
                if (currentWorld.PathNodePanel.CurrentNodeSelected != null)
                {
                    currentWorld.PathNodePanel.CurrentNodeSelected.Position += Neon.Input.DeltaMouse / currentWorld.camera.Zoom;
                }
            }
            base.Update(gameTime);
        }
    }
}
