using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Private;
using NeonStarLibrary.Components.GameplayElements;

namespace NeonStarLibrary.Components.Scripts
{
    public class MoveLoop : ScriptComponent
    {
        private MovingGeometry _movingGeometry;
        private Node _startNode;
        private Node _targetNode;

        public MoveLoop(Entity entity)
            : base(entity, "MoveLoop")
        {
        }

        public override void Init()
        {
            _movingGeometry = entity.GetComponent<MovingGeometry>();
            if (_movingGeometry.CurrentNodeList.Nodes.Count >= 2)
            {
                _startNode = _movingGeometry.CurrentNodeList.Nodes[0];
                _targetNode = _movingGeometry.CurrentNodeList.Nodes[1];
            }
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Vector2.DistanceSquared(this.entity.transform.Position, _targetNode.Position) < 50.0f*50.0f)
            {
                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                this.entity.transform.Position = _startNode.Position;
            }
            base.Update(gameTime);
        }
    }
}
