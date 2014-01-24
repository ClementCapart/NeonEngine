using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    class SupervisorChase : Chase
    {
        #region Properties
        private float _detectionWidth;

        public float DetectionWidth
        {
            get { return _detectionWidth; }
            set { _detectionWidth = value; }
        }

        private float _detectionHeight;

        public float DetectionHeight
        {
            get { return _detectionHeight; }
            set { _detectionHeight = value; }
        }

        private Vector2 _detectionBoxOffset;

        public Vector2 DetectionBoxOffset
        {
            get { return _detectionBoxOffset; }
            set { _detectionBoxOffset = value; }
        }
        #endregion

        private bool _onDuty = false;
        private Avatar _avatar;

        public SupervisorChase(Entity entity)
            :base(entity)
        {
            Name = "SupervisorChase";
            
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<Enemy>();
            this.EntityToChase = entity.containerWorld.GetEntityByName(_entityToChaseName);
            if (EntityToChase != null)
                _avatar = EntityToChase.GetComponent<Avatar>();
            base.Init();
        }

        public override void PreUpdate(GameTime gameTime)
        {
            if (_avatar != null)
            {
                switch (EnemyComponent.State)
                {
                    case EnemyState.Idle:
                    case EnemyState.Patrol:
                    case EnemyState.WaitNode:
                    case EnemyState.Wait:
                        Rectangle detectionHitbox = new Rectangle((int)(entity.transform.Position.X + _detectionBoxOffset.X - _detectionWidth / 2), (int)(entity.transform.Position.Y + _detectionBoxOffset.Y - _detectionHeight / 2), (int)_detectionWidth, (int)_detectionHeight);
                        if (detectionHitbox.Intersects(EntityToChase.hitboxes[0].hitboxRectangle))
                        {
                            if (_onDuty)
                            {
                                EnemyComponent.State = EnemyState.Chase;
                            }
                            else
                            {
                                foreach (Entity e in entity.containerWorld.Entities)
                                {
                                    if (e != _avatar.entity && e!= entity && e.hitboxes.Count > 0 && e.hitboxes[0].hitboxRectangle.Intersects(detectionHitbox) && e.hitboxes[0].Type == HitboxType.Main)
                                    {
                                        Enemy enemy = e.GetComponent<Enemy>();
                                        if (enemy != null)
                                            if (enemy.TookDamageLastFrame)
                                                Console.WriteLine("Detected ! ");
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }


    }
}
