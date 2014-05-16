using NeonEngine;
using NeonEngine.Components.Graphics2D;
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
        List<SpritesheetManager> _spritesheets;

        public DeviceActivatedComponent(Entity entity)
            :base (entity)
        {
            Name = "DeviceActivatedComponent";
        }

        public override void Init()
        {
            _componentToManage = entity.GetComponentByName(_componentName);
            _spritesheets = entity.GetComponentsByInheritance<SpritesheetManager>();
            base.Init();
       
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_componentToManage != null)
                _componentToManage.ComponentEnabled = _powered;


            if (_spritesheets != null)
            {
                foreach (SpritesheetManager ssm in _spritesheets)
                    if (_powered)
                        ssm.ChangeAnimation("On", 0, true, false, true);
                    else
                        ssm.ChangeAnimation("Off", 0, true, false, true);
            }

            base.Update(gameTime);
        }

        
    }
}
