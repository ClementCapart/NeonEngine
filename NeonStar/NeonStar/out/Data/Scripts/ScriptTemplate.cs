using System;
using NeonEngine;
using NeonEngine.Private;
using NeonStarLibrary;
using Microsoft.Xna.Framework;

namespace NeonScripts
{
    public class ScriptName : ScriptComponent
    {
        public ScriptName(Entity entity)
            :base(entity, "ScriptName")
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
