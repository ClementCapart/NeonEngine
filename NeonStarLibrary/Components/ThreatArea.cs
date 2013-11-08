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
        public Enemy EnemyComponent;
        public Entity EntityFollowed;
        public bool ShouldDetectAgain = true;

        private float _threatRange = 300f;

        public float ThreatRange
        {
            get { return _threatRange; }
            set { _threatRange = value; }
        }

        private float _attackRange = 150f;

        public float AttackRange
        {
            get { return _attackRange; }
            set { _attackRange = value; }
        }

        private string _entityToSearchFor= "";

        public string EntityToSearchFor
        {
            get { return _entityToSearchFor; }
            set { _entityToSearchFor = value; }
        }

        private float _waitBeforeChasingDelay = 0.0f;

        public float WaitBeforeChasingDelay
        {
            get { return _waitBeforeChasingDelay; }
            set { _waitBeforeChasingDelay = value; }
        }

        private float _waitBeforeAttackingDelay = 0.0f;

        public float WaitBeforeAttackingDelay
        {
            get { return _waitBeforeAttackingDelay; }
            set { _waitBeforeAttackingDelay = value; }
        }

        private float _waitTimer = 0.0f;

        public ThreatArea(Entity entity)
            :base(entity, "ThreatArea")
        {
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<Enemy>();
            if (EnemyComponent != null)
                EnemyComponent._threatArea = this;
            base.Init();
        }

        public override void PreUpdate(GameTime gameTime)
        {
            if (_waitTimer > 0.0f)
            {
                _waitTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_waitTimer < 0.0f)
                    _waitTimer = 0.0f;
            }

            if (EntityFollowed != null)
            {
                if (ShouldDetectAgain && EnemyComponent.State != EnemyState.MustFinishChase)
                {
                    if (Vector2.DistanceSquared(EntityFollowed.transform.Position, entity.transform.Position) < AttackRange * AttackRange)
                    {
                        if (EnemyComponent.State == EnemyState.WaitBeforeChase)
                        {
                            EnemyComponent.State = EnemyState.WaitBeforeAttack;
                        }

                        if (_waitBeforeAttackingDelay <= 0.0f || (EnemyComponent.State == EnemyState.WaitBeforeAttack && _waitTimer <= 0.0f))
                        {
                            EnemyComponent.State = EnemyState.Attack;
                            if (entity.spritesheets != null)
                                entity.spritesheets.ChangeSide(EntityFollowed.transform.Position.X < entity.transform.Position.X ? Side.Left : Side.Right);
                        }
                        else if (EnemyComponent.State != EnemyState.WaitBeforeAttack && EnemyComponent.State != EnemyState.Attack)
                        {
                            EnemyComponent.State = EnemyState.WaitBeforeAttack;
                            _waitTimer = _waitBeforeAttackingDelay;
                        }
                        
                    }
                    else if (Vector2.DistanceSquared(this.entity.transform.Position, EntityFollowed.transform.Position) < ThreatRange * ThreatRange)
                    {
                        if ((EnemyComponent._attack != null && EnemyComponent._attack.CurrentAttack == null) || EnemyComponent._attack == null)
                        {
                            if (EnemyComponent.State == EnemyState.WaitBeforeAttack)
                            {
                                EnemyComponent.State = EnemyState.WaitBeforeChase;
                            }

                            if (_waitBeforeChasingDelay <= 0.0f || (EnemyComponent.State == EnemyState.WaitBeforeChase && _waitTimer <= 0.0f))
                            {
                                EnemyComponent.State = EnemyState.Chase;
                            }
                            else if (EnemyComponent.State != EnemyState.WaitBeforeChase && EnemyComponent.State != EnemyState.Chase)
                            {
                                EnemyComponent.State = EnemyState.WaitBeforeChase;
                                _waitTimer = _waitBeforeChasingDelay;
                            }
                        }
                    }
                    else if (EnemyComponent.State == EnemyState.Chase || (EnemyComponent.State == EnemyState.Attack && EnemyComponent._attack != null && EnemyComponent._attack.CurrentAttack != null) || (EnemyComponent.State == EnemyState.WaitBeforeChase && _waitTimer <= 0.0f))
                    {
                        EnemyComponent.State = EnemyState.MustFinishChase;
                    }
                }
                else
                {
                    Rigidbody rg = EntityFollowed.rigidbody.beacon.CheckGround();
                    Rigidbody rg2 = this.entity.rigidbody.beacon.CheckGround();
                    if (rg != null && rg2 != null && rg == rg2)
                    {
                        ShouldDetectAgain = true;
                    }
                }
            }   
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {      
            if(EntityFollowed == null)
                foreach (Entity ent in entity.containerWorld.entities.Where(e => e.Name == _entityToSearchFor))
                    EntityFollowed = ent;
         
            base.Update(gameTime);
        }
    }
}
