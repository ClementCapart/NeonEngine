using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class Fire : ElementEffect
    {
        public double CurrentCharge = 0.0f;
        private double _chargeSpeed;

        private double _maxCharge = 100.0f;
        private float _maxChargeTimer;

        private float _airlockLaunched = 0.3f;

        private bool _chargeGoingDown = false;

        public float StageTwoThreshold;
        public float StageThreeThreshold;
        public float StageFourThreshold;

        public string StageOneAttack;
        public string StageTwoAttack;
        public string StageThreeAttack;
        public string StageFourAttack; 

        public Fire(ElementSystem elementSystem, int elementLevel, Entity entity, NeonStarInput input, GameScreen world)
            :base(elementSystem, elementLevel, entity, input, world)
        {
            EffectElement = Element.Fire;
        }

        public override void InitializeLevelParameters()
        {
            switch(this._elementLevel)
            {
                case 1:
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
            switch(State)
            {
                case ElementState.Initialization:
                case ElementState.Charge:
                    _elementSystem.AvatarComponent.CanMove = false;
                    _elementSystem.AvatarComponent.CanTurn = false;
                    _elementSystem.AvatarComponent.CanAttack = false;
                    break;
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (State)
            {
                case ElementState.Initialization:
                    State = ElementState.Charge;
                    break;

                case ElementState.Charge:
                    if (Neon.Input.Check(_input))
                    {
                        _maxChargeTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (_maxChargeTimer <= 0.0f)
                        {
                            CurrentCharge = 0.0f;
                            State = ElementState.Effect;
                        }
                        if (CurrentCharge < _maxCharge && !_chargeGoingDown)
                        {
                            CurrentCharge += gameTime.ElapsedGameTime.TotalSeconds * _chargeSpeed;
                            if (CurrentCharge >= _maxCharge)
                            {
                                CurrentCharge = _maxCharge;
                                _chargeGoingDown = true;
                            }
                        }
                        else if(CurrentCharge > 0.0f)
                        {
                            CurrentCharge -= gameTime.ElapsedGameTime.TotalSeconds * _chargeSpeed;
                            if (CurrentCharge <= 0.0f)
                            {
                                CurrentCharge = 0.0f;
                                _chargeGoingDown = false;
                            }
                        }
                    }
                    else if(Neon.Input.Released(_input))
                    {
                        State = ElementState.Effect;
                    }
                    break;

                case ElementState.Effect:
                    _elementSystem.AvatarComponent.AirLock(_airlockLaunched);
                    _elementSystem.entity.rigidbody.body.LinearVelocity = Vector2.Zero;

                    if (CurrentCharge > StageFourThreshold)
                    {
                        Attack a = AttacksManager.StartFreeAttack(StageFourAttack, _entity.spritesheets.CurrentSide, _entity.transform.Position);
                        a.Launcher = _entity;
                    }
                    else if (CurrentCharge > StageThreeThreshold)
                    {
                        Attack a = AttacksManager.StartFreeAttack(StageThreeAttack, _entity.spritesheets.CurrentSide, _entity.transform.Position);
                        a.Launcher = _entity;
                    }
                    else if (CurrentCharge > StageTwoThreshold)
                    {
                        Attack a = AttacksManager.StartFreeAttack(StageTwoAttack, _entity.spritesheets.CurrentSide, _entity.transform.Position);
                        a.Launcher = _entity;
                    }
                    else
                    {
                        Attack a = AttacksManager.StartFreeAttack(StageOneAttack, _entity.spritesheets.CurrentSide, _entity.transform.Position);
                        a.Launcher = _entity;
                    }
                    
                    switch(_input)
                    {
                        case NeonStarInput.UseLeftSlotElement:
                            _elementSystem.LeftSlotEnergy -= this._gaugeCost;
                            break;

                        case NeonStarInput.UseRightSlotElement:
                            _elementSystem.RightSlotEnergy -= this._gaugeCost;
                            break;
                    }
                    State = ElementState.End;
                    break;

                case ElementState.End:
                    break;
            }

            base.Update(gameTime);
        }

        public override void End()
        {
            base.End();
        }
    }
}
