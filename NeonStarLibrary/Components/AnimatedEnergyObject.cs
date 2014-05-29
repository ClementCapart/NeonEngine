using NeonEngine;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public class AnimatedEnergyObject : EnergyComponent
    {
        private string _poweredAnimation = "";

        public string PoweredAnimation
        {
            get { return _poweredAnimation; }
            set { _poweredAnimation = value; }
        }

        private string _unpoweredAnimation = "";

        public string UnpoweredAnimation
        {
            get { return _unpoweredAnimation; }
            set { _unpoweredAnimation = value; }
        }

        private List<SpritesheetManager> _spritesheets;

        public AnimatedEnergyObject(Entity entity)
            :base(entity)
        {
            Name = "AnimatedEnergyObject";
        }

        public override void Init()
        {
            _spritesheets = entity.GetComponentsByInheritance<SpritesheetManager>();
            base.Init();
            if (_powered)
            {
                if (_spritesheets != null)
                    foreach (SpritesheetManager ssm in _spritesheets)
                        ssm.ChangeAnimation(_poweredAnimation, 0, true, false, true);
            }
            else
            {
                if (_spritesheets != null)
                    foreach (SpritesheetManager ssm in _spritesheets)
                        ssm.ChangeAnimation(_unpoweredAnimation, 0, true, false, true);
            }
        }

        public override void PowerDevice()
        {
            if (_spritesheets != null)
                foreach(SpritesheetManager ssm in _spritesheets)
                    ssm.ChangeAnimation(_poweredAnimation, 0, true, false, true);
            base.PowerDevice();
        }

        public override void UnpowerDevice()
        {
            if (_spritesheets != null)
                foreach (SpritesheetManager ssm in _spritesheets)
                ssm.ChangeAnimation(_unpoweredAnimation, 0, true, false, true);
            base.UnpowerDevice();
        }
    }
}
