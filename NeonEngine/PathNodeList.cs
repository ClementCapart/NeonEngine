using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public enum PathType
    {
        Ground,
        Aerial,
        Platform
    }

    public class PathNodeList
    {
        public List<Node> Nodes;
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public PathType Type;

        public PathNodeList()
        {
            Nodes = new List<Node>();
        }
    }
}
