using NeonEngine;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.EnergyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.GameplayElements
{
    public class EnergySwitch : Component
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

        private string _entityToEnergize = "";

        public string EntityToEnergize
        {
            get { return _entityToEnergize; }
            set { _entityToEnergize = value; }
        }

        #endregion

        private float _activationTimer = 0.0f;
        private bool _activated = false;
        Entity _entityEnergize;
        List<EnergyComponent> _energyComponents;

        public EnergySwitch(Entity entity)
            :base(entity, "EnergySwitch")
        {
        }

        public override void Init()
        {
            _entityEnergize = entity.containerWorld.GetEntityByName(_entityToEnergize);
            if (_entityEnergize != null)
            {
                _energyComponents = _entityEnergize.GetComponentsByInheritance<EnergyComponent>();
            }

            
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
                    foreach (EnergyComponent ec in _energyComponents)
                        ec.UnpowerDevice();
                }
                else
                {
                    if (!_activated)
                    {
                        _activated = true;
                        foreach (EnergyComponent ec in _energyComponents)
                            ec.PowerDevice();
                            
                    }
                }
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
                    if (a.Name == _attackAddingTimeFromStunLock)
                        _activationTimer += a.StunLock;
                }
            }
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
