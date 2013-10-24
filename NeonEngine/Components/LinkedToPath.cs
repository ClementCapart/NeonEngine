using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class LinkedToPath : Component
    {
        private PathNodeList _linkedPathNodeList;

        public PathNodeList LinkedPathNodeList
        {
            get { return _linkedPathNodeList; }
            set { _linkedPathNodeList = value; }
        }

        public LinkedToPath(Entity entity)
            :base(entity, "LinkedToPath")
        {

        }
    }
}
