using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.GameplayElements
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
            RequiredComponents = new Type[] { typeof(Rigidbody) };
        }

        public override void Init()
        {
            if (_currentNodeList != null)
            {
                if (_currentNodeList.Type == PathType.Platform)
                {
                   /* Node CloserNode = CurrentNodeList.Nodes[0];

                    for (int i = 1; i < CurrentNodeList.Nodes.Count; i++)
                    {
                        if (Vector2.Distance(CurrentNodeList.Nodes[i].Position, entity.transform.Position) < Vector2.Distance(CloserNode.Position, entity.transform.Position))
                        {
                            CloserNode = CurrentNodeList.Nodes[i];
                        }
                    }
                    _nextNode = CloserNode;*/
                    _nextNode = CurrentNodeList.Nodes[0];
                    
                }
                _reverseStart = _reverse;
            }
            Active = true;
            base.Init();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (entity.GameWorld.FirstUpdate)
            {
                this.entity.transform.Position = _nextNode.Position;
            }

            if (Active)
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
                                if (_nextNode != null)
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
                                                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                                                _movingState = MovingState.Wait;
                                                break;
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Active)
            {
                if (entity.rigidbody != null)
                {
                    if (_movingState == MovingState.Move)
                    {
                        if (_nextNode != null)
                        {
                            if (entity.rigidbody.body != null)
                                this.entity.rigidbody.body.LinearVelocity = Vector2.Normalize(new Vector2(_nextNode.Position.X - entity.transform.Position.X, _nextNode.Position.Y - entity.transform.Position.Y)) * _speed;
                        }
                    }
                }
            }
            else
            {
                
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

                this._nextNode = _currentNodeList.Nodes[newIndex];
            }
            else
            {
                if (oldIndex == _currentNodeList.Nodes.Count - 1)
                {
                    this.Active = false;
                    if (entity.rigidbody.body != null)
                    {
                        entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                    }
                    return;
                }
                this._nextNode = _currentNodeList.Nodes[oldIndex + 1];
            }
        }
    }
}
