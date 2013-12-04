using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class NeutralizerGate : Component
    {

        public NeutralizerGate(Entity entity)
            :base(entity, "NeutralizerGate")
        {
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            Avatar _avatar = triggeringEntity.GetComponent<Avatar>();
            if(_avatar != null)
            {
                _avatar.ElementSystem.DropElement(Side.Left);
                _avatar.ElementSystem.DropElement(Side.Right);
            }
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }

    
}
