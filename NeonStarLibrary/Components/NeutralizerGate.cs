using Microsoft.Xna.Framework;
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

        private bool _verticalGate = true;

        public bool VerticalGate
        {
            get { return _verticalGate; }
            set { _verticalGate = value; }
        }

        private string _effectSpriteSheetTag = "";

        public string EffectSpriteSheetTag
        {
            get { return _effectSpriteSheetTag; }
            set 
            { 
                _effectSpriteSheetTag = value;
                _effect = AssetManager.GetSpriteSheet(_effectSpriteSheetTag);
            }
        }

        private SpriteSheetInfo _effect;

        public NeutralizerGate(Entity entity)
            :base(entity, "NeutralizerGate")
        {
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            AvatarCore _avatar = triggeringEntity.GetComponent<AvatarCore>();
            if(_avatar != null && _avatar.ElementSystem != null && (_avatar.ElementSystem.LeftSlotElement != Element.Neutral || _avatar.ElementSystem.RightSlotElement != Element.Neutral))
            {

                _avatar.ElementSystem.LeftSlotLevel = 1;
                _avatar.ElementSystem.LeftSlotElement = Element.Neutral;

                _avatar.ElementSystem.RightSlotLevel = 1;
                _avatar.ElementSystem.RightSlotElement = Element.Neutral;

                Vector2 effectPosition;

                if (_verticalGate)
                {
                    effectPosition = new Vector2(entity.transform.Position.X, _avatar.entity.transform.Position.Y);
                }
                else
                {
                    effectPosition = new Vector2(_avatar.entity.transform.Position.X, entity.transform.Position.Y);
                }

                if(_effect != null)
                    EffectsManager.GetEffect(_effect, Side.Right, effectPosition, 0.0f, Vector2.Zero, 2.0f, 0.99f);
            }
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }

    
}
