using NeonEngine;
using NeonEngine.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Scripts
{
    public class TrainingStockDoorScript : ScriptComponent
    {

        public TrainingStockDoorScript(Entity entity)
            : base(entity, "TrainingStockDoorScript")
        {
        }

        public override void Init()
        {

            this.entity.spritesheets.ChangeAnimation("Opened", 0, true, false, false);
            this.entity.rigidbody.IsGround = false;
            this.entity.rigidbody.Init();
            base.Init();
        }

        public override void OnTrigger(Entity trigger, NeonEngine.Entity triggeringEntity, object[] parameters = null)
        {

            this.entity.spritesheets.ChangeAnimation("Closing", 0, true, false, false);
            this.entity.rigidbody.IsGround = true;
            this.entity.rigidbody.Init();

            base.OnTrigger(trigger, triggeringEntity, parameters);
        }

    }
}
