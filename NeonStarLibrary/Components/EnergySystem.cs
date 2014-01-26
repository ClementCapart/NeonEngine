using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Avatar
{
    public class EnergySystem : Component
    {
        #region Properties

        private float _maxEnergyStock = 1000;

        public float MaxEnergyStock
        {
            get { return _maxEnergyStock; }
            set { _maxEnergyStock = value; }
        }

        private float _currentEnergyStock = 100.0f;

        public float CurrentEnergyStock
        {
            get { return _currentEnergyStock; }
            set { _currentEnergyStock = value; }
        }

        #endregion

        

        public AvatarCore AvatarComponent;

        public EnergySystem(Entity entity)
            :base(entity, "EnergySystem")
        {
        }

        public override void Init()
        {
            AvatarComponent = entity.GetComponent<AvatarCore>();
            base.Init();
        }

    }
}
