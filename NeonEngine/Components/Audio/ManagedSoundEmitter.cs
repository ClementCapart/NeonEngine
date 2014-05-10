using NeonEngine.Components.Audio;
using NeonEngine.Components.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Audio
{
    public class ManagedSoundEmitter : SoundEmitter
    {
        public ManagedSoundEmitter(Entity entity)
            :base(entity)
        {
            Name = "ManagedSoundEmitter";
        }
    }
}
