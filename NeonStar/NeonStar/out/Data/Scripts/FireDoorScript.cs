﻿using System;
using NeonEngine;
using NeonStarLibrary;
using NeonEngine.Private;
using Microsoft.Xna.Framework;

namespace NeonScripts
{
    public class FireDoorScript : ScriptComponent
    {
        private Enemy _enemyComponent = null;

        public FireDoorScript(Entity entity)
            :base(entity, "FireDoorScript")
        {
        }
		
		public override void Init()
		{
            _enemyComponent = entity.GetComponent<Enemy>();
			entity.spritesheets.ChangeAnimation("FireDoorOpening", 0, false, true, false, 0);
		}

        public override void Update(GameTime gameTime)
        {
        }
		
		public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
		{
			if(((Attack)parameters[0]).EffectElement == Element.Fire)
			{
				entity.spritesheets.ChangeAnimation("FireDoorOpening", 0, true, true, false, 0);
				entity.rigidbody.Remove();
                _enemyComponent.Remove();
			}		
		}
    }
}
