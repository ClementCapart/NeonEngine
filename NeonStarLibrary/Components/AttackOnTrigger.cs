using NeonEngine;
using NeonEngine.Components.Triggers;
using NeonStarLibrary.Components.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Triggers
{
    public class AttackOnTrigger : HitboxTrigger
    {
        private string _triggeringAttack = "";

        public string TriggeringAttack
        {
            get { return _triggeringAttack; }
            set { _triggeringAttack = value; }
        }

        AvatarCore _avatar;

        public AttackOnTrigger(Entity entity)
            :base(entity)
        {
            Name = "AttackOnTrigger";
        }

        public override void Init()
        {
            base.Init();
            if(_triggeringEntity != null)
                _avatar = _triggeringEntity.GetComponent<AvatarCore>();
        }

        public override void Trigger()
        {
            if (_avatar != null && _triggeredEntity != null && _triggeredComponent != null)
            {
                if (_avatar.MeleeFight != null && _avatar.MeleeFight.CurrentAttack != null && _avatar.MeleeFight.CurrentAttack.Name == _triggeringAttack)
                {
                    _triggeredComponent.OnTrigger(entity, _triggeringEntity);        
                }
            }
            
        }
    }
}
