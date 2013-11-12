using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class HitboxTrigger : Component
    {
        private bool _alreadyTouched = false;
        private bool _notOutYet = false;

        private bool _oneShot = false;

        public bool OneShot
        {
            get { return _oneShot; }
            set { _oneShot = value; }
        }

        private string _triggeringEntityName = "";

        public string TriggeringEntityName
        {
            get { return _triggeringEntityName; }
            set { _triggeringEntityName = value; }
        }

        private string _triggeredEntityName = "";

        public string TriggeredEntityName
        {
            get { return _triggeredEntityName; }
            set { _triggeredEntityName = value; }
        }

        private string _triggeredComponentName = "";

        public string TriggeredComponentName
        {
            get { return _triggeredComponentName; }
            set { _triggeredComponentName = value; }
        }

        private Entity _triggeringEntity = null;
        private Entity _triggeredEntity = null;
        private Component _triggeredComponent = null;

        public HitboxTrigger(Entity entity)
            :base(entity, "HitboxTrigger")
        {
        }

        public override void Init()
        {
            _triggeringEntity = entity.containerWorld.GetEntityByName(_triggeringEntityName);
            _triggeredEntity = entity.containerWorld.GetEntityByName(_triggeredEntityName);
            if (_triggeredEntity != null)
                _triggeredComponent = _triggeredEntity.GetComponentByName(_triggeredComponentName);
            base.Init();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_triggeredEntity == null)
            {
                _triggeredEntity = entity.containerWorld.GetEntityByName(_triggeredEntityName);
                if(_triggeredEntity != null)
                    _triggeredComponent = _triggeredEntity.GetComponentByName(_triggeredComponentName);
            }

            if (_triggeringEntity == null)
            {
                _triggeringEntity = entity.containerWorld.GetEntityByName(_triggeringEntityName);
            }
            else
            {
                if (_triggeringEntity.hitbox != null && entity.hitbox != null)
                {
                    if ((_oneShot && !_alreadyTouched) || !_oneShot)
                    {
                        if (_triggeringEntity.hitbox.hitboxRectangle.Intersects(entity.hitbox.hitboxRectangle))
                        {
                            if (!_notOutYet)
                            {
                                _alreadyTouched = true;
                                _notOutYet = true;
                                Trigger();
                            }
                        }
                        else
                        {
                            _notOutYet = false;
                        }
                    }               
                }
            }
 	        base.PreUpdate(gameTime);
        }

        public void Trigger()
        {
            if (_triggeredEntity != null && _triggeredComponent != null)
            {
                _triggeredComponent.OnTrigger(entity, _triggeringEntity);
            }
        }
    }
}
