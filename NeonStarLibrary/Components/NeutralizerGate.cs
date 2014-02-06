using NeonEngine;
using NeonStarLibrary.Components.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.GameplayElements
{
    public class NeutralizerGate : Component
    {

        private string _effectSpriteSheetTag = "";

        public string EffectSpriteSheetTag
        {
            get { return _effectSpriteSheetTag; }
            set { _effectSpriteSheetTag = value; }
        }

        private SpriteSheetInfo _effect;

        public NeutralizerGate(Entity entity)
            :base(entity, "NeutralizerGate")
        {
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            AvatarCore _avatar = triggeringEntity.GetComponent<AvatarCore>();
            if(_avatar != null)
            {
                _avatar.ElementSystem.LeftSlotLevel = 1;
                _avatar.ElementSystem.LeftSlotElement = Element.Neutral;

                _avatar.ElementSystem.RightSlotLevel = 1;
                _avatar.ElementSystem.RightSlotElement = Element.Neutral;

                //EffectsManager.GetEffect()
            }
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }

    
}
