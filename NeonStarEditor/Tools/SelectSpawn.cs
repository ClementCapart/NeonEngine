using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarEditor
{
    class SelectSpawn : Tool
    {

        public SelectSpawn(EditorScreen GameWorld)
            :base(GameWorld)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Neon.Input.MousePressed(MouseButton.LeftButton))
            {
                foreach (SpawnPoint sp in this.currentWorld.SpawnPoints)
                {
                    if (new Rectangle((int)sp.Position.X - 30, (int)sp.Position.Y - 30, 60, 60).Contains(new Point((int)Neon.Input.MousePosition.X, (int)Neon.Input.MousePosition.Y)))
                    {
                        currentWorld.SpawnPointsPanel.CurrentSpawnPointSelected = sp;
                        currentWorld.SpawnPointsPanel.InitializeSelectedData();
                    }
                }
            }

            if (Neon.Input.MouseCheck(MouseButton.RightButton))
            {
                if (currentWorld.SpawnPointsPanel.CurrentSpawnPointSelected != null)
                {
                    currentWorld.SpawnPointsPanel.CurrentSpawnPointSelected.Position += Neon.Input.DeltaMouse / currentWorld.Camera.Zoom;
                }
            }
            base.Update(gameTime);
        }
    }
}
