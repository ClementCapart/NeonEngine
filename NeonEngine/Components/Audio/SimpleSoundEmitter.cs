using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NeonEngine.Components.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Audio
{
    public class SimpleSoundEmitter : SoundEmitter
    {
        #region Properties
        private string _playingSoundTag = "";

        public string PlayingSoundTag
        {
            get { return _playingSoundTag; }
            set
            {
                _playingSoundTag = value;
                _playingSoundEffect = SoundManager.GetSound(_playingSoundTag);
                if (_currentSoundInstance != null)
                    _currentSoundInstance.Stop();
                if (_playingSoundEffect != null && AudioEmitter != null)
                {
                    _currentSoundInstance = _playingSoundEffect.CreateInstance();
                    _currentSoundInstance.IsLooped = IsLooped;
                    if (Is3DSound) _currentSoundInstance.Apply3D(entity.GameWorld.AudioListeners.ToArray(), AudioEmitter);
                    _currentSoundInstance.Volume = 0.0f;
                    _currentSoundInstance.Pitch = MathHelper.Clamp(Pitch, -1.0f, 1.0f);
                    _currentSoundInstance.Play();
                    SoundInstances.Add(_currentSoundInstance, null);

                }
            }
        }

        public override bool Is3DSound
        {
            get { return _is3DSound; }
            set 
            { 
                _is3DSound = value;
                if (_is3DSound && _currentSoundInstance != null && AudioEmitter != null)
                {
                    _currentSoundInstance.Stop();
                    SoundInstances.Remove(_currentSoundInstance);
                    if (_playingSoundEffect != null && AudioEmitter != null)
                    {
                        _currentSoundInstance = _playingSoundEffect.CreateInstance();
                        _currentSoundInstance.IsLooped = _isLooped;
                        _currentSoundInstance.Apply3D(entity.GameWorld.AudioListeners.ToArray(), AudioEmitter);
                        _currentSoundInstance.Volume = 0.0f;
                        _currentSoundInstance.Pitch = MathHelper.Clamp(_pitch, -1.0f, 1.0f);
                        _currentSoundInstance.Play();
                        SoundInstances.Add(_currentSoundInstance, null);
                    }
                }
                else if (!_is3DSound)
                {
                    if (_currentSoundInstance != null)
                    {
                        _currentSoundInstance.Stop();
                        SoundInstances.Remove(_currentSoundInstance);
                    }
                    if (_playingSoundEffect != null && AudioEmitter != null)
                    {
                        _currentSoundInstance = _playingSoundEffect.CreateInstance();
                        _currentSoundInstance.IsLooped = _isLooped;
                        _currentSoundInstance.Volume = 0.0f;
                        _currentSoundInstance.Pitch = MathHelper.Clamp(_pitch, -1.0f, 1.0f);
                        _currentSoundInstance.Play();
                        SoundInstances.Add(_currentSoundInstance, null);
                    }
                }
            }
        }

        public override bool IsLooped
        {
            get { return _isLooped; }
            set
            {
                _isLooped = value;
                if (_currentSoundInstance != null)
                {
                    _currentSoundInstance.Stop();
                    SoundInstances.Remove(_currentSoundInstance);
                    if (_playingSoundEffect != null && AudioEmitter != null)
                    {
                        _currentSoundInstance = _playingSoundEffect.CreateInstance();
                        _currentSoundInstance.Apply3D(entity.GameWorld.AudioListeners.ToArray(), AudioEmitter);
                        _currentSoundInstance.IsLooped = _isLooped;
                        _currentSoundInstance.Volume = 0.0f;
                        _currentSoundInstance.Pitch = MathHelper.Clamp(_pitch, -1.0f, 1.0f);
                        _currentSoundInstance.Play();
                        SoundInstances.Add(_currentSoundInstance, null);
                    }
                }
            }
        }

        public override float Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                if (_currentSoundInstance != null)
                    _currentSoundInstance.Volume = MathHelper.Clamp(_volume, 0.0f, 1.0f);
            }
        }

        public override float Pitch
        {
            get { return _pitch; }
            set 
            { 
                _pitch = value;
                if (_currentSoundInstance != null)
                    _currentSoundInstance.Pitch = MathHelper.Clamp(_pitch, -1.0f, 1.0f);
            }
        }
        #endregion

        protected SoundEffect _playingSoundEffect;
        protected SoundEffectInstance _currentSoundInstance;

        public SimpleSoundEmitter(Entity entity)
            :base(entity)
        {
            Name = "SimpleSoundEmitter";
        }

        public override void Init()
        {
            base.Init();

            if (_currentSoundInstance != null)
                _currentSoundInstance.Stop();
            _playingSoundEffect = SoundManager.GetSound(_playingSoundTag);
            if (_playingSoundEffect != null)
                _currentSoundInstance = _playingSoundEffect.CreateInstance();
            if (_currentSoundInstance != null)
            {
                _currentSoundInstance.Volume = MathHelper.Clamp(_currentVolume, 0.0f, 1.0f);
                _currentSoundInstance.Pitch = MathHelper.Clamp(Pitch, -1.0f, 1.0f);
                _currentSoundInstance.IsLooped = IsLooped;
                if (Is3DSound)
                    _currentSoundInstance.Apply3D(entity.GameWorld.AudioListeners.ToArray(), AudioEmitter);
                _currentSoundInstance.Play();
            }

            if (_currentSoundInstance != null)
               SoundInstances.Add(_currentSoundInstance, null);
            
        }

        public override void PreUpdate(GameTime gameTime)
        {
            if (_currentSoundInstance != null && !_currentSoundInstance.IsDisposed)
                _currentSoundInstance.Volume = MathHelper.Clamp(_currentVolume, 0.0f, 1.0f);

            if (_playingSoundEffect != null && _playingSoundEffect.IsDisposed)
            {
                _playingSoundEffect = SoundManager.GetSound(_playingSoundTag);
                if (_playingSoundEffect != null)
                    _currentSoundInstance = _playingSoundEffect.CreateInstance();

                if (_currentSoundInstance != null)
                {
                    _currentSoundInstance.Volume = MathHelper.Clamp(_currentVolume, 0.0f, 1.0f);
                    _currentSoundInstance.Pitch = MathHelper.Clamp(Pitch, -1.0f, 1.0f);
                    _currentSoundInstance.IsLooped = IsLooped;
                    if (Is3DSound)
                        _currentSoundInstance.Apply3D(entity.GameWorld.AudioListeners.ToArray(), AudioEmitter);
                    _currentSoundInstance.Play();
                }
            }
            base.PreUpdate(gameTime);
        }

        public override void Remove()
        {          
            base.Remove();
            if (_currentSoundInstance != null)
                _currentSoundInstance.Stop();
        }

        public override void OnChangeLevel()
        {
            if (_currentSoundInstance != null && !_currentSoundInstance.IsDisposed)
                _currentSoundInstance.Stop();
            base.OnChangeLevel();
        }

    }
}
