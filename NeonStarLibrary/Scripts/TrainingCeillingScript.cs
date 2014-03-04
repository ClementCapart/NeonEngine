using System;
using NeonEngine;
using NeonStarLibrary;
using NeonEngine.Private;
using Microsoft.Xna.Framework;
using NeonStarLibrary.Components.Avatar;

namespace NeonStarLibrary.Components.Scripts
{
    public class TrainingCeillingScript : ScriptComponent
    {
        private Entity _flyingBotIn;
        private Entity _flyingBotOut;

        public TrainingCeillingScript(Entity entity)
            :base(entity, "TrainingCeillingScript")
        {
        }
		
		public override void Init()
		{
            _flyingBotIn = entity.GameWorld.GetEntityByName("FlyingBotIn");
            _flyingBotOut = entity.GameWorld.GetEntityByName("FlyingBotOut");
		}

        public override void Update(GameTime gameTime)
        {
            if(_flyingBotIn != null && _flyingBotIn.transform.Position.X <= -1500)
            {
                _flyingBotIn.transform.Position += new Vector2(3200,0);
            }
            if (_flyingBotOut != null && _flyingBotOut.transform.Position.X >= 1500)
            {
                _flyingBotOut.transform.Position += new Vector2(-3200, 0);
            }
        }
    }
}
