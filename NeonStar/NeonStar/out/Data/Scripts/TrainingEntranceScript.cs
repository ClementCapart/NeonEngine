using System;
using NeonEngine;
using NeonStarLibrary;
using NeonEngine.Private;
using Microsoft.Xna.Framework;
using NeonStarLibrary.Components.Avatar;

namespace NeonStarLibrary.Components.Scripts
{
    public class TrainingEntranceScript : ScriptComponent
    {
        private Entity _airWave;

        public TrainingEntranceScript(Entity entity)
            :base(entity, "TrainingEntranceScript")
        {
        }
		
		public override void Init()
		{
            _airWave = entity.GameWorld.GetEntityByName("AirWave");
		}

        public override void Update(GameTime gameTime)
        {
            if(_airWave != null && _airWave.transform.Position.X <= 0)
            {
                _airWave.transform.Position += new Vector2(1650,0);
            }
        }
    }
}
