using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NeonEngine.Components.Audio;
using NeonEngine.Components.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Audio
{
    public class SoundInstanceInfo
    {
        public string Name = "";
        public SoundEffect Sound;
        public float Volume;
        public float Pitch;
        public bool Is3DSound;
        public Vector2 Offset;
    }

    public class ManagedSoundEmitter : SoundEmitter
    {
        #region Properties
        private List<SoundInstanceInfo> _soundList;

        public List<SoundInstanceInfo> SoundList
        {
            get { return _soundList; }
            set { _soundList = value; }
        }
        #endregion

        public ManagedSoundEmitter(Entity entity)
            :base(entity)
        {
            Name = "ManagedSoundEmitter";
        }
    }
}
