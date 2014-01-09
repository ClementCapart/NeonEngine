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
                Neon.World.ChangeScreen(new GameScreen(Neon.World.LevelGroupName, Neon.World.LevelName, 0, Neon.Game));
            base.Update(gameTime);
        }
    }
}
