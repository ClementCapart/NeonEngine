using NeonEngine.Components.CollisionDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Triggers
{
    public class HitboxTrigger : Component
    {
        private bool _alreadyTouched = false;
        private bool _notOutYet = false;

        protected bool _oneShot = false;

        public bool OneShot
        {
            get { return _oneShot; }
            set { _oneShot = value; }
        }

        protected bool _triggerEveryFrame = false;

        public bool TriggerEveryFrame
        {
            get { return _triggerEveryFrame; }
            set { _triggerEveryFrame = value; }
        }

        protected string _triggeringEntityName = "";

        public string TriggeringEntityName
        {
            get { return _triggeringEntityName; }
            set { _triggeringEntityName = value; }
        }

        protected string _triggeredEntityName = "";

        public string TriggeredEntityName
        {
            get { return _triggeredEntityName; }
            set { _triggeredEntityName = value; }
        }

        protected string _triggeredComponentName = "";

        public string TriggeredComponentName
        {
            get { return _triggeredComponentName; }
            set { _triggeredComponentName = value; }
        }

        protected Entity _triggeringEntity = null;
        protected Entity _triggeredEntity = null;
        protected Component _triggeredComponent = null;

        public HitboxTrigger(Entity entity)
            :base(entity, "HitboxTrigger")
        {
            RequiredComponents = new Type[] { typeof(Hitbox) };
        }

        public override void Init()
        {
            _triggeringEntity = entity.GameWorld.GetEntityByName(_triggeringEntityName);
            _triggeredEntity = entity.GameWorld.GetEntityByName(_triggeredEntityName);
            if (_triggeredEntity != null)
                _triggeredComponent = _triggeredEntity.GetComponentByName(_triggeredComponentName);
            base.Init();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_triggeredEntity == null)
            {
                _triggeredEntity = entity.GameWorld.GetEntityByName(_triggeredEntityName);
                if(_triggeredEntity != null)
                    _triggeredComponent = _triggeredEntity.GetComponentByName(_triggeredComponentName);
            }

            if (_triggeringEntity == null)
            {
                _triggeringEntity = entity.GameWorld.GetEntityByName(_triggeringEntityName);
            }
            else
            {
                if (_triggeringEntity.hitboxes.Count > 0 && entity.hitboxes.Count > 0)
                {
                    if ((_oneShot && !_alreadyTouched) || !_oneShot)
                    {
                        if (_triggeringEntity.hitboxes[0].hitboxRectangle.Intersects(entity.hitboxes[0].hitboxRectangle))
                        {
                            if (!_notOutYet || _triggerEveryFrame)
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

        public virtual void Trigger()
        {
            if (_triggeredEntity != null && _triggeredComponent != null)
            {
                _triggeredComponent.OnTrigger(entity, _triggeringEntity);
            }
        }
    }
}
