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
        private bool _needAirWaveFade = false;

        public bool NeedAirWaveFade
        {
            get { return _needAirWaveFade; }
            set { _needAirWaveFade = value; }
        }

        private MovingGeometry _movingGeometry;
        private Node _startNode;
        private Node _targetNode;
        private Node _currentTargetNode;
        private SpriteSheetInfo _fadeEffect;
        private Enemies.EnemyCore _enemyCore;

        public MoveLoop(Entity entity)
            : base(entity, "MoveLoop")
        {
        }

        public override void Init()
        {
            _movingGeometry = entity.GetComponent<MovingGeometry>();
            if (_movingGeometry.CurrentNodeList != null && _movingGeometry.CurrentNodeList.Nodes.Count >= 2)
            {
                _startNode = _movingGeometry.CurrentNodeList.Nodes[0];
                _targetNode = _movingGeometry.CurrentNodeList.Nodes[1];
            }
            _fadeEffect = AssetManager.GetSpriteSheet("WindAnimFrontFade");
            _enemyCore = entity.GetComponent<Enemies.EnemyCore>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_targetNode != null && Vector2.DistanceSquared(this.entity.transform.Position, _targetNode.Position) < 50.0f*50.0f)
            {
                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                if (_fadeEffect != null && _enemyCore != null)
                    EffectsManager.GetEffect(_fadeEffect, _enemyCore.CurrentSide, entity.transform.Position, 0.0f, new Vector2(-30, 0), 2.0f, 0.45f);
                this.entity.transform.Position = _startNode.Position;
            }
            if (_movingGeometry.CurrentNodeList != null && _movingGeometry.CurrentNodeList.Nodes.Count >= 2)
            {
                _currentTargetNode = _movingGeometry.GetCurrentTargetNode();
                if(_currentTargetNode == _movingGeometry.CurrentNodeList.Nodes[0])
                {
                    _currentTargetNode = _movingGeometry.CurrentNodeList.Nodes[1];
                    _movingGeometry.SetCurrentTargetNode(_currentTargetNode);
                }
            }
            base.Update(gameTime);
        }
    }
}
