using NeonEngine;
using NeonStarLibrary.Components.EnergyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public class DeviceActivatedComponent : EnergyComponent
    {
        private string _componentName = "";

        public string ComponentName
        {
            get { return _componentName; }
            set { _componentName = value; }
        }

        private Component _componentToManage;

        public DeviceActivatedComponent(Entity entity)
            :base (entity)
        {
            Name = "DeviceActivatedComponent";
        }

        public override void Init()
        {
            _componentToManage = entity.GetComponentByName(_componentName);
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_componentToManage != null)
                _componentToManage.ComponentEnabled = _powered;

            base.Update(gameTime);
        }

        
    }
}
