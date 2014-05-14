using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components;
using NeonEngine.Components.CollisionDetection;
using NeonStarLibrary.Components.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class Fire : ElementEffect
    {
        public static List<Rigidbody> FirePlatforms = new List<Rigidbody>();

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

        private Entity _firePlatform;

        public Fire(ElementSystem elementSystem, ElementSlot elementSlot, Entity entity, NeonStarInput input, GameScreen world)
            :base(elementSystem, elementSlot, entity, input, world)
        {
            EffectElement = Element.Fire;
        }

        public override void InitializeLevelParameters()
        {
            _chargeSpeed = (float)ElementManager.FireParameters[0][0];
            _maxChargeTimer = (float)ElementManager.FireParameters[0][1];

            StageOneAttack = (string)ElementManager.FireParameters[1][1];
            StageTwoAttack = (string)ElementManager.FireParameters[1][2];
            StageThreeAttack = (string)ElementManager.FireParameters[1][3];

            StageTwoThreshold = (float)ElementManager.FireParameters[1][4];
            StageThreeThreshold = (float)ElementManager.FireParameters[1][5];
            
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
                    _elementSystem.AvatarComponent.CanUseElement = false;
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
                    _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                    _entity.rigidbody.body.GravityScale = 0.0f;
                    State = ElementState.Charge;
                    break;

                case ElementState.Charge:
                    _entity.rigidbody.body.GravityScale = 0.0f;
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

                    Attack a = null;

                    if (CurrentCharge > StageThreeThreshold)
                    {
                        a = AttacksManager.StartFreeAttack(StageThreeAttack, _entity.spritesheets.CurrentSide, _entity.transform.Position, false);
                        a.Launcher = _entity;
                    }
                    else if (CurrentCharge > StageTwoThreshold)
                    {
                        a = AttacksManager.StartFreeAttack(StageTwoAttack, _entity.spritesheets.CurrentSide, _entity.transform.Position, false);
                        a.Launcher = _entity;
                    }
                    else
                    {
                        a = AttacksManager.StartFreeAttack(StageOneAttack, _entity.spritesheets.CurrentSide, _entity.transform.Position, false);
                        a.Launcher = _entity;
                    }
                    if (a != null)
                    {
                        Entity e = new Entity(_entity.GameWorld);
                        e.transform.Scale = 2.0f;
                        e.transform.Position = _entity.transform.Position + new Vector2(a.AttackInfo.Hitboxes[0].X * (a.CurrentSide == Side.Right ? 1 : -1), a.AttackInfo.Hitboxes[0].Y);
                        e.Layer = "Lock";

                        Hitbox h = new Hitbox(e);
                        h.ID = 1;
                        h.Width = a.AttackInfo.Hitboxes[0].Width;
                        h.Height = a.AttackInfo.Hitboxes[0].Height;
                        h.Type = HitboxType.OneWay;
                        h.Init();
                        e.AddComponent(h);
                        
                        Rigidbody r = new Rigidbody(e);
                        r.ID = 2;
                        r.BodyType = BodyType.Static;
                        r.Friction = 0.3f;                
                        r.IsGround = true;
                        r.Init();
                        r.Hitbox = h;
                        e.AddComponent(r);
                        r.OneWayPlatform = true;

                        AutoDestruction ad = new AutoDestruction(e);
                        ad.ID = 3;
                        ad.DestructionTimer = a.AttackInfo.Delay + a.AttackInfo.Duration;
                        ad.Init();
                        e.AddComponent(ad);

                        _entity.GameWorld.AddEntity(e);

                        FirePlatforms.Add(e.rigidbody);
                        _firePlatform = e;                       
                    }

                    _elementSlot.Cooldown = _elementSystem.ElementSlotCooldownDuration;

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
