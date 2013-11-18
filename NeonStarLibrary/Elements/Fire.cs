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
        private double _charge = 0.0f;
        private double _chargeSpeed = 200.0f;

        private double _maxCharge = 100.0f;
        private float _airLockDuration = 3.0f;
        private float _maxChargeTimer = 3.0f;

        private bool _chargeGoingDown = false;

        private int _levelOneStageTwoChargeRequired = 30;
        private int _levelOneStageThreeChargeRequired = 70;

        private int _levelTwoStageTwoChargeRequired = 30;
        private int _levelTwoStageThreeChargeRequired = 70;
        private int _levelTwoStageFourChargeRequired = 90;

        private int _levelThreeStageTwoChargeRequired = 30;
        private int _levelThreeStageThreeChargeRequired = 70;
        private int _levelThreeStageFourChargeRequired = 90;

        private string _levelOneFireAttackNameStage1 = "LiOnElementFire11";
        private string _levelOneFireAttackNameStage2 = "LiOnElementFire12";
        private string _levelOneFireAttackNameStage3 = "LiOnElementFire13";

        private string _levelTwoFireAttackNameStage1 = "LiOnElementFire21";
        private string _levelTwoFireAttackNameStage2 = "LiOnElementFire22";
        private string _levelTwoFireAttackNameStage3 = "LiOnElementFire23";
        private string _levelTwoFireAttackNameStage4 = "LiOnElementFire24";

        private string _levelThreeFireAttackNameStage1 = "LiOnElementFire31";
        private string _levelThreeFireAttackNameStage2 = "LiOnElementFire32";
        private string _levelThreeFireAttackNameStage3 = "LiOnElementFire33";
        private string _levelThreeFireAttackNameStage4 = "LiOnElementFire34";

        public Fire(ElementSystem elementSystem, int elementLevel, Entity entity, NeonStarInput input, GameScreen world)
            :base(elementSystem, elementLevel, entity, input, world)
        {
            _cooldownDuration = 4.0f;
            EffectElement = Element.Fire;
        }

        public override void PreUpdate(GameTime gameTime)
        {
            switch(State)
            {
                case ElementState.Initialization:
                case ElementState.Charge:
                    _elementSystem.AvatarComponent.thirdPersonController.CanMove = false;
                    _elementSystem.AvatarComponent.thirdPersonController.CanTurn = false;
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
                    if(!_elementSystem.entity.rigidbody.isGrounded) _elementSystem.AvatarComponent.AirLock(_airLockDuration);
                    break;

                case ElementState.Charge:
                    if (Neon.Input.Check(_input))
                    {
                        _maxChargeTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (_maxChargeTimer <= 0.0f)
                        {
                            _elementSystem.AvatarComponent.AirLock(0.0f);
                            _elementSystem.entity.rigidbody.body.LinearVelocity = Vector2.Zero;

                            switch (_input)
                            {
                                case NeonStarInput.UseLeftSlotElement:
                                    _elementSystem.LeftSlotCooldownTimer = _cooldownDuration;
                                    break;

                                case NeonStarInput.UseRightSlotElement:
                                    _elementSystem.RightSlotCooldownTimer = _cooldownDuration;
                                    break;
                            }
                            State = ElementState.End;
                        }
                        if (_charge < _maxCharge && !_chargeGoingDown)
                        {
                            _charge += gameTime.ElapsedGameTime.TotalSeconds * _chargeSpeed;
                            if (_charge >= _maxCharge)
                            {
                                _charge = _maxCharge;
                                _chargeGoingDown = true;
                            }
                        }
                        else if(_charge > 0.0f)
                        {
                            _charge -= gameTime.ElapsedGameTime.TotalSeconds * _chargeSpeed;
                            if (_charge <= 0.0f)
                            {
                                _charge = 0.0f;
                                _chargeGoingDown = false;
                            }
                        }
                    }
                    else if(Neon.Input.Released(_input))
                    {
                        _elementSystem.AvatarComponent.AirLock(0.0f);
                        _elementSystem.entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                        switch(_elementLevel)
                        {
                            case 1:
                                if (_charge > _levelOneStageThreeChargeRequired)
                                {
                                    Attack a = AttacksManager.StartFreeAttack(_levelOneFireAttackNameStage3, _entity.spritesheets.CurrentSide, _entity.transform.Position);
                                    a.Launcher = _entity;
                                    a.EffectElement = Element.Fire;
                                }
                                else if (_charge > _levelOneStageTwoChargeRequired)
                                {
                                    Attack a = AttacksManager.StartFreeAttack(_levelOneFireAttackNameStage2, _entity.spritesheets.CurrentSide, _entity.transform.Position);
                                    a.Launcher = _entity;
                                    a.EffectElement = Element.Fire;
                                }
                                else
                                {
                                    Attack a = AttacksManager.StartFreeAttack(_levelOneFireAttackNameStage1, _entity.spritesheets.CurrentSide, _entity.transform.Position);
                                    a.Launcher = _entity;
                                    a.EffectElement = Element.Fire;
                                }
                                break;

                            case 2:
                                if (_charge > _levelTwoStageFourChargeRequired)
                                {
                                    Attack a = AttacksManager.StartFreeAttack(_levelTwoFireAttackNameStage4, _entity.spritesheets.CurrentSide, _entity.transform.Position);
                                    a.Launcher = _entity;
                                    a.EffectElement = Element.Fire;
                                }
                                else if (_charge > _levelTwoStageThreeChargeRequired)
                                {
                                    Attack a = AttacksManager.StartFreeAttack(_levelTwoFireAttackNameStage3, _entity.spritesheets.CurrentSide, _entity.transform.Position);
                                    a.Launcher = _entity;
                                    a.EffectElement = Element.Fire;
                                }
                                else if (_charge > _levelTwoStageTwoChargeRequired)
                                {
                                    Attack a = AttacksManager.StartFreeAttack(_levelTwoFireAttackNameStage2, _entity.spritesheets.CurrentSide, _entity.transform.Position);
                                    a.Launcher = _entity;
                                    a.EffectElement = Element.Fire;
                                }
                                else
                                {
                                    Attack a = AttacksManager.StartFreeAttack(_levelTwoFireAttackNameStage1, _entity.spritesheets.CurrentSide, _entity.transform.Position);
                                    a.Launcher = _entity;
                                    a.EffectElement = Element.Fire;
                                }
                                break;

                            case 3:
                                if (_charge > _levelThreeStageFourChargeRequired)
                                {
                                    Attack a = AttacksManager.StartFreeAttack(_levelThreeFireAttackNameStage4, _entity.spritesheets.CurrentSide, _entity.transform.Position);
                                    a.Launcher = _entity;
                                    a.EffectElement = Element.Fire;
                                }
                                else if (_charge > _levelThreeStageThreeChargeRequired)
                                {
                                    Attack a = AttacksManager.StartFreeAttack(_levelThreeFireAttackNameStage3, _entity.spritesheets.CurrentSide, _entity.transform.Position);
                                    a.Launcher = _entity;
                                    a.EffectElement = Element.Fire;
                                }
                                else if (_charge > _levelThreeStageTwoChargeRequired)
                                {
                                    Attack a = AttacksManager.StartFreeAttack(_levelThreeFireAttackNameStage2, _entity.spritesheets.CurrentSide, _entity.transform.Position);
                                    a.Launcher = _entity;
                                    a.EffectElement = Element.Fire;
                                }
                                else
                                {
                                    Attack a = AttacksManager.StartFreeAttack(_levelThreeFireAttackNameStage1, _entity.spritesheets.CurrentSide, _entity.transform.Position);
                                    a.Launcher = _entity;
                                    a.EffectElement = Element.Fire;
                                }
                                break;
                        }

                        switch(_input)
                        {
                            case NeonStarInput.UseLeftSlotElement:
                                _elementSystem.LeftSlotCooldownTimer = _cooldownDuration;
                                break;

                            case NeonStarInput.UseRightSlotElement:
                                _elementSystem.RightSlotCooldownTimer = _cooldownDuration;
                                break;
                        }
                        State = ElementState.End;
                    }
                    break;

                case ElementState.Effect:
                    break;

                case ElementState.End:
                    _elementSystem.CurrentElementEffect = null;
                    _elementSystem.CanUseElement = true;
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
