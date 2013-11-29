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
        private float _cooldownDuration = 1.0f;




        public Thunder(ElementSystem elementSystem, int elementLevel, Entity entity, NeonStarInput input, GameScreen world)
            :base(elementSystem, elementLevel, entity, input, world)
        {
            this.EffectElement = Element.Thunder;
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
                    break;

                case ElementState.Charge:
                    _entity.rigidbody.GravityScale = 0.0f;
                    _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                    _entity.rigidbody.body.ApplyLinearImpulse(new Vector2(_impulseForce * (_entity.spritesheets.CurrentSide == Side.Right ? 1 : -1), 0));
                    EffectsManager.GetEffect(_thunderEffectSpritesheetInfo, _elementSystem.AvatarComponent.CurrentSide, new Vector2(_elementSystem.entity.transform.Position.X, _elementSystem.entity.transform.Position.Y), 0, new Vector2(_thunderEffectSpritesheetInfo.FrameWidth / 2, 0), 0.9f);
                    State = ElementState.Effect;
                    break;

                case ElementState.Effect:
                    if (_dashDuration > 0.0f)
                    {
                        _entity.rigidbody.GravityScale = 0.0f;
                        _dashDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    
                    if(_dashDuration <= 0.0f)
                    {
                        _dashDuration = 0.0f;
                        _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                        State = ElementState.End;
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
