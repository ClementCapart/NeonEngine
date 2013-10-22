using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public enum NodeType
    {
        Move,
        Jump,
        DelayedMove
    }

    public class Node
    {
        public Vector2 Position;
        public NodeType Type;
        public int index;

        public Node()
        {

        }
    }
}
