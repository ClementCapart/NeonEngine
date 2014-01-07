using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class SpawnPoint
    {
        public Vector2 Position;
        public Side Side;


        private int _index;

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
    }
}
