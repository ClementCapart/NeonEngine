﻿using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    class MovingGeometry : Component
    {
        enum MovingState 
        {
            Wait,
            Move
        }

        #region Properties
        private PathNodeList _currentNodeList;

        public PathNodeList CurrentNodeList
        {
            get { return _currentNodeList; }
            set
            {
                _currentNodeList = value;
                Init();
            }
        }

        private float _speed = 5f;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private bool _roundtrip = true;

        public bool Roundtrip
        {
            get { return _roundtrip; }
            set { _roundtrip = value; }
        }

        private bool _reverseStart;

        public bool ReverseStart
        {
            get { return _reverseStart; }
            set { _reverseStart = value; }
        }

        private float _pathPrecisionTreshold;

        public float PathPrecisionTreshold
        {
            get { return _pathPrecisionTreshold; }
            set { _pathPrecisionTreshold = value; }
        }
        #endregion

        public bool Active = true;

        private Node _nextNode;
        private Node _previousNode;
        private bool _reverse;
        private float _currentNodeDelay = 0.0f;

        private MovingState _movingState = MovingState.Move;

        public MovingGeometry(Entity entity)
            :base(entity, "MovingGeometry")
        {
        }

        public override void Init()
        {
            if (_currentNodeList != null)
            {
                if (_currentNodeList.Type == PathType.Platform)
                {
                    Node CloserNode = CurrentNodeList.Nodes[0];

                    for (int i = 1; i < CurrentNodeList.Nodes.Count; i++)
                    {
                        if (Vector2.Distance(CurrentNodeList.Nodes[i].Position, entity.transform.Position) < Vector2.Distance(CloserNode.Position, entity.transform.Position))
                        {
                            CloserNode = CurrentNodeList.Nodes[i];
                        }
                    }
                    _nextNode = CloserNode;
                }
                _reverseStart = _reverse;
            }
            base.Init();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_currentNodeList != null)
            {
                switch (_movingState)
                {
                    case MovingState.Wait:
                        _currentNodeDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (_currentNodeDelay <= 0.0f)
                        {
                            _currentNodeDelay = 0.0f;
                            _movingState = MovingState.Move;
                            SearchNextNode();
                        }
                        break;

                    case MovingState.Move:
                        if (_currentNodeList.Type == PathType.Platform)
                        {
                            if (Vector2.Distance(_nextNode.Position, entity.transform.Position) <= _pathPrecisionTreshold)
                            {
                                switch (_nextNode.Type)
                                {
                                    case NodeType.Move:
                                        SearchNextNode();
                                        break;

                                    case NodeType.DelayedMove:
                                        _currentNodeDelay = _nextNode.NodeDelay;
                                        _movingState = MovingState.Wait;
                                        break;
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
            if (_movingState == MovingState.Move)
            {
                if (_nextNode != null)
                {
                    this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(-_speed, this.entity.rigidbody.body.LinearVelocity.Y);
                }
            }

            base.Update(gameTime);
        }

        private void SearchNextNode()
        {
            this._previousNode = this._nextNode;

            int newIndex = 0;
            int oldIndex = _currentNodeList.Nodes.IndexOf(_previousNode);
            if (_roundtrip)
            {
                if (oldIndex == 0)
                {
                    newIndex = 1;
                    _reverse = false;
                }
                else if (oldIndex == _currentNodeList.Nodes.Count - 1)
                {
                    newIndex = _currentNodeList.Nodes.Count - 2;
                    _reverse = true;
                }
                else
                {
                    newIndex = oldIndex + (_reverse ? -1 : 1);
                }

            }
            this._nextNode = _currentNodeList.Nodes[newIndex];
        }
    }
}