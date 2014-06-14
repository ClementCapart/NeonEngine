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
    public class ManagedSoundEmitter : SoundEmitter
    {
        #region Properties
        private new float Volume
        {
            get
            {
                return base.Volume;
            }
            set
            {
                base.Volume = value;
            }
        }

        private new bool Is3DSound
        {
            get
            {
                return base.Is3DSound;
            }
            set
            {
                base.Is3DSound = value;
            }
        }

        private new float Pitch
        {
            get
            {
                return base.Pitch;
            }
            set
            {
                base.Pitch = value;
            }
        }

        private new bool IsLooped
        {
            get
            {
                return base.IsLooped;
            }
            set
            {
                base.IsLooped = value;
            }
        }

        private List<SoundInstanceInfo> _soundList = new List<SoundInstanceInfo>();

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

        public bool PlaySound(string name)
        {
            SoundEffectInstance sei = null;
            for (int i = 0; i < _soundList.Count; i++)
            {
                if (_soundList[i].Name == name)
                {
                    SoundInstanceInfo info = _soundList[i];
                    if (info.Sound == null)
                        return false;
                    if (info.Sound.IsDisposed)
                        info.Sound = SoundManager.GetSound(info.Sound.Name);
                    if (info.Sound == null)
                        return false;       
                    sei = info.Sound.CreateInstance();
                    sei.Volume = info.Volume;
                    sei.Pitch = info.Pitch;
                    if (info.Is3DSound)
                    {
                        AudioEmitter.Position += new Vector3(info.Offset, 0);
                        sei.Apply3D(entity.GameWorld.AudioListeners.ToArray(), AudioEmitter);
                        AudioEmitter.Position -= new Vector3(info.Offset, 0);
                    }
                    sei.Play();
                    SoundInstances.Add(sei, info);
                    return true;
                }
            }
            return false;
        }

        public override void PreUpdate(GameTime gameTime)
        {
            for (int i = SoundInstances.Count - 1; i >= 0; i--)
            {
                SoundEffectInstance sei = SoundInstances.ElementAt(i).Key;
                if (sei.State == SoundState.Stopped)
                    SoundInstances.Remove(sei);
            }
            base.PreUpdate(gameTime);
        }         
    }
}
