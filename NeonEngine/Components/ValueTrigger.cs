using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NeonEngine
{
    public class ValueTrigger : Component
    {
        private bool _alreadyTriggered = false;

        private bool _oneShot = false;

        public bool OneShot
        {
            get { return _oneShot; }
            set { _oneShot = value; }
        }

        private string _triggeringComponentName = "";

        public string TriggeringComponentName
        {
            get { return _triggeringComponentName; }
            set { _triggeringComponentName = value; }
        }

        private string _triggeringVariableName = "";

        public string TriggeringVariableName
        {
            get { return _triggeringVariableName; }
            set { _triggeringVariableName = value; }
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

        private Component _triggeringComponent = null;
        private PropertyInfo _triggeringProperty = null;

        private object _lastValue = null;

        private Entity _triggeredEntity = null;
        private Component _triggeredComponent = null;



        public ValueTrigger(Entity entity)
            :base(entity, "ValueTrigger")
        {
        }

        public override void Init()
        {
            _triggeringComponent = entity.GetComponentByName(_triggeringComponentName);
            if (_triggeringComponent != null)
            {
                _triggeringProperty = _triggeringComponent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.Name == _triggeringVariableName).First();
                if(_triggeringProperty != null)
                    _lastValue = _triggeringProperty.GetValue(_triggeringComponent, null);
            }

            _triggeredEntity = entity.containerWorld.GetEntityByName(_triggeredEntityName);
            if(_triggeredEntity != null)
                _triggeredComponent = entity.GetComponentByName(_triggeredComponentName);
            
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_triggeringComponent == null)
            {
                _triggeringComponent = entity.GetComponentByName(_triggeringComponentName);
                if (_triggeredComponent != null)
                {
                    _triggeringProperty = _triggeringComponent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.Name == _triggeringVariableName).First();
                    if(_triggeringProperty != null)
                        _lastValue = _triggeringProperty.GetValue(_triggeringComponent, null);
                }
            }

            if (_triggeredEntity == null)
            {
                _triggeredEntity = entity.containerWorld.GetEntityByName(_triggeredEntityName);
                if (_triggeredEntity != null)
                    _triggeredComponent = entity.GetComponentByName(_triggeredComponentName);
            }

            base.Update(gameTime);
        }

        public override void PostUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if ((OneShot && !_alreadyTriggered) || !OneShot)
            {
                if (_triggeringProperty != null)
                    if (_lastValue.GetHashCode() != _triggeringProperty.GetValue(_triggeringComponent, null).GetHashCode())
                        if (_triggeredComponent != null)
                        {
                            _alreadyTriggered = true;
                            _triggeredComponent.OnTrigger(entity, entity);
                        }
                if (_triggeringProperty != null)
                    _lastValue = _triggeringProperty.GetValue(_triggeringComponent, null);
            }
            
            base.PostUpdate(gameTime);
        }
    }
}
