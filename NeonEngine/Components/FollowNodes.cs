using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class FollowNodes : Component
    {
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

        private float _speed = 100f;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private bool _roundtrip;

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

        private Node _nextNode;
        private Node _previousNode;

        public FollowNodes(Entity entity)
            :base(entity, "FollowNodes")
        {
        }

        public override void Init()
        {
            if (_currentNodeList == null)
            {
                _currentNodeList = entity.containerWorld.NodeLists[0];
            }
            if (_currentNodeList.Type == PathType.Ground)
            {
                Node CloserNode = CurrentNodeList.Nodes[0];

                for (int i = 1; i < CurrentNodeList.Nodes.Count; i++ )
                {
                    if (Math.Sqrt(Math.Pow(CurrentNodeList.Nodes[i].Position.X - entity.transform.Position.X, 2)) < Math.Sqrt(Math.Pow(CloserNode.Position.X - entity.transform.Position.X, 2)))
                    {
                        CloserNode = CurrentNodeList.Nodes[i];
                    }
                }
                _nextNode = CloserNode;
            }
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_currentNodeList.Type == PathType.Ground)
            {
                if (this._nextNode.Position.X + _pathPrecisionTreshold > entity.transform.Position.X && this._nextNode.Position.X - _pathPrecisionTreshold < entity.transform.Position.X)
                    SearchNextNode();
            }
                
            
            base.Update(gameTime);
        }

        private void SearchNextNode()
        {

        }

    }
}
