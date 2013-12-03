using System;
using NeonEngine;
using NeonEngine.Private;
using NeonStarLibrary;
using Microsoft.Xna.Framework;

namespace NeonScripts
{
    public class RespawnConduitScript : ScriptComponent
    {
        public RespawnConduitScript(Entity entity)
            :base(entity, "RespawnConduitScript")
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
            triggeringEntity.rigidbody.body.LinearVelocity = (new Vector2(0.0f, 0.0f));
            triggeringEntity.transform.Position = (new Vector2(7600.0f, -1000.0f));
        }
    }
}
