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

        private float _waitThreatDelay = 0.0f;

        public float WaitThreatDelay
        {
            get { return _waitThreatDelay; }
            set { _waitThreatDelay = value; }
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
                        if (_waitThreatDelay <= 0.0f || (EnemyComponent.State != EnemyState.Idle && EnemyComponent.State != EnemyState.Patrol && EnemyComponent.State != EnemyState.WaitThreat && EnemyComponent.State != EnemyState.Attack) || (EnemyComponent.State == EnemyState.WaitThreat && _waitTimer <= 0.0f))
                        {
                            EnemyComponent.State = EnemyState.Attack;
                            if (entity.spritesheets != null)
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
                        if ((EnemyComponent._attack != null && EnemyComponent._attack.CurrentAttack == null) || EnemyComponent._attack == null)
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
                    else if (EnemyComponent.State == EnemyState.Chase || (EnemyComponent.State == EnemyState.Attack && EnemyComponent._attack != null && EnemyComponent._attack.CurrentAttack != null) || (EnemyComponent.State == EnemyState.WaitThreat && _waitTimer <= 0.0f))
                    {
                        EnemyComponent.State = EnemyState.MustFinishChase;
                    }
                    else if (EnemyComponent.State == EnemyState.WaitThreat)
                    {
                        EnemyComponent.State = EnemyState.Idle;
                        _waitTimer = 0.0f;
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
