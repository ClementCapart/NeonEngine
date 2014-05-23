using Microsoft.Xna.Framework;
using NeonEngine;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.HUD;
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

        private ElementHUD _elementHUD;

        private SpriteSheetInfo _effect;

        public NeutralizerGate(Entity entity)
            :base(entity, "NeutralizerGate")
        {
        }

        public override void Init()
        {
            Entity e = entity.GameWorld.GetEntityByName("HUD");
            if (e != null)
                _elementHUD = e.GetComponent<ElementHUD>();
            base.Init();
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            AvatarCore _avatar = triggeringEntity.GetComponent<AvatarCore>();
            if(_avatar != null && _avatar.ElementSystem != null)
            {
                bool removedElement = false;

                for (int i = 0; i < _avatar.ElementSystem.ElementSlots[0].Length; i++)
                {
                    if (_avatar.ElementSystem.ElementSlots[0][i].Type != Element.Neutral)
                    {
                        _elementHUD.LoseElement(i, 0);
                        removedElement = true;
                    }
                    _avatar.ElementSystem.ElementSlots[0][i].Type = Element.Neutral;

                }
                for (int i = 0; i < _avatar.ElementSystem.ElementSlots[1].Length; i++)
                {
                    if (_avatar.ElementSystem.ElementSlots[1][i].Type != Element.Neutral)
                    {
                        _elementHUD.LoseElement(i, 1);
                        removedElement = true;
                    }
                    _avatar.ElementSystem.ElementSlots[1][i].Type = Element.Neutral;
                }

                if (removedElement)
                {
                    Vector2 effectPosition;

                    if (_verticalGate)
                    {
                        effectPosition = new Vector2(entity.transform.Position.X, _avatar.entity.transform.Position.Y);
                    }
                    else
                    {
                        effectPosition = new Vector2(_avatar.entity.transform.Position.X, entity.transform.Position.Y);
                    }

                    if (_effect != null)
                        EffectsManager.GetEffect(_effect, Side.Right, effectPosition, 0.0f, Vector2.Zero, 2.0f, 0.99f);
                }
            }
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }

    
}
