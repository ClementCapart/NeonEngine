using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Scripts
{
    public class DisableTigerScript : Component
    {

        private Entity _tigerToDestroy;
        private Entity _triggerToDisable;

        public DisableTigerScript(Entity entity)
            :base(entity, "DisableTigerScript")
        {

        }

        public override void Init()
        {
            _tigerToDestroy = entity.GameWorld.GetEntityByName("ScriptedTiger");
            _triggerToDisable = entity.GameWorld.GetEntityByName("ThunderScript");
            base.Init();
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            if (_tigerToDestroy != null)
                _tigerToDestroy.Destroy();
            if (_triggerToDisable != null)
            {
                _triggerToDisable.hitboxes[0].Width = 0;
                _triggerToDisable.hitboxes[0].Height = 0;
            }
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
