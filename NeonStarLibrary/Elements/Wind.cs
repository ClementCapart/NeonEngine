using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class Wind : ElementEffect
    {
        private Vector2 _windImpulse = new Vector2(0, -500);
        private float _impulseDuration = 0.15f;
        private float _airVerticalVelocity = -1.0f;
        private float _airMaxVerticalVelocity = -4.0f;
        private float _airControlSpeed = 1.5f;
        private float _timedGaugeConsumption = 30.0f;
        private Vector2 _enemyWindImpulse = new Vector2(0, -1000);
        private string _attackName = "AirAttack";

        private Attack _airAttack;

        public Wind(ElementSystem elementSystem, int elementLevel, Entity entity, NeonStarInput input, GameScreen world)
            :base(elementSystem, elementLevel, entity, input, world)
        {           
        }

        public override void InitializeLevelParameters()
        {           
            this.EffectElement = Element.Wind;
            switch (ElementLevel)
            {
                case 1:
                    _gaugeCost = 30.0f;
                    break;

                case 2:                    
                    break;

                case 3:                  
                    break;
            }
            base.InitializeLevelParameters();
        }

        public override void PreUpdate(GameTime gameTime)
        {
            switch (State)
            {
                case ElementState.Initialization:
                case ElementState.Charge:
                    _elementSystem.AvatarComponent.CanMove = false;
                    _elementSystem.AvatarComponent.CanTurn = false;
                    _elementSystem.AvatarComponent.CanAttack = false;
                    _elementSystem.AvatarComponent.CanUseElement = false;
                    _entity.rigidbody.GravityScale = 0.0f;
                    break;

                case ElementState.Effect:
                    _elementSystem.AvatarComponent.CanUseElement = false;
                    _elementSystem.AvatarComponent.CanAttack = false;
                    _elementSystem.AvatarComponent.ThirdPersonController.CurrentAirMaxSpeed = _airControlSpeed;
                    _entity.rigidbody.GravityScale = 0.0f;
                    break;
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch(State)
            {
                case ElementState.Initialization:
                    switch (_input)
                    {
                        case NeonStarInput.UseLeftSlotElement:
                            _elementSystem.LeftSlotEnergy -= _gaugeCost;
                            break;

                        case NeonStarInput.UseRightSlotElement:
                            _elementSystem.RightSlotEnergy -= _gaugeCost;
                            break;
                    }
                    
                    if (_entity.rigidbody != null && _entity.rigidbody.isGrounded)
                    {
                        _airAttack = AttacksManager.StartFreeAttack(_attackName, Side.Left, _entity.transform.Position);
                        _entity.rigidbody.body.ApplyLinearImpulse(_windImpulse);
                        State = ElementState.Charge;
                    }
                    else
                        State = ElementState.Effect;
                    break;

                case ElementState.Charge:
                    if (_impulseDuration > 0.0f)
                    {
                        _impulseDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        if (!Neon.Input.Check(this._input))
                            State = ElementState.End;
                        else
                            State = ElementState.Effect;
                        if (_entity.rigidbody != null)
                            _entity.rigidbody.body.LinearVelocity = new Vector2(_entity.rigidbody.body.LinearVelocity.X, _airMaxVerticalVelocity);
                    }
                    break;

                case ElementState.Effect:
                    if (Neon.Input.Released(this._input))
                        State = ElementState.End;
                    else
                    {
                        bool stillFloating = false;

                        switch (_input)
                        {
                            case NeonStarInput.UseLeftSlotElement:
                                if (_elementSystem.LeftSlotEnergy > _timedGaugeConsumption * (float)gameTime.ElapsedGameTime.TotalSeconds)
                                {
                                    _elementSystem.LeftSlotEnergy -= _timedGaugeConsumption * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    if (_elementSystem.LeftSlotEnergy < 0.0f)
                                        _elementSystem.LeftSlotEnergy = 0;
                                    stillFloating = true;
                                }
                                else
                                    State = ElementState.End;
                                break;

                            case NeonStarInput.UseRightSlotElement:
                                if (_elementSystem.RightSlotEnergy > _timedGaugeConsumption * (float)gameTime.ElapsedGameTime.TotalSeconds)
                                {
                                    _elementSystem.RightSlotEnergy -= _timedGaugeConsumption * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    if (_elementSystem.RightSlotEnergy < 0.0f)
                                        _elementSystem.RightSlotEnergy = 0;
                                    stillFloating = true;
                                }
                                else
                                    State = ElementState.End;
                                break;
                        }

                        if (_entity.rigidbody != null && stillFloating)
                        {
                            _entity.rigidbody.body.LinearVelocity += new Vector2(0, _airVerticalVelocity);
                            if (_entity.rigidbody.body.LinearVelocity.Y < _airMaxVerticalVelocity)
                                _entity.rigidbody.body.LinearVelocity = new Vector2(_entity.rigidbody.body.LinearVelocity.X, _airMaxVerticalVelocity);
                        }
                    }
                    break;

                case ElementState.End:
                    break;
            }
            base.Update(gameTime);
        }
    }
}
