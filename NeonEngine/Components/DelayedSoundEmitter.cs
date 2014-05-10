using NeonEngine.Components.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Audio
{
    public class DelayedSoundEmitter : SoundEmitter
    {
        #region Properties
        private float _startDelay = 0.0f;

        public float StartDelay
        {
            get { return _startDelay; }
            set { _startDelay = value; }
        }

        #endregion

        private float _currentTimer = 0.0f;

        public DelayedSoundEmitter(Entity entity)
            :base(entity)
        {
            Name = "DelayedSoundEmitter";
        }

        public override void Init()
        {
            base.Init();
            if (_startDelay > 0.0f && _currentSoundInstance != null && !_currentSoundInstance.IsDisposed)
            {
                _currentSoundInstance.Stop();
            }

            _currentTimer = 0.0f;
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.PreUpdate(gameTime);
            if (_currentSoundInstance != null)
            {
                if (_currentTimer >= _startDelay)
                {
                    _currentSoundInstance.Play();
                }
                else
                {
                    _currentSoundInstance.Stop();
                    _currentTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            
        }
    }
}
