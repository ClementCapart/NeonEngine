using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class ThreatArea : Component
    {
        #region Properties
        private bool _mustBeOnSamePlatform = true;

        public bool MustBeOnSamePlatform
        {
            get { return _mustBeOnSamePlatform; }
            set { _mustBeOnSamePlatform = value; }
        }

        private float _threatRange = 300f;

        public float ThreatRange
        {
            get { return _threatRange; }
            set { _threatRange = value; }
        }

        private float _threatAngle = 360.0f;

        public float ThreatAngle
        {
            get { return _threatAngle; }
            set { _threatAngle = value; }
        }

        private float _attackRange = 150f;

        public float AttackRange
        {
            get { return _attackRange; }
            set { _attackRange = value; }
        }

        private string _entityToSearchFor = "";

        public string EntityToSearchFor
        {
            get { return _entityToSearchFor; }
            set { _entityToSearchFor = value; }
        }

        private float _waitThreatDelay = 0.0f;

        public float WaitThreatDelay
        {
            get { return _waitThreatDelay; }
            set { _waitThreatDelay = value; }
        }
        #endregion       

        public Enemy EnemyComponent;
        public Entity EntityFollowed;
        public bool ShouldDetect = false;

        private float _waitTimer = 0.0f;

        public ThreatArea(Entity entity)
            :base(entity, "ThreatArea")
        {
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<Enemy>();
            if (EnemyComponent != null)
                EnemyComponent.ThreatArea = this;
            base.Init();
        }

        public override void PreUpdate(GameTime gameTime)
        {
            if (EntityFollowed == null)
                foreach (Entity ent in entity.containerWorld.entities.Where(e => e.Name == _entityToSearchFor))
                    EntityFollowed = ent;

            if (_waitTimer > 0.0f)
            {
                _waitTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_waitTimer < 0.0f)
                    _waitTimer = 0.0f;
            }
            if (EntityFollowed != null && _mustBeOnSamePlatform)
            {
                Rigidbody rg = EntityFollowed.rigidbody.beacon.CheckGround();
                Rigidbody rg2 = this.entity.rigidbody.beacon.CheckGround();
                if (rg != null && rg2 != null && rg == rg2)
                {
                    ShouldDetect = true;
                }
            }

            if (EnemyComponent.State != EnemyState.StunLocked && EnemyComponent.State != EnemyState.Dying && EnemyComponent.State != EnemyState.Dead)
            {
                if (EntityFollowed != null)
                {
                    if ((ShouldDetect || !_mustBeOnSamePlatform) && EnemyComponent.State != EnemyState.MustFinishChase)
                    {
                        float angle = MathHelper.ToDegrees(Neon.utils.AngleBetween(EntityFollowed.transform.Position, entity.transform.Position));
                        float lowerLimit = entity.transform.Rotation;
                        if (lowerLimit < 0) lowerLimit = 360 + lowerLimit;
                        float upperLimit = entity.transform.Rotation + _threatAngle;
                        if (upperLimit < 0) upperLimit = 360 + upperLimit;

                        if (lowerLimit > upperLimit)
                        {
                            float limit = upperLimit;
                            upperLimit = lowerLimit;
                            lowerLimit = upperLimit;
                        }

                        if (Vector2.DistanceSquared(EntityFollowed.transform.Position, entity.transform.Position) < AttackRange * AttackRange && (angle > lowerLimit && angle < upperLimit))
                        {
                            if (_waitThreatDelay <= 0.0f || (EnemyComponent.State != EnemyState.Idle && EnemyComponent.State != EnemyState.Patrol && EnemyComponent.State != EnemyState.WaitThreat) || (EnemyComponent.State == EnemyState.WaitThreat && _waitTimer <= 0.0f))
                            {
                                EnemyComponent.State = EnemyState.Attacking;
                                if (entity.spritesheets != null && (EnemyComponent.State != EnemyState.Attacking || (EnemyComponent.State == EnemyState.Attacking && EnemyComponent.Attack.CurrentAttack == null)))
                                    entity.spritesheets.ChangeSide(EntityFollowed.transform.Position.X < entity.transform.Position.X ? Side.Left : Side.Right);

                            }
                            else if (EnemyComponent.State == EnemyState.Idle || EnemyComponent.State == EnemyState.Patrol)
                            {
                                EnemyComponent.State = EnemyState.WaitThreat;
                                _waitTimer = _waitThreatDelay;
                            }
                        }
                        else if (Vector2.DistanceSquared(this.entity.transform.Position, EntityFollowed.transform.Position) < ThreatRange * ThreatRange)
                        {
                            if ((EnemyComponent.Attack != null && EnemyComponent.Attack.CurrentAttack == null) || EnemyComponent.Attack == null)
                            {
                                if (_waitThreatDelay <= 0.0f || (EnemyComponent.State != EnemyState.Idle && EnemyComponent.State != EnemyState.Patrol && EnemyComponent.State != EnemyState.WaitThreat) || (EnemyComponent.State == EnemyState.WaitThreat && _waitTimer <= 0.0f))
                                {
                                    EnemyComponent.State = EnemyState.Chase;
                                }
                                else if (EnemyComponent.State == EnemyState.Idle || EnemyComponent.State == EnemyState.Patrol)
                                {
                                    EnemyComponent.State = EnemyState.WaitThreat;
                                    _waitTimer = _waitThreatDelay;
                                }
                            }
                        }
                        else if (EnemyComponent.State == EnemyState.Chase || (EnemyComponent.State == EnemyState.Attacking && EnemyComponent.Attack != null && EnemyComponent.Attack.CurrentAttack != null) || (EnemyComponent.State == EnemyState.WaitThreat && _waitTimer <= 0.0f))
                        {
                            if (EnemyComponent.Chase != null)
                                EnemyComponent.State = EnemyState.MustFinishChase;
                        }
                        else if (EnemyComponent.State == EnemyState.WaitThreat)
                        {
                            _waitTimer = 0.0f;
                        }
                    }
                    else if (EnemyComponent.State != EnemyState.MustFinishChase && EnemyComponent.State != EnemyState.WaitNode && EnemyComponent.State != EnemyState.Wait)
                    {
                        if (EnemyComponent.FollowNodes != null)
                            EnemyComponent.State = EnemyState.Patrol;
                    }
                }
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {      
            base.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {        
            ShouldDetect = false;
            base.PostUpdate(gameTime);
        }
    }
}
