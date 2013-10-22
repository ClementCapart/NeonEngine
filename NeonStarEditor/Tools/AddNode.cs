using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarEditor
{
    class AddNode : Tool
    {
        PathNodeList pathNodeList;

        public AddNode(PathNodeList pnl, EditorScreen GameWorld)
            :base(GameWorld)
        {
            this.pathNodeList = pnl;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Neon.Input.MousePressed(MouseButton.LeftButton))
            {
                Node n = new Node();
                n.index = pathNodeList.Nodes.Count;
                n.Type = NodeType.Move;
                n.Position = Neon.Input.MousePosition;

                pathNodeList.Nodes.Add(n);
                currentWorld.PathNodePanel.CurrentNodeSelected = n;
                currentWorld.PathNodePanel.InitializeNodeData();
            }

            base.Update(gameTime);
        }
    }
}
