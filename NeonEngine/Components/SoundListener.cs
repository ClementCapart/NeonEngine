using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Audio
{
    public class SoundListener : Component
    {
        #region Properties

        #endregion

        private AudioListener _audioListener;

        public SoundListener(Entity entity)
            :base(entity, "SoundListener")
        {   
        }

        public override void Init()
        {
            _audioListener = new AudioListener();
            base.Init();
        }
    }
}
