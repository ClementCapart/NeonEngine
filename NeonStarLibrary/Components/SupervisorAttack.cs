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
                        break;
                }
            }
            
            base.PreUpdate(gameTime);
        }
    }
}
