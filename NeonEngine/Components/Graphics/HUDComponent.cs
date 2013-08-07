using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class HUDComponent : DrawableComponent
    {
        public HUDComponent(Entity entity, float Layer, string Name)
            :base(0, entity, "HUDComponent")
        {

        }
    }
}
