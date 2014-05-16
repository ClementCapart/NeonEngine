using NeonEngine;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.EnergyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public class TemporaryEnergyDevice : EnergyDevice
    {
        #region Properties
        private bool _triggerFromAllElementsExceptNeutral = true;

        public bool TriggerFromAllElementsExceptNeutral
        {
            get { return _triggerFromAllElementsExceptNeutral; }
            set { _triggerFromAllElementsExceptNeutral = value; }
        }

        private Element _triggeringElement;

        public Element TriggeringElement
        {
            get { return _triggeringElement; }
            set { _triggeringElement = value; }
        }

        private float _activationDuration = 2f;

        public float ActivationDuration
        {
            get { return _activationDuration; }
            set { _activationDuration = value; }
        }

        private string _attackAddingTimeFromStunLock = "";

        public string AttackAddingTimeFromStunLock
        {
            get { return _attackAddingTimeFromStunLock; }
            set { _attackAddingTimeFromStunLock = value; }
        }

        private string _offAnimation = "";

        public string OffAnimation
        {
            get { return _offAnimation; }
            set { _offAnimation = value; }
        }

        private string _onAnimation = "";

        public string OnAnimation
        {
            get { return _onAnimation; }
            set { _onAnimation = value; }
        }
        #endregion

        private float _activationTimer = 0.0f;
        private bool _activated = false;

        public TemporaryEnergyDevice(Entity entity)
            :base(entity)
        {
            Name = "TemporaryEnergyDevice";
        }

        public override void Init()
        {         
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_activationTimer > 0)
            {
                _activationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_activationTimer <= 0)
                {
                    _activationTimer = 0.0f;
                    _activated = false;
                    this.DeactivateDevice();
                }
                else
                {
                    if (!_activated)
                    {
                        _activated = true;
                        this.ActivateDevice();                           
                    }
                }
            }
            if (entity.spritesheets != null)
            {
                if (_activated)
                    entity.spritesheets.ChangeAnimation(_onAnimation, true, 0, true, false, true);
                else
                    entity.spritesheets.ChangeAnimation(_offAnimation, true, 0, true, false, true);
            }
            
            base.Update(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            if (trigger == entity && parameters.Length > 0)
            {
                Attack a = parameters[0] as Attack;
                if ((a.AttackElement != Element.Neutral && _triggerFromAllElementsExceptNeutral) || (a.AttackElement == _triggeringElement && !_triggerFromAllElementsExceptNeutral))
                {
                    _activationTimer = _activationDuration;
                    /*if (a.Name == _attackAddingTimeFromStunLock)
                        _activationTimer += a.StunLock;*/
                }
            }
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
