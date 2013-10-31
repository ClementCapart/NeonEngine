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

        public Entity EntityAttacked;

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

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {           
            if (EntityFollowed != null)
            {
                if (ShouldDetectAgain && EnemyComponent.State != EnemyState.Wait && EnemyComponent.State != EnemyState.MustFinishChase)
                {
                    if (Vector2.DistanceSquared(EntityFollowed.transform.Position, entity.transform.Position) < AttackRange * AttackRange)
                    {
                        EnemyComponent.State = EnemyState.Attack;
                    }
                    else if (Vector2.DistanceSquared(this.entity.transform.Position, EntityFollowed.transform.Position) < ThreatRange * ThreatRange)
                    {
                        if(EnemyComponent.State != EnemyState.Attack || ( EnemyComponent._attack != null && EnemyComponent._attack.CurrentAttack == null))
                            EnemyComponent.State = EnemyState.Chase;
                    }
                    else if (EnemyComponent.State == EnemyState.Chase || (EnemyComponent.State == EnemyState.Attack && EnemyComponent._attack != null && EnemyComponent._attack.CurrentAttack != null))
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
            else
            {
                foreach (Entity ent in entity.containerWorld.entities.Where(e => e.Name == _entityToSearchFor))
                {
                    if (Vector2.DistanceSquared(ent.transform.Position, entity.transform.Position) < ThreatRange * ThreatRange)
                    {
                        EntityFollowed = ent;
                        if (EnemyComponent.State != EnemyState.Attack || EnemyComponent._attack.CurrentAttack == null)
                            EnemyComponent.State = EnemyState.Chase;
                    }

                    if (Vector2.DistanceSquared(ent.transform.Position, entity.transform.Position) < AttackRange * AttackRange)
                    {
                        EntityFollowed = ent;
                        EnemyComponent.State = EnemyState.Attack;
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}
