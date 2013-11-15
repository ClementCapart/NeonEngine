using System;
using NeonEngine;
using NeonStarLibrary;
using NeonEngine.Private;
using Microsoft.Xna.Framework;

namespace NeonScripts
{
    public class Test : ScriptComponent
    {
        public Test(Entity entity)
            :base(entity, "Test")
        {
        }
		
		public override void Init()
		{
		}

        public override void Update(GameTime gameTime)
        {
        }
		
		public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
		{
			Console.WriteLine("HOY");
		}
    }
}
