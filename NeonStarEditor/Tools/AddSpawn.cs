using Microsoft.Xna.Framework.Graphics;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarEditor
{
    class AddSpawn : Tool
    {
        private Texture2D _addSpawnTexture;

        public AddSpawn(EditorScreen GameWorld)
            : base(GameWorld)
        {
            _addSpawnTexture = AssetManager.GetTexture("SpawnPointIcon");
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Neon.Input.MousePressed(MouseButton.LeftButton))
            {
                SpawnPoint sp = new SpawnPoint();
                sp.Side = Side.Right;
                sp.Position = Neon.Input.MousePosition;

                int index = 0;

                foreach (SpawnPoint spawnP in currentWorld.SpawnPoints)
                    if (index <= spawnP.Index)
                        index = spawnP.Index + 1;
                sp.Index = index;

                currentWorld.SpawnPoints.Add(sp);
                
                currentWorld.SpawnPointsPanel.InitializeSpawnPointData();
                currentWorld.SpawnPointsPanel.CurrentSpawnPointSelected = sp;
                currentWorld.SpawnPointsPanel.SpawnPointList.SelectedItem = sp;
                currentWorld.CurrentTool = new SelectSpawn(currentWorld);
            }

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_addSpawnTexture, Neon.Input.MousePosition, null, Microsoft.Xna.Framework.Color.White, 0f, new Microsoft.Xna.Framework.Vector2(_addSpawnTexture.Width / 2, _addSpawnTexture.Height / 2), 1f, SpriteEffects.None, 1f);
            base.Draw(spriteBatch);
        }
    }
}
