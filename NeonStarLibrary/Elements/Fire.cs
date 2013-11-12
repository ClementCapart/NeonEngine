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

        private int _chargeLevel = 0;

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
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (_state)
            {
                case ElementState.Initialization:
                    _state = ElementState.Charge;
                    break;

                case ElementState.Charge:
                    if (Neon.Input.Check(_input))
                    {
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
                        switch(_elementLevel)
                        {
                            case 1:
                                if (_charge > _levelOneStageThreeChargeRequired)
                                {
                                    _chargeLevel = 3;
                                    AttacksManager.StartFreeAttack(_levelOneFireAttackNameStage3, _entity.spritesheets.CurrentSide, _entity.transform.Position).Launcher = _entity;
                                }
                                else if (_charge > _levelOneStageTwoChargeRequired)
                                {
                                    _chargeLevel = 2;
                                    AttacksManager.StartFreeAttack(_levelOneFireAttackNameStage2, _entity.spritesheets.CurrentSide, _entity.transform.Position).Launcher = _entity;
                                }
                                else
                                {
                                    _chargeLevel = 1;
                                    AttacksManager.StartFreeAttack(_levelOneFireAttackNameStage1, _entity.spritesheets.CurrentSide, _entity.transform.Position).Launcher = _entity;
                                }
                                break;

                            case 2:
                                if (_charge > _levelTwoStageFourChargeRequired)
                                {
                                    _chargeLevel = 4;
                                    AttacksManager.StartFreeAttack(_levelTwoFireAttackNameStage4, _entity.spritesheets.CurrentSide, _entity.transform.Position)._entity = _entity;
                                }
                                else if (_charge > _levelTwoStageThreeChargeRequired)
                                {
                                    _chargeLevel = 3;
                                    AttacksManager.StartFreeAttack(_levelTwoFireAttackNameStage3, _entity.spritesheets.CurrentSide, _entity.transform.Position)._entity = _entity;
                                }
                                else if (_charge > _levelTwoStageTwoChargeRequired)
                                {
                                    _chargeLevel = 2;
                                    AttacksManager.StartFreeAttack(_levelTwoFireAttackNameStage2, _entity.spritesheets.CurrentSide, _entity.transform.Position)._entity = _entity;
                                }
                                else
                                {
                                    _chargeLevel = 1;
                                    AttacksManager.StartFreeAttack(_levelTwoFireAttackNameStage1, _entity.spritesheets.CurrentSide, _entity.transform.Position)._entity = _entity;
                                }
                                break;

                            case 3:
                                if (_charge > _levelThreeStageFourChargeRequired)
                                {
                                    _chargeLevel = 4;
                                    AttacksManager.StartFreeAttack(_levelThreeFireAttackNameStage4, _entity.spritesheets.CurrentSide, _entity.transform.Position)._entity = _entity;
                                }
                                else if (_charge > _levelThreeStageThreeChargeRequired)
                                {
                                    _chargeLevel = 3;
                                    AttacksManager.StartFreeAttack(_levelThreeFireAttackNameStage3, _entity.spritesheets.CurrentSide, _entity.transform.Position)._entity = _entity;
                                }
                                else if (_charge > _levelThreeStageTwoChargeRequired)
                                {
                                    _chargeLevel = 2;
                                    AttacksManager.StartFreeAttack(_levelThreeFireAttackNameStage2, _entity.spritesheets.CurrentSide, _entity.transform.Position)._entity = _entity;
                                }
                                else
                                {
                                    _chargeLevel = 1;
                                    AttacksManager.StartFreeAttack(_levelThreeFireAttackNameStage1, _entity.spritesheets.CurrentSide, _entity.transform.Position)._entity = _entity;
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
                        _state = ElementState.End;
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
