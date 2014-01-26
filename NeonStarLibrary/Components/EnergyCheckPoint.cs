using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public class EnergyCheckPoint : EnergyComponent
    {
        public EnergyCheckPoint(Entity entity)
            :base(entity)
        {
            Name = "EnergyCheckPoint";
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public override void PowerDevice()
        {
            base.PowerDevice();
        }

        public override void UnpowerDevice()
        {
            base.UnpowerDevice();
        }
    }
}
