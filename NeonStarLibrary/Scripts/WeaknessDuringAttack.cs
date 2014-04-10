using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Scripts
{
    public class WeaknessDuringAttack : Component
    {
        #region Properties
        private string _weaknessAttack = "";

        public string WeaknessAttack
        {
            get { return _weaknessAttack; }
            set { _weaknessAttack = value; }
        }

        private string _attackWeakTo = "";

        public string AttackWeakTo
        {
            get { return _attackWeakTo; }
            set { _attackWeakTo = value; }
        }
        #endregion

        public WeaknessDuringAttack(Entity entity)
            :base(entity, "WeaknessDuringAttack")
        {










        }
    }
}
