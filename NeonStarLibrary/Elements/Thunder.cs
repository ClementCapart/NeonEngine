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
        private SpriteSheetInfo _thunderEffectSpritesheetInfo = null;
    
        private AnimatedSpecialEffect effect;
        
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
            _thunderEffectSpritesheetInfo = AssetManager.GetSpriteSheet("LiOnThunderDashFX");
            
            this.EffectElement = Element.Thunder;
            switch (_elementLevel)
            {
                case 1:
                    _gaugeCost = (float)ElementManager.ThunderParameters[0][0];
                    _dashImpulse = new Vector2(_entity.spritesheets.CurrentSide == Side.Right ? (float)ElementManager.ThunderParameters[0][1] : -(float)ElementManager.ThunderParameters[0][1], 0);
                    _dashDuration = (float)ElementManager.ThunderParameters[0][2];
                    _attackToLaunch = (string)ElementManager.ThunderParameters[0][3];
                    break;

                case 2:
                    _gaugeCost = (float)ElementManager.ThunderParameters[1][0];
                    if (Neon.Input.Check(NeonStarInput.MoveUp))
                        _dashImpulse = new Vector2(0, -(float)ElementManager.ThunderParameters[1][4]);
                    else if (Neon.Input.Check(NeonStarInput.MoveDown))
                        _dashImpulse = new Vector2(0, (float)ElementManager.ThunderParameters[1][5]);
                    else
                        _dashImpulse = new Vector2(_entity.spritesheets.CurrentSide == Side.Right ? (float)ElementManager.ThunderParameters[1][1] : -(float)ElementManager.ThunderParameters[1][1], 0);
                    _dashDuration = (float)ElementManager.ThunderParameters[1][2];
                    _attackToLaunch = (string)ElementManager.ThunderParameters[1][3];
                    break;

                case 3:
                    _gaugeCost = (float)ElementManager.ThunderParameters[2][0];
                    if (Neon.Input.Check(NeonStarInput.MoveUp))
                        _dashImpulse = new Vector2(0, -(float)ElementManager.ThunderParameters[2][4]);
                    else if (Neon.Input.Check(NeonStarInput.MoveDown))
                        _dashImpulse = new Vector2(0, (float)ElementManager.ThunderParameters[2][5]);
                    else
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
                    if(State == ElementState.Charge)
                    effect = EffectsManager.GetEffect(_thunderEffectSpritesheetInfo, _elementSystem.AvatarComponent.CurrentSide, _elementSystem.entity.transform.Position, (float)Math.PI / 2, new Vector2(_thunderEffectSpritesheetInfo.FrameWidth / 2 - 120, -80), 2.0f , 0.9f);
                    _entity.hitboxes[0].SwitchType(HitboxType.Invincible, _dashDuration);
                    break;

                case ElementState.Charge:                    
                    Neon.World.Camera.ChaseStrength = 0.0f;
                    _entity.rigidbody.GravityScale = 0.0f;
                    _entity.rigidbody.body.LinearVelocity = Vector2.Zero;                       
                    State = ElementState.Effect;
                    ThunderAttack = AttacksManager.GetAttack(_attackToLaunch, _elementSystem.AvatarComponent.CurrentSide, _entity);
                    _entity.rigidbody.body.ApplyLinearImpulse(_dashImpulse);  
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

                    break;

                case ElementState.End:
                    ThunderAttack.CancelAttack();
                    ThunderAttack = null;
                    break;
            }
            base.Update(gameTime);
        }
    }
}
