using System;
using NeonEngine;
using NeonStarLibrary;
using NeonEngine.Private;
using Microsoft.Xna.Framework;

namespace NeonScripts
{
    public class FireDoorScript : ScriptComponent
    {
        public FireDoorScript(Entity entity)
            :base(entity, "FireDoorScript")
        {
        }
		
		public override void Init()
		{
            //if (entity.spritesheets.Name == "FireDoorOpening")
                entity.spritesheets.ChangeAnimation("FireDoorOpening", 0, false, true, false, 0);
		}

        public override void Update(GameTime gameTime)
        {
        }
		
		public override void OnTrigger(Entity trigger, Entity triggeringEntity)
		{
		}
    }
}
