using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class DoorTrigger : Component
    {
        private Door _doorComponent;

        private bool _4thDoorToOpen = false;

        public DoorTrigger(Entity entity)
            :base(entity, "DoorTrigger")
        {

        }

        public override void Init()
        {
            _doorComponent = this.entity.GetComponent<Door>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Buttons.Y) && _4thDoorToOpen)
            {
                _doorComponent.Switch();
                this.Remove();
            }
            base.Update(gameTime);
        }

        public override void FinalUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _4thDoorToOpen = false;
            base.FinalUpdate(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            if (trigger.Name == "04DoorButtonTrigger")
            {
                _4thDoorToOpen = true;
            }
            else if(_doorComponent != null)
            {
                _doorComponent.Switch();
            }

            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
