using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class DummyScript : Component
    {

        private bool _activated = false;
        private Entity _dummy = null;
        private Door _doorToOpen = null;

        public DummyScript(Entity entity)
            :base(entity, "DummyScript")
        {

        }

        public override void Init()
        {
            _doorToOpen = entity.containerWorld.GetEntityByName("02Door").GetComponent<Door>();
            entity.spritesheets.ChangeAnimation("Off");
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_activated)
            {
                if (Math.Abs(_dummy.transform.Position.X - entity.transform.Position.X) > 1.0f)
                    _dummy.transform.Position = new Vector2(MathHelper.Lerp(_dummy.transform.Position.X, entity.transform.Position.X, 0.05f), _dummy.transform.Position.Y);
                else
                {
                    entity.spritesheets.ChangeAnimation("On");
                    _activated = false;
                    _doorToOpen.OpenDoor();

                }
            }
            base.Update(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            _activated = true;
            _dummy = triggeringEntity;
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
