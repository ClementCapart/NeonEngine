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
        private Vector2 _windImpulse;
        private float _impulseDuration;
        private float _airVerticalVelocity;
        private float _airMaxVerticalVelocity;
        private float _airControlSpeed;
        private float _timedGaugeConsumption;
        private string _attackName;

        private Attack _airAttack;

        public Wind(ElementSystem elementSystem, int elementLevel, Entity entity, NeonStarInput input, GameScreen world)
            :base(elementSystem, elementLevel, entity, input, world)
        {           
        }

        public override void InitializeLevelParameters()
        {           
            this.EffectElement = Element.Wind;

            _gaugeCost = (float)ElementManager.WindParameters[0][0];
            _windImpulse = new Vector2(0, (float)ElementManager.WindParameters[0][1]);
            _impulseDuration = (float)ElementManager.WindParameters[0][2];
            _attackName = (string)ElementManager.WindParameters[0][3];
            _timedGaugeConsumption = (float)ElementManager.WindParameters[0][4];
            _airControlSpeed = (float)ElementManager.WindParameters[0][5];
            _airVerticalVelocity = (float)ElementManager.WindParameters[0][6];
            _airMaxVerticalVelocity = (float)ElementManager.WindParameters[0][7];

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
                    if (_entity.rigidbody != null && _entity.rigidbody.isGrounded && _entity.spritesheets.CurrentSpritesheetName != _elementSystem.WindStartAnimation)
                    {
                        if (_entity.spritesheets != null && _entity.spritesheets.CurrentSpritesheetName != _elementSystem.WindStartAnimation)
                        {
                            _entity.spritesheets.ChangeAnimation(this._elementSystem.WindStartAnimation, 0, true, false, false);
                            switch (_input)
                            {
                                case NeonStarInput.UseLeftSlotElement:
                                    _elementSystem.LeftSlotEnergy -= _gaugeCost;
                                    break;

                                case NeonStarInput.UseRightSlotElement:
                                    _elementSystem.RightSlotEnergy -= _gaugeCost;
                                    break;
                            }

                            _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                        }
                        
                    }
                    else if (_entity.spritesheets != null && _entity.spritesheets.CurrentSpritesheetName == _elementSystem.WindStartAnimation && _entity.spritesheets.CurrentSpritesheet.IsFinished)
                    {
                        _airAttack = AttacksManager.StartFreeAttack(_attackName, Side.Left, _entity.transform.Position);
                        _entity.rigidbody.body.ApplyLinearImpulse(_windImpulse);
                        _entity.spritesheets.ChangeAnimation(_elementSystem.WindImpulseAnimation, 0, true, false, false);
                        State = ElementState.Charge;
                    }
                    else if(_entity.spritesheets != null && _entity.spritesheets.CurrentSpritesheetName != _elementSystem.WindStartAnimation)
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
