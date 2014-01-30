using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NeonEngine.Components.Text2D
{
    public class DisplayValue : Component
    {
        private TextDisplay _display = null;

        public TextDisplay Display
        {
            get { return _display; }
            set { _display = value; }
        }

        private string _entityName = "";

        public string EntityName
        {
            get { return _entityName; }
            set { _entityName = value; }
        }

        private string _componentName = "";

        public string ComponentName
        {
            get { return _componentName; }
            set { _componentName = value; }
        }

        private string _displayedVariable = "";

        public string DisplayedVariable
        {
            get { return _displayedVariable; }
            set { _displayedVariable = value; }
        }

        private Entity _valueEntity;
        private Component _valueComponent;
        private PropertyInfo _propertyValue;

        private float _targetValue;
        private float _currentValue;

        public DisplayValue(Entity entity)
            :base(entity, "DisplayValue")
        {

        }

        public override void Init()
        {
            if (_entityName != "")
                _valueEntity = entity.GameWorld.GetEntityByName(_entityName);
            if (_valueEntity != null)
                _valueComponent = _valueEntity.GetComponentByName(_componentName);
            if (_valueComponent != null)
                _propertyValue = _valueComponent.GetType().GetProperty(_displayedVariable);
            base.Init();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_valueComponent != null)
                if (_propertyValue != null)
                    _targetValue = (float)_propertyValue.GetValue(_valueComponent, null);

            SmoothInterpolate();

            if(_display != null)
                _display.Text = _currentValue < 10 && _currentValue > -1 ? "0"+Math.Floor(_currentValue).ToString() : Math.Floor(_currentValue).ToString();
            base.PreUpdate(gameTime);
        }

        public  void SmoothInterpolate()
        {
            if (_currentValue != _targetValue && Math.Abs(_currentValue - _targetValue) > 1.0f)
                _currentValue = MathHelper.Lerp(_currentValue, _targetValue, 0.1f);
            else
                _currentValue = _targetValue;
        }
    }
}
