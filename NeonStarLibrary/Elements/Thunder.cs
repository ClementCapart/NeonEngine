using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using NeonStarLibrary.Components.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class Thunder : ElementEffect
    {
        private SpriteSheetInfo _thunderEffectSpritesheetInfo = null;
        private SpriteSheetInfo _thunderFinishSpritesheetInfo = null;
    
        private AnimatedSpecialEffect effect;
        private AnimatedSpecialEffect finishEffect;
        
        private Vector2 _dashImpulse = Vector2.Zero;
        private float _dashDuration = 0.15f;
        private string _attackToLaunch = "";
        private Attack ThunderAttack;

        public Thunder(ElementSystem elementSystem, int elementLevel, Entity entity, NeonStarInput input, GameScreen world)
            :base(elementSystem, elementLevel, entity, input, world)
        {           
        }

        public override void InitializeLevelParameters()
        {
            _thunderEffectSpritesheetInfo = AssetManager.GetSpriteSheet("LiOnThunderDashLink");
            _thunderFinishSpritesheetInfo = AssetManager.GetSpriteSheet("LiOnThunderDashFinish");
            
            this.EffectElement = Element.Thunder;
            switch (ElementLevel)
            {
                case 1:
                    _gaugeCost = (float)ElementManager.ThunderParameters[0][0];
                    _dashImpulse = new Vector2(_entity.spritesheets.CurrentSide == Side.Right ? (float)ElementManager.ThunderParameters[0][1] : -(float)ElementManager.ThunderParameters[0][1], 0);
                    _dashDuration = (float)ElementManager.ThunderParameters[0][2];
                    _attackToLaunch = (string)ElementManager.ThunderParameters[0][3];
                    break;

                case 2:
                    _gaugeCost = (float)ElementManager.ThunderParameters[1][0];
                    /*if (Neon.Input.Check(NeonStarInput.MoveUp))
                        _dashImpulse = new Vector2(0, -(float)ElementManager.ThunderParameters[1][4]);
                    else if (Neon.Input.Check(NeonStarInput.MoveDown))
                        _dashImpulse = new Vector2(0, (float)ElementManager.ThunderParameters[1][5]);
                    else*/
                        _dashImpulse = new Vector2(_entity.spritesheets.CurrentSide == Side.Right ? (float)ElementManager.ThunderParameters[1][1] : -(float)ElementManager.ThunderParameters[1][1], 0);
                    _dashDuration = (float)ElementManager.ThunderParameters[1][2];
                    _attackToLaunch = (string)ElementManager.ThunderParameters[1][3];
                    break;

                case 3:
                    _gaugeCost = (float)ElementManager.ThunderParameters[2][0];
                    /*if (Neon.Input.Check(NeonStarInput.MoveUp))
                        _dashImpulse = new Vector2(0, -(float)ElementManager.ThunderParameters[2][4]);
                    else if (Neon.Input.Check(NeonStarInput.MoveDown))
                        _dashImpulse = new Vector2(0, (float)ElementManager.ThunderParameters[2][5]);
                    else*/
                        _dashImpulse = new Vector2(_entity.spritesheets.CurrentSide == Side.Right ? (float)ElementManager.ThunderParameters[2][1] : -(float)ElementManager.ThunderParameters[2][1], 0);
                    _dashDuration = (float)ElementManager.ThunderParameters[2][2];
                    _attackToLaunch = (string)ElementManager.ThunderParameters[2][3];
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
                case ElementState.Effect:
                    _elementSystem.AvatarComponent.CanMove = false;
                    _elementSystem.AvatarComponent.CanTurn = false;
                    _elementSystem.AvatarComponent.CanAttack = false;
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
                    if(_entity.spritesheets != null)
                        _entity.spritesheets.ChangeAnimation(_elementSystem.ThunderLaunchAnimation, true, 0, true, false, false);
                    if (_entity.rigidbody != null)
                    {
                        _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                        _entity.rigidbody.GravityScale = 0.0f;
                    }
                    if(_entity.hitboxes.Count > 0)
                        _entity.hitboxes[0].SwitchType(HitboxType.Invincible, _dashDuration);
                    if (_entity.spritesheets != null && _entity.spritesheets.IsFinished())
                    {
                        _entity.spritesheets.Active = false;
                        State = ElementState.Charge;
                        if (effect == null)
                        {
                            float angle = 0.0f;
                            if (_dashImpulse.X == 0)
                            {
                                if (_dashImpulse.Y < 0)
                                    angle = (float)-Math.PI / 2;
                                else
                                    angle = (float)Math.PI / 2;
                                
                            }

                            Vector2 offset;
                            if (_dashImpulse.X == 0)
                            {
                                if (_dashImpulse.Y < 0)
                                    offset = new Vector2(4, -177);
                                else
                                    offset = new Vector2(4, 200);
                                
                            }
                            else
                            {
                                offset = new Vector2(189, 12);
                            }
                            effect = EffectsManager.GetEffect(_thunderEffectSpritesheetInfo, _dashImpulse.X != 0 ? _elementSystem.AvatarComponent.CurrentSide : Side.Right, _elementSystem.entity.transform.Position, angle, offset, 2.0f, 0.7f);
                        }
                    }
                    
                    break;

                case ElementState.Charge:
                    _entity.rigidbody.GravityScale = 0.0f;                                   
                    State = ElementState.Effect;                 
                    _entity.rigidbody.body.ApplyLinearImpulse(_dashImpulse);
                    ThunderAttack = AttacksManager.GetAttack(_attackToLaunch, _elementSystem.AvatarComponent.CurrentSide, _entity);
                    break;

                case ElementState.Effect:
                    if (finishEffect == null)
                    {
                        if (_dashDuration > 0.0f)
                        {
                            _entity.rigidbody.GravityScale = 0.0f;
                            _dashDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                            if (ThunderAttack != null) ThunderAttack.Update(gameTime);
                        }
                        if (_dashDuration <= 0.0f)
                        {
                            _dashDuration = 0.0f;
                            _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                            if (ThunderAttack != null) ThunderAttack.CancelAttack();
                            ThunderAttack = null;
                            finishEffect = EffectsManager.GetEffect(_thunderFinishSpritesheetInfo, _elementSystem.AvatarComponent.CurrentSide, _entity.transform.Position, 0.0f, Vector2.Zero, 2.0f, 0.9f);
                            switch (_input)
                            {
                                case NeonStarInput.UseLeftSlotElement:
                                    _elementSystem.LeftSlotEnergy -= _gaugeCost;
                                    break;

                                case NeonStarInput.UseRightSlotElement:
                                    _elementSystem.RightSlotEnergy -= _gaugeCost;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (finishEffect != null && finishEffect.spriteSheet != null && finishEffect.spriteSheet.currentFrame == finishEffect.spriteSheet.spriteSheetInfo.FrameCount - 1)
                        {
                            State = ElementState.End;
                        }
                    }
                    break;

                case ElementState.End:  
                    if(ThunderAttack != null)
                        ThunderAttack.CancelAttack();
                    ThunderAttack = null;
                    break;
            }

            if (ThunderAttack != null)
                ThunderAttack.Update(gameTime);
            base.Update(gameTime);
        }
    }
}
