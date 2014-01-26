using System;
using NeonEngine;
using NeonStarLibrary;
using NeonEngine.Private;
using Microsoft.Xna.Framework;
using NeonStarLibrary.Components.Enemies;
using NeonStarLibrary.Components.Avatar;

namespace NeonScripts
{
    public class FireDoorScript : ScriptComponent
    {
        private EnemyCore _enemyComponent = null;

        public FireDoorScript(Entity entity)
            :base(entity, "FireDoorScript")
        {
        }
		
		public override void Init()
		{
            _enemyComponent = entity.GetComponent<EnemyCore>();
			entity.spritesheets.ChangeAnimation("FireDoorOpening", 0, false, true, false, 0);
		}

        public override void Update(GameTime gameTime)
        {
        }
		
		public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
		{
			if(((Attack)parameters[0]).AttackElement == Element.Fire)
			{
				entity.spritesheets.ChangeAnimation("FireDoorOpening", 0, true, true, false, 0);
				entity.rigidbody.Remove();
                _enemyComponent.Remove();
			}		
		}
    }
}
