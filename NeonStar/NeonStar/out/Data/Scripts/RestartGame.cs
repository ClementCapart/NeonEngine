using System;
using System.Collections.Generic;
using System.Text;
using NeonEngine.Private;
using NeonEngine;
using NeonStarLibrary;
using Microsoft.Xna.Framework;

namespace NeonStarScripts
{
    class RestartGame : ScriptComponent
    {
        public RestartGame(Entity entity)
            : base(entity, "RestartGame")
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Buttons.Back))
                Neon.world.ChangeScreen(new GameScreen(Neon.world.levelFilePath, 0, Neon.game));
            base.Update(gameTime);
        }
    }
}
