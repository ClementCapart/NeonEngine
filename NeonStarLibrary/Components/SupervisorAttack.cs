using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Enemies
{
    public class SupervisorAttack : EnemyAttack
    {
        #region Properties
        private float _attackRangeWidth;

        public float AttackRangeWidth
        {
            get { return _attackRangeWidth; }
            set { _attackRangeWidth = value; }
        }

        private float _attackRangeHeight;

        public float AttackRangeHeight
        {
            get { return _attackRangeHeight; }
            set { _attackRangeHeight = value; }
        }

        private Vector2 _attackRangeOffset;

        public Vector2 AttackRangeOffset
        {
            get { return _attackRangeOffset; }
            set { _attackRangeOffset = value; }
        }
        #endregion

        public SupervisorAttack(Entity entity)
            :base(entity)
        {
            Name = "SupervisorAttack";
        }

        public override void Init()
        {
            base.Init();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(EntityToAttack != null)
            {
                switch(EnemyComponent.State)
                {
                    case EnemyState.Idle:
                    case EnemyState.Patrol:
                    case EnemyState.WaitNode:
                        Rectangle attackRange = new Rectangle((int)(entity.transform.Position.X - _attackRangeWidth / 2), (int)(entity.transform.Position.Y - _attackRangeHeight / 2), (int)_attackRangeWidth, (int)_attackRangeHeight);
                        if (attackRange.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                        {
                            if (entity.rigidbody != null)
                                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                            EnemyComponent.State = EnemyState.WaitThreat;
                        }
                        break;

                    case EnemyState.WaitThreat:
                        if (EnemyComponent.WaitThreatTimer >= EnemyComponent.WaitThreatDuration)
                        {
                            EnemyComponent.State = EnemyState.Chase;
                        }
                        Rectangle attackRange2 = new Rectangle((int)(entity.transform.Position.X - _attackRangeWidth / 2), (int)(entity.transform.Position.Y - _attackRangeHeight / 2), (int)_attackRangeWidth, (int)_attackRangeHeight);
                        if (!attackRange2.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                        {
                            if (entity.rigidbody != null)
                                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                            EnemyComponent.State = EnemyState.Idle;
                            EnemyComponent.WaitThreatTimer = 0.0f;
                        }
                        break;

                    case EnemyState.Wait:
                        
                        break;
                }
            }
            
            base.PreUpdate(gameTime);
        }
    }
}
