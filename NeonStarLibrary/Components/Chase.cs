using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public abstract class Chase : Component
    {
        #region Properties
        protected string _entityToChaseName = "";

        public string EntityToChaseName
        {
            get { return _entityToChaseName; }
            set { _entityToChaseName = value; }
        }

        protected float _detectionDistance = 1.0f;
        
        public float DetectionDistance
        {
            get { return _detectionDistance; }
            set { _detectionDistance = value; }
        }

        protected float _waitStopDuration = 1.0f;

        public float WaitStopDuration
        {
            get { return _waitStopDuration; }
            set { _waitStopDuration = value; }
        }

        protected float _chaseSpeed = 5.0f;

        public float ChaseSpeed
        {
            get { return _chaseSpeed; }
            set { _chaseSpeed = value; }
        }

        protected float _chasePrecision = 0.0f;

        public float ChasePrecision
        {
          get { return _chasePrecision; }
          set { _chasePrecision = value; }
        }
        #endregion

        public Enemy EnemyComponent = null;

        public Entity EntityToChase = null;

        protected Vector2 _positionToReach = new Vector2(-9999, -9999);     
        protected float _waitStopTimer = 0.0f;

        public Chase(Entity entity)
            :base(entity, "Chase")
        {}
    }
}
