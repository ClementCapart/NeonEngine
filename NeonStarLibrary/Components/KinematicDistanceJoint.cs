using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.CollisionDetection
{
    class KinematicDistanceJoint : Component
    {
        #region Properties
        private string _linkedEntityName;

        public string LinkedEntityName
        {
            get { return _linkedEntityName; }
            set { _linkedEntityName = value; }
        }

        private Vector2 _offset;

        public Vector2 Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }
        #endregion

        private Entity _linkedEntity;

        public KinematicDistanceJoint(Entity entity)
            :base(entity, "KinematicDistanceJoint")
        {
            RequiredComponents = new Type[] { typeof(Rigidbody) };
        }

        public override void Init()
        {
            _linkedEntity = entity.GameWorld.GetEntityByName(_linkedEntityName);
            base.Init();
        }

        public override void FinalUpdate(GameTime gameTime)
        {
            if (_linkedEntity != null && _linkedEntity.rigidbody != null)
                this.entity.transform.Position = _linkedEntity.transform.Position + Offset;
            base.FinalUpdate(gameTime);
        }
    }
}
