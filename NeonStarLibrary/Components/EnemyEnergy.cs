using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Enemies
{
    
    public class EnemyEnergy : Component
    {
        #region Properties
        private string _startAnimationName = "";

        public string StartAnimationName
        {
            get { return _startAnimationName; }
            set { _startAnimationName = value; }
        }

        private string _movingAnimationName = "";

        public string MovingAnimationName
        {
            get { return _movingAnimationName; }
            set { _movingAnimationName = value; }
        }

        private string _completeAnimationName = "";

        public string CompleteAnimationName
        {
            get { return _completeAnimationName; }
            set { _completeAnimationName = value; }
        }

        private float _moveSpeed = 100.0f;

        public float MoveSpeed
        {
            get { return _moveSpeed; }
            set { _moveSpeed = value; }
        }

        private float _targetThreshold = 10.0f;

        public float TargetThreshold
        {
            get { return _targetThreshold; }
            set { _targetThreshold = value; }
        }
        #endregion

        public Vector2 TargetPosition;
        private bool _startMoving = false;
        private Vector2 _direction;
        public bool FinishedTraveling = false;
        public bool ShouldBeDestroyed = false;
        public bool WaitingToBeDestroyed = false;

        AnimatedSpecialEffect _startEffect;

        public EnemyEnergy(Entity entity)
            :base(entity, "EnemyEnergy")
        {
            RequiredComponents = new Type[] { typeof(SpritesheetManager) };
        }

        public override void Init()
        {
            if (entity.spritesheets != null)
            {
                if (_startEffect == null)
                    _startEffect = EffectsManager.GetEffect(entity.spritesheets.SpritesheetList[StartAnimationName], Side.Right, Vector2.Zero, 0.0f, Vector2.Zero, 2.0f, 0.89f);
                else
                    _startEffect.transform.Position = entity.transform.Position;
                //entity.spritesheets.ChangeAnimation(StartAnimationName, 0, true, false, false);
                _direction = Vector2.Normalize(new Vector2(TargetPosition.X - entity.transform.Position.X, TargetPosition.Y - entity.transform.Position.Y));
                entity.spritesheets.RotationOffset = (float)Math.Atan2(_direction.Y, _direction.X);
            }
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            if (entity.spritesheets != null)
            {
                //if (entity.spritesheets.CurrentSpritesheetName == StartAnimationName && entity.spritesheets.IsFinished())
                if(_startEffect != null && (_startEffect.spriteSheet.currentFrame == 7 || _startEffect.spriteSheet.spriteSheetInfo.FrameCount < 8))
                {
                    entity.spritesheets.ChangeAnimation(MovingAnimationName, 0, true, false, true);
                    _startMoving = true;
                }

                if (FinishedTraveling && entity.spritesheets.CurrentSpritesheetName == CompleteAnimationName && entity.spritesheets.IsFinished())
                {
                    ShouldBeDestroyed = true;
                }
            }

            if (_startMoving)
            {
                entity.transform.Position += _direction * _moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if(new Rectangle((int)(TargetPosition.X - _targetThreshold), (int)(TargetPosition.Y - _targetThreshold), (int)_targetThreshold * 2,(int)_targetThreshold * 2).Contains(new Point((int)entity.transform.Position.X, (int)entity.transform.Position.Y)))
            {
                _startMoving = false;
                entity.spritesheets.ChangeAnimation(CompleteAnimationName, 0, true, false, false);
                FinishedTraveling = true;
            }

            base.Update(gameTime);
        }
    }
}
