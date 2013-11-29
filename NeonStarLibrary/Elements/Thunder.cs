using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class Thunder : ElementEffect
    {
        private float _impulseForce = 1300.0f;
        private float _dashDuration = 0.15f;
        private SpriteSheetInfo _thunderEffectSpritesheetInfo = null;

        private string _attackToLaunch = "Empty";
        private Attack ThunderAttack;
        private AnimatedSpecialEffect effect;

        public Thunder(ElementSystem elementSystem, int elementLevel, Entity entity, NeonStarInput input, GameScreen world)
            :base(elementSystem, elementLevel, entity, input, world)
        {
            this.EffectElement = Element.Thunder;
            _cooldownDuration = 1.0f;
            _thunderEffectSpritesheetInfo = AssetManager.GetSpriteSheet("LiOnThunderDashFX");
        }

        public override void PreUpdate(GameTime gameTime)
        {
            switch (State)
            {
                case ElementState.Initialization:
                case ElementState.Charge:
                case ElementState.Effect:
                    _elementSystem.AvatarComponent.CanMove = false;
                    _elementSystem.AvatarComponent.CanTurn = false;
                    _elementSystem.AvatarComponent.CanAttack = false;
                    break;
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch(State)
            {
                case ElementState.Initialization:
                    State = ElementState.Charge;
                    if (_entity.rigidbody != null)
                    {
                        if (_entity.rigidbody.beacon != null)
                        {
                            switch (_elementSystem.AvatarComponent.CurrentSide)
                            {
                                case Side.Left:
                                    if (_entity.rigidbody.beacon.CheckLeftSide(50) != null)
                                    {
                                        _dashDuration = 0.0f;
                                        _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                                        State = ElementState.End;
                                    }
                                    break;

                                case Side.Right:
                                    if (_entity.rigidbody.beacon.CheckRightSide(50) != null)
                                    {
                                        _dashDuration = 0.0f;
                                        _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                                        State = ElementState.End;
                                    }
                                    break;
                            }

                        }
                    }
                    if(State == ElementState.Charge)
                        effect = EffectsManager.GetEffect(_thunderEffectSpritesheetInfo, _elementSystem.AvatarComponent.CurrentSide, new Vector2(_elementSystem.entity.transform.Position.X, _elementSystem.entity.transform.Position.Y), 0.0f, new Vector2(_thunderEffectSpritesheetInfo.FrameWidth / 2 - 120, -80), 0.9f);
                    break;

                case ElementState.Charge:
                    
                    Neon.world.camera.ChaseStrength = 0.0f;
                    _entity.rigidbody.GravityScale = 0.0f;
                    _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                    _entity.rigidbody.body.ApplyLinearImpulse(new Vector2(_impulseForce * (_entity.spritesheets.CurrentSide == Side.Right ? 1 : -1), 0));
                    
                    State = ElementState.Effect;
                    ThunderAttack = AttacksManager.GetAttack(_attackToLaunch, _elementSystem.AvatarComponent.CurrentSide, _entity);
                    if (_entity.rigidbody != null)
                    {
                        if (_entity.rigidbody.beacon != null)
                        {
                            switch (_elementSystem.AvatarComponent.CurrentSide)
                            {
                                case Side.Left:
                                    if (_entity.rigidbody.beacon.CheckLeftSide(50) != null)
                                    {
                                        _dashDuration = 0.0f;
                                        _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                                        effect.Destroy();
                                        State = ElementState.End;
                                    }
                                    break;

                                case Side.Right:
                                    if (_entity.rigidbody.beacon.CheckRightSide(50) != null)
                                    {
                                        _dashDuration = 0.0f;
                                        _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                                        effect.Destroy();
                                        State = ElementState.End;
                                    }
                                    break;
                            }
                        }
                    }
                    break;

                case ElementState.Effect:
                    if (_dashDuration > 0.0f)
                    {
                        _entity.rigidbody.GravityScale = 0.0f;
                        _dashDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (ThunderAttack != null) ThunderAttack.Update(gameTime);
                    }
                    
                    if(_dashDuration <= 0.0f)
                    {
                        _dashDuration = 0.0f;
                        _entity.rigidbody.body.LinearVelocity = Vector2.Zero;

                        if (ThunderAttack != null) ThunderAttack.CancelAttack();
                        ThunderAttack = null;
                        State = ElementState.End;
                    }

                    if (_entity.rigidbody != null)
                    {
                        if (_entity.rigidbody.beacon != null)
                        {
                            switch (_elementSystem.AvatarComponent.CurrentSide)
                            {
                                case Side.Left:
                                    if (_entity.rigidbody.beacon.CheckLeftSide(20) != null)
                                    {
                                        _dashDuration = 0.0f;
                                        _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                                        effect.Destroy();
                                        State = ElementState.End;
                                    }
                                    break;

                                case Side.Right:
                                    if (_entity.rigidbody.beacon.CheckRightSide(20) != null)
                                    {
                                        _dashDuration = 0.0f;
                                        _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                                        effect.Destroy();
                                        State = ElementState.End;
                                    }
                                    break;
                            }

                        }
                    }

                    switch (_input)
                    {
                        case NeonStarInput.UseLeftSlotElement:
                            _elementSystem.LeftSlotCooldownTimer = _cooldownDuration;
                            break;

                        case NeonStarInput.UseRightSlotElement:
                            _elementSystem.RightSlotCooldownTimer = _cooldownDuration;
                            break;
                    }

                    break;

                case ElementState.End:
                    break;
            }
            base.Update(gameTime);
        }
    }
}
